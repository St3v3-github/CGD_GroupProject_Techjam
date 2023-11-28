using System.Collections;
using UnityEngine;

namespace WarriorAnims
{
	public class WarriorController:SuperStateMachine
	{
		#region Components

		[Header("Components")]
		public Warrior warrior;
		public GameObject target;
		public GameObject weapon;
		private Rigidbody rb;
		[HideInInspector] public SuperCharacterController superCharacterController;
		[HideInInspector] public WarriorMovementController warriorMovementController;
		[HideInInspector] public WarriorInputController warriorInputController;
		[HideInInspector] public WarriorInputSystemController warriorInputSystemController;
		[HideInInspector] public WarriorTiming warriorTiming;
		[HideInInspector] public Animator animator;
		[HideInInspector] public WarriorIKHands ikHands;

		#endregion

		#region Inputs

		// Inputs.
		[HideInInspector] public bool inputAiming;
		[HideInInspector] public float inputAimVertical = 0;
		[HideInInspector] public float inputAimHorizontal = 0;
		[HideInInspector] public bool inputAttack;
		[HideInInspector] public bool inputAttackMove;
		[HideInInspector] public bool inputAttackRanged;
		[HideInInspector] public bool inputAttackSpecial;
		[HideInInspector] public bool inputBlock;
		[HideInInspector] public bool inputDeath;
		[HideInInspector] public bool inputJump;
		[HideInInspector] public bool inputLightHit;
		[HideInInspector] public bool inputRoll;
		[HideInInspector] public bool inputSheath;
		[HideInInspector] public bool inputTarget;
		[HideInInspector] public float inputVertical = 0;
		[HideInInspector] public float inputHorizontal = 0;

		[HideInInspector] public Vector3 moveInput;
		[HideInInspector] public Vector2 aimInput;

		private bool useInputSystem;

		public bool allowedInput { get { return _allowedInput; } }
		private bool _allowedInput = true;

		#endregion

		#region Variables

		// Variables.
		[HideInInspector] public bool isMoving;
		[HideInInspector] public bool isDead = false;
		[HideInInspector] public bool isBlocking = false;
		[HideInInspector] public bool isTargeting = false;
		[HideInInspector] public bool jumpAttack;
		[HideInInspector] public bool sheathed;
		[HideInInspector] public bool waitingOnWeapons = true;
		[HideInInspector] public bool useRootMotion = false;
		private bool canChain;
		private int attack;
		private int specialAttack;

		public bool canAction { get { return _canAction; } }
		private bool _canAction = true;

		public bool canBlock { get { return _canBlock && !sheathed; } }
		private bool _canBlock = true;

		public bool canMove { get { return _canMove; } }
		private bool _canMove = true;

		public bool canJump { get { return _canJump && !sheathed && !isDead; } }
		private bool _canJump = true;

		public bool canDoubleJump { get { return _canDoubleJump && !sheathed; } }
		private bool _canDoubleJump = false;

		public bool canRunAttack { get { return warrior == Warrior.Archer || warrior == Warrior.Crossbow || warrior == Warrior.TwoHanded; } }

		// Animation speed control. (doesn't affect lock timing)
		public float animationSpeed = 1;

		public Coroutine co;

		#endregion

		#region Initialization

