using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    InputManager inputManager;

    [Header("Movement")]
    public float playerSpeed;

    [Header("Jump")]
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

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody playerRigidbody;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
    }

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.freezeRotation = true;
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        //Movement has to go in here
        HandleMovement();
        HandleGroundCheck();
        
        HandleJump();
    }

    private void HandleMovement()
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
           moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

           gameObject.transform.Translate(moveDirection * Time.deltaTime * playerSpeed);

            //trying out Force movement - momentum seemed fun but maybe not :(
            //playerRigidbody.AddForce(moveDirection.normalized * playerSpeed, ForceMode.Force);
        //}
    }

    private void HandleGroundCheck()
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
    }
}
