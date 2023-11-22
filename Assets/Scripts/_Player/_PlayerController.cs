using Unity.VisualScripting;
using UnityEngine;

public class _PlayerController : MonoBehaviour
{
    public _InputManager inputManager;

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
        HandleGroundCheck();
    }

    public void HandleMovement(Vector2 movementInput)
    {
        horizontalInput = movementInput.x;
        verticalInput = movementInput.y;

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        gameObject.transform.Translate(moveDirection * Time.deltaTime * playerSpeed);

        //trying out Force movement - momentum seemed fun but maybe not :(
        //playerRigidbody.AddForce(moveDirection.normalized * playerSpeed, ForceMode.Force);
    }

    public void HandleJump()
    {
        if (isGrounded && isReadyToJump)
        {
            playerRigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            isReadyToJump = false;

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void HandleGroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.5f, groundLayer);
    }

    private void ResetJump()
    {
        isReadyToJump = true;
    }
}