		private void Awake()
		{
			// Get SuperCharacterController.
			superCharacterController = GetComponent<SuperCharacterController>();

			// Get Movement Controller.
			warriorMovementController = GetComponent<WarriorMovementController>();

			// Add Timing Controllers.
			warriorTiming = gameObject.AddComponent<WarriorTiming>();
			warriorTiming.warriorController = this;

			// Add IKHands.
			ikHands = GetComponentInChildren<WarriorIKHands>();
			if (ikHands != null) {
				if (warrior == Warrior.TwoHanded
					|| warrior == Warrior.Hammer
					|| warrior == Warrior.Crossbow
					|| warrior == Warrior.Spearman) {
					ikHands.canBeUsed = true;
					ikHands.BlendIK(true, 0, 0.25f);
				}
			}

			// Setup Animator, add AnimationEvents script.
			animator = GetComponentInChildren<Animator>();
			if (animator == null) {
				Debug.LogError("ERROR: There is no Animator component for character.");
				Debug.Break();
			}
			else {
				animator.gameObject.AddComponent<WarriorCharacterAnimatorEvents>();
				animator.GetComponent<WarriorCharacterAnimatorEvents>().warriorController = this;
				animator.gameObject.AddComponent<WarriorAnimatorParentMove>();
				animator.GetComponent<WarriorAnimatorParentMove>().animator = animator;
				animator.GetComponent<WarriorAnimatorParentMove>().warriorController = this;
				animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
				animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
			}

			// Determine input source.
			warriorInputController = GetComponent<WarriorInputController>();
			if (warriorInputController != null) {
				useInputSystem = false;
			}
			else {
				warriorInputSystemController = GetComponent<WarriorInputSystemController>();
				if (warriorInputSystemController != null) { useInputSystem = true; }
				else { Debug.LogError("No inputs!"); }
			}

			// Setup Rigidbody.
			rb = GetComponent<Rigidbody>();
			if (rb != null) { rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; }

			currentState = WarriorState.Idle;
			SwitchCollisionOn();
		}

		#endregion

		#region Input

		/// <summary>
		/// Takes input from either WarriorInputController or WarriorInputSystemController.
		/// </summary>
		private void GetInput()
		{
			if (allowedInput) {

				// Use input from WarriorInputController / Input Manager.
				if (!useInputSystem) {
					if (warriorInputController != null) {
						inputAiming = warriorInputController.inputAiming;
						aimInput = warriorInputController.aimInput;
						inputAttack = warriorInputController.inputAttack;
						inputAttackMove = warriorInputController.inputAttackMove;
						inputAttackRanged = warriorInputController.inputAttackRanged;
						inputAttackSpecial = warriorInputController.inputAttackSpecial;
						inputBlock = warriorInputController.hasBlockInput;
						inputDeath = warriorInputController.inputDeath;
						inputJump = warriorInputController.inputJump;
						inputLightHit = warriorInputController.inputLightHit;
						inputTarget = warriorInputController.hasTargetInput;
						moveInput = warriorInputController.moveInput;
					}
				}
				// Use input from WarriorInputSystemController / Warrior Input Actions.
				else {
					if (warriorInputSystemController != null) {
						inputAiming = warriorInputSystemController.inputAiming;
						aimInput = warriorInputSystemController.aimInput;
						inputAttack = warriorInputSystemController.inputAttack;
						inputAttackMove = warriorInputSystemController.inputAttackMove;
						inputAttackRanged = warriorInputSystemController.inputAttackRanged;
						inputAttackSpecial = warriorInputSystemController.inputAttackSpecial;
						inputBlock = warriorInputSystemController.inputBlock;
						inputDeath = warriorInputSystemController.inputDeath;
						inputJump = warriorInputSystemController.inputJump;
						inputLightHit = warriorInputSystemController.inputLightHit;
						inputTarget = warriorInputSystemController.inputTarget;
						moveInput = warriorInputSystemController.moveInput;
					}
				}
			}
		}

		/// <summary>
		/// Checks move input and returns if active.
		/// </summary>
		public bool HasMoveInput()
		{ return moveInput != Vector3.zero; }

		/// <summary>
		/// Checks aim input and returns if active.
		/// </summary>
		public bool HasAimInput()
		{ return aimInput != Vector2.zero; }

		/// <summary>
		/// Checks block input and returns if true/false.
		/// </summary>
		public bool HasBlockInput()
		{ return inputBlock; }

		/// <summary>
		/// Checks block input and returns if true/false.
		/// </summary>
		public bool HasTargetInput()
		{ return inputTarget; }

		/// <summary>
		/// Shuts off input from WarriorInputController or WarriorInputSystemController. GUI still enabled.
		/// </summary>
		public void AllowInput(bool b)
		{ _allowedInput = b; }

		#endregion

		#region Updates

		private void Update()
		{
			GetInput();

			// Character is on ground.
			if (MaintainingGround()) {
				Attacking();
				if (canAction) {
					Blocking();
					if (inputLightHit) { GetHit(); }
					if (!isBlocking && !sheathed) { Targeting(); }
				}
				DeathRevive();
			}
			// Character is in air.
			else {
				if (inputAttack) { JumpAttack(); }
			}

			UpdateAnimationSpeed();
		}

