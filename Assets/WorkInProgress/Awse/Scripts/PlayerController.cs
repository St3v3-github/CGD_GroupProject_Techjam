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

    InputManager input_manager;

    [Header("Aiming")]
    [SerializeField] public Camera _camera;
    [SerializeField] public CameraController camera_controller;

    [Header("Movement")]
    [SerializeField] public float friction = 6;
    [SerializeField] public float gravity = 20;
    [SerializeField] public float jump_force = 8;
    [Tooltip("Automatically jump when holding jump button")]
    [SerializeField] public bool auto_jump = false;
    [Tooltip("How precise the player's air control is")]
    [SerializeField] public float air_control = 0.3f;
    [SerializeField] public MovementSettings ground_settings = new MovementSettings(7, 14, 10);
    [SerializeField] public MovementSettings air_settings = new MovementSettings(7, 2, 2);
    [SerializeField] public MovementSettings strafe_settings = new MovementSettings(1, 50, 50);

    /// <summary>
    /// Returns player's current speed.
    /// </summary>
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

    private void Awake()
    {
        input_manager = FindObjectOfType<InputManager>();
    }

    public void Start()
    {
        player_transform = transform;
        character = GetComponent<CharacterController>();

        //camera_controller = _camera.GetComponent<CameraController>();
        //camera_controller.Init(player_transform);
    }

    public void Update()
    {
        move_input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        QueueJump();
        // Sets the player movement state.
        if (character.isGrounded)
        {
            GroundMovement();
        }
        else
        {
            AirMovement();
        }
    }

    public void FixedUpdate()
    {
        //Mainly physics based movement (i.e using rigidbody) should go here, but just in case

        // Move the character.
        character.Move(player_velocity * Time.deltaTime);
    }

    // Queues the next jump.
    public void QueueJump()
    {
        if (auto_jump)
        {
            jump_queued = input_manager.jumpInput;
            return;
        }

        if (input_manager.jumpInput && !jump_queued)
        {
            jump_queued = true;
        }

        if (!input_manager.jumpInput)
        {
            jump_queued = false;
        }
    }

    // Handles the player's air movement.
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

    //Air control occurs when player is airborne, allowed the player to move horizontally more freely than when grounded.
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

    // Handles player ground movement.
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

        // Reset the gravity velocity
        player_velocity.y = -gravity * Time.deltaTime;

        if (jump_queued)
        {
            player_velocity.y = jump_force;
            jump_queued = false;
        }
    }

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

    // Calculates acceleration based on desired speed and direction.
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
}
