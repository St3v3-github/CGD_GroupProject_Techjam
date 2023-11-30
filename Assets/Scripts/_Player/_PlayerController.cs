using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    public class MovementSettings
    {
        public float max_speed;
        public float acceleration;
        public float deceleration;

        public MovementSettings(float _max_speed, float _acceleration, float _deceleration)
        {
            max_speed = _max_speed;
            acceleration = _acceleration;
            deceleration = _deceleration;
        }
    }

    [Header("Aiming")]
    [SerializeField] public Camera _camera;
    [SerializeField] public CameraController camera_controller;

    [Header("Movement")]
    [SerializeField] public float friction = 6;
    [SerializeField] public float gravity = 20;
    [SerializeField] public float jump_force = 8;

    [Tooltip("Automatically jump when holding jump button")]
    [SerializeField] public bool auto_jump = false;

    [Tooltip("How precise the player's air control is, ranges from 0 to 1. Should not be greater than 0.4 if you want player to have control in air")]
    [SerializeField] public float air_control = 0.3f;

    [SerializeField] public MovementSettings ground_settings = new MovementSettings(7, 14, 10);
    [SerializeField] public MovementSettings air_settings = new MovementSettings(7, 2, 2);
    [SerializeField] public MovementSettings strafe_settings = new MovementSettings(3, 50, 50);

    // Fixed Jump Variables
    [Header("Jump")]
    public float jumpCooldown;
    public bool isReadyToJump;

    //Returns player's current speed.
    public float speed { get { return character.velocity.magnitude; } }

    public CharacterController character;
    public Vector3 move_dir_norm = Vector3.zero;
    public Vector3 player_velocity = Vector3.zero;

    // Used to queue the next jump just before hitting the ground.
    public bool jump_queued = false;

    // Used to display real time friction values.
    public float player_friction = 0;

    public Vector3 move_input;
    public Transform player_transform;
    public Transform camera_transform;

    [Header("Melee")]
    public float meleeDistance = 3f;
    public int meleeDamage = 1;
    public float meleeSpeed = 1f;
    public float meleeDelay = 1f;
    public LayerMask playerLayer;
    bool inMelee = false;
    bool readyToMelee = true;

    public AnimationManager playerAnimControl;
    


    [Header("Dash")]
    //Dashing related values
    [Tooltip("How long the player's dash is")]
    public float max_dash_time;
    [Tooltip("The force of the player's dash")]
    public float dash_force;
    [SerializeField] private float dash_timer;
    [SerializeField] private bool dashing = false;
    [SerializeField] private bool dash_ready = true;
    [SerializeField] private float dash_cooldown = 2f;

    //Quake's coordinate system, unlike a standard right handed one, has the Z-axis be up/down and the Y-axis be depth. All calculations
    //are converted to match Unity's coordinate system.


    public void Start()
    {
        player_transform = transform;
        character = GetComponent<CharacterController>();

        camera_controller = _camera.GetComponent<CameraController>();
        camera_transform = _camera.transform;
        camera_controller.Init(player_transform, camera_transform);
    }

    public void HandleCamera(Vector2 cameraInput)
    {
        // Move/rotate camera
        camera_controller.LookRotation(cameraInput, player_transform, camera_transform);
        camera_controller.CursorLock();
    }

    public void HandleMovement(Vector2 movementInput)
    {
        // Sets the player movement state.
        move_input = new Vector3(movementInput.x, 0, movementInput.y);
        
        //Checks if the player is moving to trigger a walk animation
        if(move_input.x != 0.0f || move_input.z != 0.0f)
        {
            playerAnimControl.toggleWalkingBool(true);
        }
        else
        {
            playerAnimControl.toggleWalkingBool(false);
        }

        if (character.isGrounded)
        {
            playerAnimControl.toggleGroundedBool(true); //Toggles boolean in the animator to trigger landing animation if airborne
            GroundMovement();
        }
        else
        {
            playerAnimControl.toggleGroundedBool(false); //toggles boolean in animator to trigger falling animation
            AirMovement();
        }

        // Move the character.
        character.Move(player_velocity * Time.deltaTime);
    }

    /// <summary>
    /// Handles the player's air movement.
    /// </summary>
    public void AirMovement()
    {
        float acceleleration;

        var w_direction = new Vector3(move_input.x, 0, move_input.z);
        w_direction = player_transform.TransformDirection(w_direction);

        float w_speed = w_direction.magnitude;
        w_speed *= air_settings.max_speed;

        w_direction.Normalize();
        move_dir_norm = w_direction;

        float w_speed2 = w_speed;
        if (Vector3.Dot(player_velocity, w_direction) < 0)
        {
            acceleleration = air_settings.deceleration;
        }
        else
        {
            acceleleration = air_settings.acceleration;
        }

        // If the player is strafing left or right, no vertical movement!
        if (move_input.z == 0 && move_input.x != 0)
        {
            if (w_speed > strafe_settings.max_speed)
            {
                w_speed = strafe_settings.max_speed;
            }

            acceleleration = strafe_settings.acceleration;
        }

        Accelerate(w_direction, w_speed, acceleleration);
        if (air_control > 0)
        {
            AirControl(w_direction, w_speed2);
        }

        // Applies gravity to the player
        player_velocity.y -= gravity * Time.deltaTime;

    }

    /// <summary>
    /// Air control occurs when player is airborne, affecting the player's ability to move horizontally when airborne.
    /// </summary>
    /// <param name="target_direction"></param>
    /// <param name="target_speed"></param>
    public void AirControl(Vector3 target_direction, float target_speed)
    {
        // Only control air movement when moving forward or backward.
        if (Mathf.Abs(move_input.z) < 0.001 || Mathf.Abs(target_speed) < 0.001)
        {
            return;
        }

        float zSpeed = player_velocity.y;
        player_velocity.y = 0;
        /* Next two lines are equivalent to idTech's VectorNormalize() */
        float speed = player_velocity.magnitude;
        player_velocity.Normalize();

        float dot = Vector3.Dot(player_velocity, target_direction);
        float i = 32;
        i *= air_control * dot * dot * Time.deltaTime;

        // Change direction while slowing down.
        if (dot > 0)
        {
            player_velocity.x *= speed + target_direction.x * i;
            player_velocity.y *= speed + target_direction.y * i;
            player_velocity.z *= speed + target_direction.z * i;

            player_velocity.Normalize();
            move_dir_norm = player_velocity;
        }

        player_velocity.x *= speed;
        player_velocity.y = zSpeed;
        player_velocity.z *= speed;
    }

    /// <summary>
    /// Handles player ground movement.
    /// </summary>
    public void GroundMovement()
    {
        // Do not apply friction if the player is queueing up the next jump
        if (!jump_queued)
        {
            Applyfriction(1.0f);
        }
        else
        {
            Applyfriction(0f);
        }

        var w_direction = new Vector3(move_input.x, 0, move_input.z);
        w_direction = player_transform.TransformDirection(w_direction);
        w_direction.Normalize();
        move_dir_norm = w_direction;

        var w_speed = w_direction.magnitude;
        w_speed *= ground_settings.max_speed;

        Accelerate(w_direction, w_speed, ground_settings.acceleration);
    }
    /// <summary>
    /// Apply friction when player is grounded and jump is not queued (when/if jump queueing is reimplimented)
    /// </summary>
    /// <param name="t"></param>
    public void Applyfriction(float t)
    {
        Vector3 vec = player_velocity;
        vec.y = 0;
        float speed = vec.magnitude;
        float drop = 0;

        // Only apply friction when grounded!
        if (character.isGrounded)
        {
            float control = speed < ground_settings.deceleration ? ground_settings.deceleration : speed;
            drop = control * friction * Time.deltaTime * t;
        }

        float new_speed = speed - drop;
        player_friction = new_speed;
        if (new_speed < 0)
        {
            new_speed = 0;
        }

        if (speed > 0)
        {
            new_speed /= speed;
        }

        //y not needed!
        player_velocity.x *= new_speed;
        player_velocity.z *= new_speed;
    }

    /// <summary>
    /// Calculates acceleration based on desired speed and direction.
    /// </summary>
    /// <param name="_target_direction"></param>
    /// <param name="_target_speed"></param>
    /// <param name="_acceleration"></param>
    public void Accelerate(Vector3 _target_direction, float _target_speed, float _acceleration)
    {
        float currentspeed = Vector3.Dot(player_velocity, _target_direction);
        float addspeed = _target_speed - currentspeed;
        if (addspeed <= 0)
        {
            return;
        }

        float accelspeed = _acceleration * Time.deltaTime * _target_speed;
        if (accelspeed > addspeed)
        {
            accelspeed = addspeed;
        }

        player_velocity.x += accelspeed * _target_direction.x;
        player_velocity.z += accelspeed * _target_direction.z;
    }

    public void HandleJump()
    {
        if (character.isGrounded)
        {
            player_velocity.y = jump_force;
            isReadyToJump = false;

            playerAnimControl.toggleJumpingBool(true); //toggles bool in animator to trigger jumping animation

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        isReadyToJump = true;
        playerAnimControl.toggleJumpingBool(false); //toggles bool in animator to stop jumping animation looping unintentionally
    }

    public void HandleMelee()
    {

        if (!readyToMelee || inMelee) return;

        readyToMelee = false;
        inMelee = true;
        playerAnimControl.toggleAttackingBool(true); //toggles bool in animator to trigger melee animation

        Invoke(nameof(ResetMelee), meleeSpeed);
        Invoke(nameof(MeleeRaycast), meleeDelay);
    }

    private void MeleeRaycast()
    {

        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, meleeDistance, playerLayer))
        {
            Debug.Log("HIT ENEMY :O -1 HEALTH");
            if (hit.transform.TryGetComponent<TestDummy>(out TestDummy T))
            {
                T.TakeDamage(meleeDamage);
            }

        }
    }

    private void ResetMelee()
    {
        inMelee = false;
        readyToMelee = true;
        playerAnimControl.toggleAttackingBool(false); //toggles bool in animator to prevent melee anim looping
    }

    public IEnumerator PlayerDash(Vector2 movement_input)
    {
        if (dash_ready)
        {
            dashing = true;
            dash_timer = max_dash_time;

            while (dashing)
            {
                Debug.Log("Dashing");
                //gets input direction to dash any direction regardless of camera orientation
                Vector3 input_direction = transform.forward * movement_input.y + transform.right * movement_input.x;
                character.Move((input_direction.normalized * dash_force) * Time.deltaTime);

                dash_timer -= Time.deltaTime;

                if (dash_timer <= 0)
                {
                    dashing = false;
                    dash_ready = false;
                    Invoke(nameof(ResetDash), dash_cooldown);
                }
                yield return null;
            }
        }
    }

    private void ResetDash()
    {
        dash_ready = true;
    }

}