		/// <summary>
		/// Updates the Animator with the animation speed multiplier.
		/// </summary>
		private void UpdateAnimationSpeed()
		{ SetAnimatorFloat("AnimationSpeed", animationSpeed); }

		#endregion

		#region Combat

		/// <summary>
		/// Warrior jumps.
		/// </summary>
		public void Jump()
		{
			// Turn IK off for Crossbow Warrior.
			if (warrior == Warrior.Crossbow && ikHands != null) { ikHands.SetIKOff(); }
		}

		/// <summary>
		/// Warrior lands.
		/// </summary>
		public void Land()
		{
			LockJump(false);
			LockDoubleJump(true);

			// Turn IK on for Crossbow Warrior.
			if (warrior == Warrior.Crossbow && ikHands != null) { ikHands.BlendIK(true, 0.5f, 0.25f); }

			// Lock Warrior if JumpAttacked.
			if (jumpAttack) { Lock(true, true, true, true, 0, warriorTiming.TimingLock(warrior, "jumpattack")); }
		}

		/// <summary>
		/// The different attack types.
		/// </summary>
		private void Attacking()
		{
			if (canAction) {
				if (inputAttack) {

					// Running attack.
					if (isMoving && canRunAttack) { RunningAttack(1); }
					else { AttackChain(); }
				}
				if (inputAttackMove) { MoveAttack(1); }
				if (inputAttackRanged) { RangeAttack(1); }
				if (inputAttackSpecial) { SpecialAttack(1); }
			}

			// Running Attack.
			if (inputAttack && isMoving && canMove) { RunningAttack(1); }

			// Chain Attack.
			if (inputAttack && canChain) { AttackChain(); }
		}

		/// <summary>
		/// Regular attack, cannot be chained.
		/// </summary>
		public void Attack(int attackNumber)
		{
			if (canAction) {
				Lock(true, true, true, true, 0, warriorTiming.TimingLock(warrior, ("attack" + attackNumber.ToString())));
				SetAnimatorInt("Action", attackNumber);
				SetAnimatorTrigger(AnimatorTrigger.AttackTrigger);
				if (warrior == Warrior.Spearman && attackNumber == 4 && ikHands != null)
				{ ikHands.SetIKPause(warriorTiming.TimingLock(warrior, "attack4")); }
				if (warrior == Warrior.Spearman && attackNumber == 5 && ikHands != null)
				{ ikHands.SetIKPause(warriorTiming.TimingLock(warrior, "attack5")); }
			}
		}

		/// <summary>
		/// 3 hit combo attack chain.
		/// </summary>
		public void AttackChain()
		{
			// If charater is not in air, do regular attack.
			if (attack == 0) { StartCoroutine(_Attack1()); }

			// If within chain time.
			else if (canChain) {
				if (warrior != Warrior.Archer) {
					if (attack == 1) { StartCoroutine(_Attack2()); }
					else if (attack == 2) { StartCoroutine(_Attack3()); }
				}
			}
		}

		/// <summary>
		/// First attack in the AttackChain.
		/// </summary>
		private IEnumerator _Attack1()
		{
			StopAllCoroutines();
			SetAnimatorInt("Action", 1);
			SetAnimatorTrigger(AnimatorTrigger.AttackTrigger);
			Lock(true, true, true, true, 0, warriorTiming.TimingLock(warrior, "attack1"));
			ChainWindow((warriorTiming.TimingChain(warrior, "attack1start")), (warriorTiming.TimingChain(warrior, "attack1end")));
			attack = 1;
			yield return null;
		}

		/// <summary>
		/// Second attack in the AttackChain.
		/// </summary>
		private IEnumerator _Attack2()
		{
			StopAllCoroutines();
			canChain = false;
			SetAnimatorInt("Action", 2);
			SetAnimatorTrigger(AnimatorTrigger.AttackTrigger);
			Lock(true, true, true, true, 0, warriorTiming.TimingLock(warrior, "attack2"));
			ChainWindow((warriorTiming.TimingChain(warrior, "attack2start")), (warriorTiming.TimingChain(warrior, "attack2end")));
			attack = 2;
			yield return null;
		}

