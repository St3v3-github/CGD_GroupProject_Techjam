using UnityEngine;

namespace WarriorAnims
{
	public class WarriorMovementController:SuperStateMachine
	{
		[Header("Components")]
		private WarriorController warriorController;

		[Header("Movement")]
		public float movementAcceleration = 90.0f;
		public float walkSpeed = 4f;
		public float runSpeed = 6f;
		private readonly float rotationSpeed = 40f;
		public float groundFriction = 50f;
		[HideInInspector] public Vector3 currentVelocity;
		[HideInInspector] public bool crouch;
		[HideInInspector] public bool dropping;

		[Header("Jumping")]
		public float gravity = 25.0f;
		public float jumpAcceleration = 5.0f;
		public float jumpHeight = 3.0f;
		public float doubleJumpHeight = 4f;
		private bool doublejumped = false;
		public float inAirSpeed = 6f;

		[Header("Debug")]
		public WarriorState warriorState;
		public bool debugMessages = true;

		[HideInInspector] public Vector3 lookDirection { get; private set; }

		private void Start()
		{
			warriorController = GetComponent<WarriorController>();

			// Set currentState to Idle on start.
			currentState = WarriorState.Idle;
		}

		#region Updates

		/*void Update () {
		 * Update is normally run once on every frame update. We won't be using it in this case, since the SuperCharacterController
		 * component sends a callback Update called SuperUpdate. SuperUpdate is recieved by the SuperStateMachine, and then fires further
		 * callbacks depending on the state
		}*/

		// Put any code in here you want to run BEFORE the state's update function. This is run regardless of what state you're in.
		protected override void EarlyGlobalSuperUpdate()
		{
		}

		// Put any code in here you want to run AFTER the state's update function.  This is run regardless of what state you're in.
		protected override void LateGlobalSuperUpdate()
		{
			// Move the player by our velocity every frame.
			transform.position += currentVelocity * warriorController.superCharacterController.deltaTime;

			// If alive and is moving, set animator.
			if (!warriorController.isDead && warriorController.canMove && !warriorController.isBlocking) {
				if (currentVelocity.magnitude > 0 && warriorController.HasMoveInput()) {
					warriorController.isMoving = true;
					warriorController.SetAnimatorBool("Moving", true);
					warriorController.SetAnimatorFloat("Velocity Z", currentVelocity.magnitude);
				}
				else {
					warriorController.isMoving = false;
					warriorController.SetAnimatorBool("Moving", false);
					warriorController.SetAnimatorFloat("Velocity Z", 0);
				}
			}
			// Targeting.
			if (!warriorController.isTargeting) {
				if (warriorController.HasMoveInput() && warriorController.canMove && !warriorController.isBlocking) { RotateTowardsMovementDir(); }
			}
			else { RotateTowardsTarget(warriorController.target.transform.position); }

			// Update animator with local movement values.
			warriorController.SetAnimatorFloat("Velocity X", transform.InverseTransformDirection(currentVelocity).x);
			warriorController.SetAnimatorFloat("Velocity Z", transform.InverseTransformDirection(currentVelocity).z);

			warriorState = (WarriorState)currentState;
		}

		#endregion

		#region Gravity / Jumping

		public void RotateGravity(Vector3 up)
		{ lookDirection = Quaternion.FromToRotation(transform.up, up) * lookDirection; }

		// Calculate the initial velocity of a jump based off gravity and desired maximum height attained.
		private float CalculateJumpSpeed(float jumpHeight, float gravity)
		{ return Mathf.Sqrt(2 * jumpHeight * gravity); }

		private void DoubleJump()
		{
			if (debugMessages) { Debug.Log("DoubleJump"); }
			if (!doublejumped) { warriorController.LockDoubleJump(false); }
			if (warriorController.inputJump && warriorController.canDoubleJump && !doublejumped) { currentState = WarriorState.DoubleJump; }
		}

		#endregion

		#region States

		// Below are the state functions. Each one is called based on the name of the state, so when currentState = Idle, we call Idle_EnterState.
		// If currentState = Jump, we call Jump_SuperUpdate()
		private void Idle_EnterState()
		{
			if (debugMessages) { Debug.Log("Idle_EnterState"); }
			warriorController.superCharacterController.EnableSlopeLimit();
			warriorController.superCharacterController.EnableClamping();
			doublejumped = false;
		}

