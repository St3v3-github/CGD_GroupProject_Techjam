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
        //HandleJump();
    }

    private void FixedUpdate()
    {
        //Movement has to go in here
        HandleMovement();
        HandleGroundCheck();
    }

    private void HandleMovement()
    {
        horizontalInput = inputManager.movementInput.x;
        verticalInput = inputManager.movementInput.y;

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        gameObject.transform.Translate(moveDirection * Time.deltaTime * playerSpeed);

        //trying out Force movement - momentum seemed fun but maybe not :(
        //playerRigidbody.AddForce(moveDirection.normalized * playerSpeed, ForceMode.Force);
    }

    private void HandleGroundCheck()
    {
        /*        Ray ray = new Ray(transform.position, Vector3.down * (playerHeight * 0.5f + 0.5f));
                Debug.DrawRay(transform.position, Vector3.down);
                if (Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.5f, groundLayer))
                {
                    isGrounded = true;
                }

                else
                {
                    isGrounded = false;
                }*/

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 1f, groundLayer);

    }
}

   /* private void HandleJump()
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
        if (isGrounded)
        {
            isReadyToJump = true;
        }
    }
}
*/