		/// <summary>
		/// Third and final attack in the AttackChain.
		/// </summary>
		private IEnumerator _Attack3()
		{
			StopAllCoroutines();
			SetAnimatorInt("Action", 3);
			SetAnimatorTrigger(AnimatorTrigger.AttackTrigger);
			Lock(true, true, true, true, 0, warriorTiming.TimingLock(warrior, "attack3"));
			if (warrior == Warrior.Hammer
				|| warrior == Warrior.Spearman
				|| warrior == Warrior.TwoHanded && ikHands != null) {
				ikHands.SetIKPause(warriorTiming.TimingLock(warrior, "attack3"));
			}

			// Reset attack chain.
			attack = 0;

			canChain = false;
			yield return null;
		}

		/// <summary>
		/// Attack while moving.  Uses Upperbody layer mask and animation layer.
		/// </summary>
		public void RunningAttack(int attackNumber)
		{
			if (warrior == Warrior.Archer
				|| warrior == Warrior.TwoHanded
				|| warrior == Warrior.Crossbow) {
				SetAnimatorInt("Action", 1);
				SetAnimatorTrigger(AnimatorTrigger.AttackTrigger);
			}
		}

		/// <summary>
		/// Attack while in the air.  Slams down to the ground.
		/// </summary>
		public void JumpAttack()
		{
			Debug.Log("JumpAttack");
			if (warrior == Warrior.Karate
				|| warrior == Warrior.Brute
				|| warrior == Warrior.Hammer
				|| warrior == Warrior.Spearman
				|| warrior == Warrior.Swordsman
				|| warrior == Warrior.TwoHanded
				|| warrior == Warrior.Crossbow
				|| warrior == Warrior.Mage) {
				jumpAttack = true;
				warriorMovementController.dropping = true;
			}
		}

		/// <summary>
		/// Special attack, or powerup buff.
		/// </summary>
		public void SpecialAttack(int attackNumber)
		{
			specialAttack = attackNumber;
			SetAnimatorInt("Action", attackNumber);
			SetAnimatorTrigger(AnimatorTrigger.AttackSpecialTrigger);
			Lock(true, true, true, true, 0, warriorTiming.TimingLock(warrior, ("special" + attackNumber.ToString())));
			if (warrior == Warrior.Crossbow || warrior == Warrior.Hammer && ikHands != null)
			{ ikHands.SetIKPause(warriorTiming.TimingLock(warrior, "special" + attackNumber.ToString())); }
		}

		/// <summary>
		/// Attack that moves the Warrior forward.
		/// </summary>
		public void MoveAttack(int attackNumber)
		{
			specialAttack = attackNumber;
			SetAnimatorInt("Action", attackNumber);
			SetAnimatorTrigger(AnimatorTrigger.AttackMoveTrigger);
			Lock(true, true, true, true, 0, warriorTiming.TimingLock(warrior, ("move" + attackNumber.ToString())));
			if (warrior == Warrior.Hammer && ikHands != null)
			{ ikHands.SetIKPause(warriorTiming.TimingLock(warrior, "move" + attackNumber.ToString())); }
		}

		/// <summary>
		/// Ranged attack.
		/// </summary>
		public void RangeAttack(int attackNumber)
		{
			specialAttack = attackNumber;
			SetAnimatorInt("Action", attackNumber);
			SetAnimatorTrigger(AnimatorTrigger.AttackRanged);
			Lock(true, true, true, true, 0, warriorTiming.TimingLock(warrior, ("range" + attackNumber.ToString())));
			if (warrior == Warrior.TwoHanded) { StartCoroutine(_TwoHandedRangeAttack()); }
		}

		/// <summary>
		/// Throw sword attack for 2Handed Warrior.
		/// </summary>
		private IEnumerator _TwoHandedRangeAttack()
		{
			yield return new WaitForSeconds(0.7f);
			animator.transform.GetChild(3).gameObject.SetActive(true);
			HideShowWeapons(false);
			yield return new WaitForSeconds(1.325f);
			animator.transform.GetChild(3).gameObject.SetActive(false);
			HideShowWeapons(true);
		}