		// Run every frame we are in the idle state.
		private void Idle_SuperUpdate()
		{
			if (debugMessages) { Debug.Log("Idle_SuperUpdate"); }

			// Jump.
			if (warriorController.canJump && warriorController.inputJump) {
				Debug.Log("Jump");
				currentState = WarriorState.Jump;
				return;
			}
			// Falling.
			if (!warriorController.MaintainingGround()) {
				currentState = WarriorState.Fall;
				return;
			}
			// Moving.
			if (warriorController.HasMoveInput() && warriorController.canMove && !warriorController.isBlocking) {
				currentState = WarriorState.Move;
				return;
			}
			// Apply friction to slow to a halt.
			currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, groundFriction * warriorController.superCharacterController.deltaTime);
		}

		// Run once when exit the Idle state.
		private void Idle_ExitState()
		{
			if (debugMessages) { Debug.Log("Idle_ExitState"); }
		}

		// Run once when exit the Idle state.
		private void Idle_MoveState()
		{ warriorController.SetAnimatorBool("Moving", true); }

		private void Move_EnterState()
		{
			if (debugMessages) {Debug.Log("Move_EnterState"); }
		}

		private void Move_SuperUpdate()
		{
			if (debugMessages) { Debug.Log("Move_SuperUpdate"); }

			// If Jump.
			if (warriorController.canJump && warriorController.inputJump) {
				Debug.Log("JumpMove");
				currentState = WarriorState.Jump;
				return;
			}
			// Fallling.
			if (!warriorController.MaintainingGround()) {
				currentState = WarriorState.Fall;
				return;
			}
			// Set speed determined by movement type.
			if (warriorController.HasMoveInput() && warriorController.canMove) {

				// Keep strafing animations from playing.
				warriorController.SetAnimatorFloat("Velocity X", 0F);

				// Strafing or Walking.
				if (warriorController.isTargeting) {
					currentVelocity = Vector3.MoveTowards(currentVelocity, warriorController.moveInput
						* walkSpeed, movementAcceleration * warriorController.superCharacterController.deltaTime);
					RotateTowardsTarget(warriorController.target.transform.position);
					return;
				}
				// Run.
				currentVelocity = Vector3.MoveTowards(currentVelocity, warriorController.moveInput
					* runSpeed, movementAcceleration * warriorController.superCharacterController.deltaTime);
			}
			// Not moving, Idle.
			else { currentState = WarriorState.Idle; }
		}

		private void Jump_EnterState()
		{
			if (debugMessages) { Debug.Log("Jump_EnterState"); }
			warriorController.SetAnimatorInt("Jumping", 1);
			warriorController.SetAnimatorTrigger(AnimatorTrigger.JumpTrigger);
			warriorController.superCharacterController.DisableClamping();
			warriorController.superCharacterController.DisableSlopeLimit();
			currentVelocity += warriorController.superCharacterController.up * CalculateJumpSpeed(jumpHeight, gravity);
			warriorController.LockJump(true);
			warriorController.Jump();
		}

		private void Jump_SuperUpdate()
		{
			if (debugMessages) { Debug.Log("Jump_SuperUpdate"); }

			Vector3 planarMoveDirection = Math3d.ProjectVectorOnPlane(warriorController.superCharacterController.up, currentVelocity);
			Vector3 verticalMoveDirection = currentVelocity - planarMoveDirection;

			// Jump Attack, Dropping.
			if (dropping) {
				currentState = WarriorState.Drop;
				return;
			}
			// Falling.
			if (currentVelocity.y < 0) {
				currentVelocity = planarMoveDirection;
				currentState = WarriorState.Fall;
				return;
			}

			planarMoveDirection = Vector3.MoveTowards(planarMoveDirection, warriorController.moveInput
				* inAirSpeed, jumpAcceleration * warriorController.superCharacterController.deltaTime);
			verticalMoveDirection -= warriorController.superCharacterController.up * gravity * warriorController.superCharacterController.deltaTime;

			currentVelocity = planarMoveDirection + verticalMoveDirection;
		}

