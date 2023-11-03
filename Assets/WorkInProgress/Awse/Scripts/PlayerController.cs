using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

    InputManager inputManager;

    [Header("Movement")]
    //public float playerSpeed;
    [SerializeField] public float friction = 6;
    [SerializeField] public float gravity = 20;
    [SerializeField] public float jump_force = 8;
    [Tooltip("Auto jump when holding down the jump button")]
    [SerializeField] public bool auto_jump = false;
    [Tooltip("Precision of air control")]
    [SerializeField] public float air_control;
    [SerializeField] public MovementSettings ground_settings = new MovementSettings(7, 14, 10);
    [SerializeField] public MovementSettings air_settings = new MovementSettings(7, 2, 2);
    [SerializeField] public MovementSettings strafe_settings = new MovementSettings(1, 50, 50);

    public float Speed { get { return character.velocity.magnitude; } }
    public CharacterController character;
    public Vector3 move_direction_norm = Vector3.zero;
    public Vector3 player_velocity = Vector3.zero;

    //queues next jump
    public bool jump_queued = false;
    //real time fricton value
    public float player_friction;

    public Vector3 move_input;
    public Transform _transform;
    public Transform _cam_transform;


    /*[Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public bool isReadyToJump;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundLayer;
    public bool isGrounded;

    [Header("Other")]
    public Transform orientation;
    /*public Inventory_UI inventory_display;
    //Fix this for me later, am lazy
    public float timer = 1.0f;
    float ui_cooldown = 0.0f;*/

    /*float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody playerRigidbody;*/

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
    }

    private void Start()
    {
        _transform = transform;
        character = GetComponent<CharacterController>();
        //playerRigidbody = GetComponent<Rigidbody>();
        //playerRigidbody.freezeRotation = true;
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        //Movement has to go in here
        move_input = new Vector3(inputManager.movementInput.x, 0, inputManager.movementInput.y);
        QueueJump();

        if(character.isGrounded)
        {
            GroundMovement();
        }
        else
        {
            AirMovement();
        }

        character.Move(player_velocity * Time.deltaTime);
        //HandleMovement();
        //HandleGroundCheck();
        
        //HandleJump();
    }

    void QueueJump()
    {
        if(auto_jump)
        {
            jump_queued = inputManager.jumpInput;
            return;
        }

        if(inputManager.jumpInput && !jump_queued)
        {
            jump_queued = true;
        }

        if(!inputManager.jumpInput)
        {
            jump_queued = false;
        }
    }

    void AirMovement()
    {
        float _acceleration;

        var w_direction = new Vector3(move_input.x, 0, move_input.z);
        w_direction = transform.TransformDirection(w_direction);

        float w_speed = w_direction.magnitude;
        w_speed *= air_settings.max_speed;

        w_direction.Normalize();
        move_direction_norm = w_direction;

        float w_speed2 = w_speed;
        if(Vector3.Dot(player_velocity, w_direction) < 0)
        {
            _acceleration = air_settings.deceleration;
        }
        else
        {
            _acceleration = air_settings.acceleration;
        }

        //if the player is strafing left or right and not moving vertically
        if(move_input.z == 0 && move_input.x != 0)
        {
            if(w_speed > strafe_settings.max_speed)
            {
                w_speed = strafe_settings.max_speed;
            }

            _acceleration = strafe_settings.acceleration;
        }

        Accelerate(w_direction, w_speed, _acceleration);
        if(air_control > 0)
        {
            AirControl(w_direction, w_speed2);
        }

        //apply grav to player
        player_velocity.y -= gravity * Time.deltaTime;
    }

    //air control when player is in the air, letting the player move horizontally faster than if grounded
    void AirControl(Vector3 target_direction, float target_speed)
    {
        if(Mathf.Abs(move_input.z) < 0.001 || Mathf.Abs(target_speed) < 0.001)
        {
            return;
        }

        float z_speed = player_velocity.y;
        player_velocity.y = 0;

        //apparently below is equal to VectorNormalize() used by idtech, prayge
        float speed = player_velocity.magnitude;
        player_velocity.Normalize();
        float dot = Vector3.Dot(player_velocity, target_direction);
        float i = 32;
        i *= air_control * dot * dot * Time.deltaTime;

        //change dir while decreasing speed
        if(dot > 0)
        {
            player_velocity.x *= speed + target_direction.x * i;
            player_velocity.y *= speed + target_direction.y * i;
            player_velocity.z *= speed + target_direction.z * i;

            player_velocity.Normalize();
            move_direction_norm = player_velocity;
        }

        player_velocity.x *= speed;
        player_velocity.y = z_speed;
        player_velocity.z *= speed;
    }

    void GroundMovement()
    {
        //don't apply friction if jump is queued
        if(!jump_queued)
        {
            ApplyFriction(1.0f);
        }
        else
        {
            ApplyFriction(0f);
        }

        var w_direction = new Vector3(move_input.x, 0, move_input.z);
        w_direction = transform.TransformDirection(w_direction);
        w_direction.Normalize();
        move_direction_norm = w_direction;

        var w_speed = w_direction.magnitude;
        w_speed *= ground_settings.max_speed;

        Accelerate(w_direction, w_speed, ground_settings.acceleration);

        //reset grav velocity
        player_velocity.y = -gravity * Time.deltaTime;

        if(jump_queued)
        {
            player_velocity.y = jump_force;
            jump_queued = false;
        }
    }

    private void ApplyFriction(float f)
    {
        Vector3 vec = player_velocity;
        vec.y = 0;
        float speed = vec.magnitude;
        float drop = 0;

        //apply friction when grounded
        if(character.isGrounded)
        {
            float control = speed < ground_settings.deceleration ? ground_settings.deceleration : speed;
            drop = control * friction * Time.deltaTime * f;
        }

        float new_speed = speed - drop;
        player_friction = new_speed;
        if(new_speed < 0)
        {
            new_speed = 0;
        }
        if(speed > 0)
        {
            new_speed /= speed;
        }

        //y value not needed
        player_velocity.x *= new_speed;
        player_velocity.z *= new_speed;
    }

    private void Accelerate(Vector3 _target_direction, float _target_speed, float _acceleration)
    {
        float current_speed = Vector3.Dot(player_velocity, _target_direction);
        float add_speed = _target_speed - current_speed;
        if(add_speed <= 0)
        {
            return;
        }

        float accel_speed = _acceleration * Time.deltaTime * _target_speed;
        if(accel_speed > add_speed)
        {
            accel_speed = add_speed;
        }

        //y value not needed
        player_velocity.x += accel_speed * _target_direction.x;
        player_velocity.z += accel_speed * _target_direction.y;
    }

    /*private void HandleMovement()
    {
        horizontalInput = inputManager.movementInput.x;
        verticalInput = inputManager.movementInput.y;
       /* if (ui_cooldown > 0)
        {
            ui_cooldown -= Time.deltaTime;
        }
        if(inventory_display.in_view)
        {

            if (ui_cooldown <= 0)
            {
                ui_cooldown = timer;
                if (horizontalInput > 0)
                {
                    inventory_display.moveSelector(Inventory_UI.Directions.RIGHT);
                }
                if (horizontalInput < 0)
                {
                    inventory_display.moveSelector(Inventory_UI.Directions.LEFT);
                }
                if (verticalInput < 0)
                {
                    inventory_display.moveSelector(Inventory_UI.Directions.DOWN);
                }
                if (verticalInput > 0)
                {
                    inventory_display.moveSelector(Inventory_UI.Directions.UP);
                }
            }
        }
        else
        {*/
           //moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

           //gameObject.transform.Translate(moveDirection * Time.deltaTime * playerSpeed);

            //trying out Force movement - momentum seemed fun but maybe not :(
            //playerRigidbody.AddForce(moveDirection.normalized * playerSpeed, ForceMode.Force);
        //}
    //}

    /*private void HandleGroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.5f, groundLayer);
    }

    private void HandleJump()
    {
        if (inputManager.jumpInput && isReadyToJump && isGrounded)
        {
            playerRigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            isReadyToJump = false;

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        isReadyToJump = true;
    }*/
}