		/// <summary>
		/// Puts the Warrior in blocking state.
		/// </summary>
		public void Blocking()
		{
			if (canBlock) {
				if (!isBlocking) {
					if (HasBlockInput()) {
						isBlocking = true;
						_canMove = false;
						SetAnimatorBool("Blocking", true);
						SetAnimatorTrigger(AnimatorTrigger.BlockTrigger);
						if (warrior == Warrior.Crossbow && ikHands != null) { ikHands.BlendIK(false, 0, 0.1f); }
					}
				}
				else {
					if (!HasBlockInput()) {
						isBlocking = false;
						_canMove = true;
						SetAnimatorBool("Blocking", false);
						if (warrior == Warrior.Crossbow && ikHands != null) { ikHands.BlendIK(true, 0, 0.1f); }
					}
				}
			}
		}

		/// <summary>
		/// Knocks the Warrior back while in Blocking state.
		/// </summary>
		public void BlockBreak()
		{
			SetAnimatorTrigger(AnimatorTrigger.BlockBreakTrigger);
			Lock(true, true, true, true, 0, 1f);
			if (warrior == Warrior.TwoHanded && ikHands != null) { ikHands.SetIKPause(0.9f); }
		}

		/// <summary>
		/// Character will strafe around target.
		/// </summary>
		private void Targeting()
		{
			isTargeting = HasTargetInput();
			SetAnimatorBool("Targeting", HasTargetInput());
		}

		/// <summary>
		/// Character takes a light hit.
		/// </summary>
		public void GetHit()
		{
			SetAnimatorInt("Action", 1);
			SetAnimatorTrigger(AnimatorTrigger.LightHitTrigger);
			Lock(true, true, true, true, 0, warriorTiming.TimingLock(warrior, ("lighthit1".ToString())));
		}

		/// <summary>
		/// Kills the Warrior, or if already dead, revives. If blocking, BlockBreak.
		/// </summary>
		private void DeathRevive()
		{
			// Death/Revive.
			if (inputDeath) {
				if (!isDead) {
					if (isBlocking) { BlockBreak(); }
					else { Death(); }
				}
				else { Revive(); }
			}
		}

		/// <summary>
		/// Kills the Warrior.
		/// </summary>
		public void Death()
		{
			SetAnimatorTrigger(AnimatorTrigger.DeathTrigger);
			Lock(true, true, true, false, 0.1f, 0f);
			if (warrior == Warrior.Crossbow || warrior == Warrior.TwoHanded && ikHands != null) { ikHands.SetIKOff(); }
			isDead = true;
		}

		/// <summary>
		/// Revives the Warrior from Death.
		/// </summary>
		public void Revive()
		{
			SetAnimatorTrigger(AnimatorTrigger.ReviveTrigger);
			Lock(true, true, true, true, 0, warriorTiming.TimingLock(warrior, "revive"));
			isDead = false;
			if (warrior == Warrior.Crossbow || warrior == Warrior.TwoHanded && ikHands != null) { ikHands.BlendIK(true, 1f, 0.25f); }
		}

		/// <summary>
		/// Character Dash.
		/// </summary>
		/// <param name="1">Forward.</param>
		/// <param name="2">Right.</param>
		/// <param name="3">Backward.</param>
		/// <param name="4">Left.</param>
		public void Dash(int dash)
		{
			// Knight has 2 sets of Dashes.
			if (dash < 0) { Lock(true, true, true, true, 0, warriorTiming.TimingLock(warrior, "dash2")); }
			else { Lock(true, true, true, true, 0, warriorTiming.TimingLock(warrior, "dash")); }

			SetAnimatorInt("Action", dash);
			SetAnimatorTrigger(AnimatorTrigger.DashTrigger);
		}

		#endregion

		#region Locks

		/// <summary>
		/// Lock character movement and/or action, on a delay for a set time.
		/// </summary>
		/// <param name="lockMovement">If set to <c>true</c> lock movement.</param>
		/// <param name="lockAction">If set to <c>true</c> lock action.</param>
		/// <param name="timed">If set to <c>true</c> timed.</param>
		/// <param name="delayTime">Delay time.</param>
		/// <param name="lockTime">Lock time.</param>
		public void Lock(bool lockMovement, bool lockAction, bool lockJump, bool timed, float delayTime, float lockTime)
		{
			if (co != null) { StopCoroutine(co); }
			co = StartCoroutine(_Lock(lockMovement, lockAction, lockJump, timed, delayTime, lockTime));
		}