		private void DoubleJump_EnterState()
		{
			if (debugMessages) { Debug.Log("DoubleJump_EnterState"); }
			currentVelocity += warriorController.superCharacterController.up * CalculateJumpSpeed(doubleJumpHeight, gravity);
			warriorController.LockDoubleJump(true);
			doublejumped = true;
			warriorController.SetAnimatorInt("Jumping", 3);
			warriorController.SetAnimatorTrigger(AnimatorTrigger.JumpTrigger);
		}

		private void DoubleJump_SuperUpdate()
		{ Jump_SuperUpdate(); }

		private void Fall_EnterState()
		{
			if (debugMessages) { Debug.Log("Fall_EnterState"); }
			warriorController.LockJump(false);
			if (!doublejumped) { warriorController.LockDoubleJump(false); }
			warriorController.superCharacterController.DisableClamping();
			warriorController.superCharacterController.DisableSlopeLimit();
			warriorController.SetAnimatorInt("Jumping", 2);
			warriorController.SetAnimatorTrigger(AnimatorTrigger.JumpTrigger);
		}

		private void Fall_SuperUpdate()
		{
			if (debugMessages) { Debug.Log("Fall_SuperUpdate"); }

			// Fallen to ground.
			if (warriorController.AcquiringGround()) {
				currentVelocity = Math3d.ProjectVectorOnPlane(warriorController.superCharacterController.up, currentVelocity);
				currentState = WarriorState.Idle;
				return;
			}
			// Jump Attack, Dropping.
			if (dropping) {
				currentState = WarriorState.Drop;
				return;
			}
			// Allow DoubleJump.
			DoubleJump();

			// Normal gravity.
			currentVelocity -= warriorController.superCharacterController.up * gravity * warriorController.superCharacterController.deltaTime;
		}

		private void Fall_ExitState()
		{
			if (debugMessages) { Debug.Log("Fall_ExitState"); }
			warriorController.SetAnimatorInt("Jumping", 0);
			warriorController.SetAnimatorTrigger(AnimatorTrigger.JumpTrigger);
			if (warriorController.AcquiringGround()) { warriorController.Land(); }
		}

		private void Drop_EnterState()
		{
			if (debugMessages) { Debug.Log("Drop_EnterState"); }
			warriorController.LockJump(true);
			warriorController.LockDoubleJump(true);
			warriorController.SetAnimatorInt("Jumping", 4);
			warriorController.SetAnimatorTrigger(AnimatorTrigger.JumpTrigger);
		}

		private void Drop_SuperUpdate()
		{
			if (debugMessages) { Debug.Log("Drop_SuperUpdate"); }

			// Fallen to ground.
			if (warriorController.AcquiringGround()) {
				currentVelocity = Math3d.ProjectVectorOnPlane(warriorController.superCharacterController.up, currentVelocity);
				currentState = WarriorState.Idle;
				return;
			}
			// 3x gravity for quick descent.
			currentVelocity -= warriorController.superCharacterController.up * gravity * 3 * warriorController.superCharacterController.deltaTime;
		}

		private void Drop_ExitState()
		{
			if (debugMessages) { Debug.Log("Drop_ExitState"); }
			warriorController.SetAnimatorInt("Action", 0);
			warriorController.SetAnimatorTrigger(AnimatorTrigger.AttackTrigger);
			if (warriorController.AcquiringGround()) { warriorController.Land(); }
			warriorController.jumpAttack = false;
			dropping = false;
			warriorController.SetAnimatorInt("Jumping", 0);
		}

		#endregion

		/// <summary>
		/// Rotate towards the direction the Warrior is moving.
		/// </summary>
		private void RotateTowardsMovementDir()
		{
			if (warriorController.moveInput != Vector3.zero)
			{
				if (debugMessages) { Debug.Log("RotateTowardsMovementDir"); }
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(warriorController.moveInput), Time.deltaTime * rotationSpeed);
			}
		}

		/// <summary>
		/// Rotate towards a point in space.  Used when Targeting/Strafing.
		/// </summary>
		private void RotateTowardsTarget(Vector3 targetPosition)
		{
			if (debugMessages) { Debug.Log("RotateTowardsTarget: " + targetPosition); }
			Quaternion targetRotation = Quaternion.LookRotation(targetPosition - new Vector3(transform.position.x, 0, transform.position.z));
			transform.eulerAngles = Vector3.up
				* Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, (rotationSpeed * Time.deltaTime) * rotationSpeed);
		}
	}
}