		//Timed -1 = infinite, 0 = no, 1 = yes.
		public IEnumerator _Lock(bool lockMovement, bool lockAction, bool lockJump, bool timed, float delayTime, float lockTime)
		{
			if (delayTime > 0) { yield return new WaitForSeconds(delayTime); }
			if (lockMovement) { LockMove(true); }
			if (lockAction) { LockAction(true); }
			if (lockJump) { LockJump(true); }
			if (timed) {
				if (lockTime > 0) {
					yield return new WaitForSeconds(lockTime);
					UnLock(lockMovement, lockAction, lockJump);
				}
			}
		}

		/// <summary>
		/// Keep character from moving and use or diable Rootmotion.
		/// </summary>
		public void LockMove(bool b)
		{
			_canMove = !b;
			SetAnimatorRootMotion(b);
			if (b) {
				SetAnimatorBool("Moving", false);
				moveInput = Vector3.zero;
			}
		}

		/// <summary>
		/// Keep character from doing actions.
		/// </summary>
		public void LockAction(bool b)
		{ _canAction = !b; }

		/// <summary>
		/// Keep character from blocking.
		/// </summary>
		public void LockBlock(bool b)
		{ _canBlock = !b; }

		/// <summary>
		/// Keep character from jumping.
		/// </summary>
		public void LockJump(bool b)
		{ _canJump = !b; }

		/// <summary>
		/// Keep character from double jumping.
		/// </summary>
		public void LockDoubleJump(bool b)
		{ _canDoubleJump = !b; }

		/// <summary>
		/// Let character move and act again.
		/// </summary>
		private void UnLock(bool movement, bool actions, bool jump)
		{
			if (movement) { LockMove(false); }
			if (actions) { LockAction(false); }
			if (jump) { LockJump(false); }
		}

		#endregion

		#region Misc

		/// <summary>
		/// Returns whether the character is near the ground.
		/// </summary>
		public bool AcquiringGround()
		{ return superCharacterController.currentGround.IsGrounded(false, 0.01f); }

		/// <summary>
		/// Returns whether the character is on the ground.
		/// </summary>
		public bool MaintainingGround()
		{ return superCharacterController.currentGround.IsGrounded(true, 0.5f); }

		/// <summary>
		/// Locks the movement of the Warrior and turns off its collision.
		/// </summary>
		public void SwitchCollisionOff()
		{
			_canMove = false;
			superCharacterController.enabled = false;
			SetAnimatorRootMotion(true);
			if (rb != null) { rb.isKinematic = false; }
		}

		/// <summary>
		/// Unlocks the movement of the Warrior and turns on its collision.
		/// </summary>
		public void SwitchCollisionOn()
		{
			_canMove = true;
			superCharacterController.enabled = true;
			SetAnimatorRootMotion(false);
			if (rb != null) { rb.isKinematic = true; }
		}

		/// <summary>
		/// Set Animator Trigger.
		/// </summary>
		public void SetAnimatorTrigger(AnimatorTrigger trigger)
		{
			Debug.Log("SetAnimatorTrigger: " + trigger + " - " + ( int )trigger);
			animator.SetInteger("TriggerNumber", ( int )trigger);
			animator.SetTrigger("Trigger");
		}

		/// <summary>
		/// Set Animator Bool.
		/// </summary>
		public void SetAnimatorBool(string name, bool b)
		{ animator.SetBool(name, b); }

		/// <summary>
		/// Set Animator float.
		/// </summary>
		public void SetAnimatorFloat(string name, float f)
		{ animator.SetFloat(name, f); }

		/// <summary>
		/// Set Animator ingeter.
		/// </summary>
		public void SetAnimatorInt(string name, int i)
		{ animator.SetInteger(name, i); }

		/// <summary>
		/// Set Animator to use root motion or not.
		/// </summary>
		public void SetAnimatorRootMotion(bool b)
		{ useRootMotion = b; }

		/// <summary>
		/// Allows Attack chaining to occur within a set timeframe.
		/// <param name="timeToWindow">How long to wait to start the window.</param>
		/// <param name="chainLength">How long the window stays open.</param>
		/// </summary>
		public void ChainWindow(float timeToWindow, float chainLength)
		{
			StopCoroutine("_ChainWindow");
			StartCoroutine(_ChainWindow(timeToWindow, chainLength));
		}

		public IEnumerator _ChainWindow(float timeToWindow, float chainLength)
		{
			yield return new WaitForSeconds(timeToWindow);
			canChain = true;
			yield return new WaitForSeconds(chainLength);
			canChain = false;
			attack = 0;
		}

		/// <summary>
		/// Hide and Unhide the Warrior's weapon.
		/// </summary>
		public void SheathWeapons()
		{
			bool hideshow;
			if (sheathed) {
				sheathed = false;
				SetAnimatorBool("Weapons", true);
				SetAnimatorInt("Action", 2);
				SetAnimatorTrigger(AnimatorTrigger.WeaponSwitchTrigger);
				hideshow = true;
				if (warrior == Warrior.TwoHanded
					|| warrior == Warrior.Hammer
					|| warrior == Warrior.Crossbow
					|| warrior == Warrior.Spearman && ikHands != null) {
					ikHands.BlendIK(true, 0.75f, 0.5f);
				}
			}
			else {
				sheathed = true;
				SetAnimatorBool("Weapons", false);
				SetAnimatorInt("Action", 1);
				SetAnimatorTrigger(AnimatorTrigger.WeaponSwitchTrigger);
				hideshow = false;
				if (warrior == Warrior.TwoHanded
					|| warrior == Warrior.Hammer
					|| warrior == Warrior.Crossbow
					|| warrior == Warrior.Spearman & ikHands != null) {
					ikHands.BlendIK(false, 0f, 0.25f);
				}
			}
			StartCoroutine(_HideShowWeapons(hideshow));
			Lock(true, true, true, true, 0, warriorTiming.TimingLock(warrior, "sheath"));
		}

		/// <summary>
		/// Waits for animation's Animation Event to trigger WArriorAnimatorEvent.cs's WeaponSwitch method.
		/// </summary>
		public IEnumerator _HideShowWeapons(bool hideshow)
		{
			while (waitingOnWeapons) { yield return null; }
			HideShowWeapons(hideshow);
		}

		/// <summary>
		/// Hide and Unhide the Warrior's weapon.
		/// <param name="chainLength">How long the window stays open.</param>
		/// </summary>
		private void HideShowWeapons(bool showhide)
		{
			if (weapon != null) { weapon.gameObject.SetActive(showhide); }
			waitingOnWeapons = true;
		}

		/// <summary>
		/// Prints out all the WarriorController variables.
		/// </summary>
		public void ControllerDebug()
		{
			Debug.Log("CONTROLLER SETTINGS---------------------------");
			Debug.Log($"useInputSystem:{useInputSystem}   allowedInput:{allowedInput}   isMoving:{isMoving}     " +
				$"isDead:{isDead}    isBlocking:{isBlocking}    isTargeting:{isTargeting}     " +
				$"jumpAttack:{jumpAttack}    sheathed:{sheathed}    waitingOnWeapons:{waitingOnWeapons}     " +
				$"useRootMotion:{useRootMotion}    canChain:{canChain}    attack:{attack}     " +
				$"specialAttack:{specialAttack}    canAction:{canAction}    canDoubleJump:{canDoubleJump}     " +
				$"canMove:{canMove}    canJump:{canJump}    attack:{attack}     " +
				$"animationSpeed:{animationSpeed}");
		}

		/// <summary>
		/// Prints out all the Animator parameters.
		/// </summary>
		public void AnimatorDebug()
		{
			Debug.Log("ANIMATOR SETTINGS---------------------------");
			Debug.Log($"Moving:{animator.GetBool("Moving")}   Targeting:{animator.GetBool("Targeting")}   Stunned:{animator.GetBool("Stunned")}     " +
				$"Blocking:{animator.GetBool("Blocking")}    Weapons:{animator.GetBool("Weapons")}    Jumping:{animator.GetInteger("Jumping")}     " +
				$"Action:{animator.GetInteger("Action")}    TriggerNumber:{animator.GetInteger("TriggerNumber")}    Velocity X:{animator.GetFloat("Velocity X")}     " +
				$"Velocity Z:{animator.GetFloat("Velocity Z")}");
		}

		#endregion
	}
}