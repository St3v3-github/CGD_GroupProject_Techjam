using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputManager;

public class UpdatedPlayerController : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Camera playerCam;

    float xRotation;
    float yRotation;

    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public Transform orientation; //this might not be needed

    Vector3 movementDirection;
    Rigidbody rb;

    public float playerHeight;
    public LayerMask groundMask;
    public bool isGrounded;
    public float groundDrag;

    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;
    public bool crouchPressed = false;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;
    public bool sprintPressed = false;

    public MovementState state;

    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private void StateHandler()
    {
        //Crouch just compresses the character right now, need to hook up the crouch to the animation manager in the future
        //and just make the hitbox of the player smaller.
        if(crouchPressed)
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        if (isGrounded && sprintPressed)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if (isGrounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
        startYScale = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        SpeedControl();

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);

        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        StateHandler();

        if(crouchPressed)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    

    public void HandleMovement(Vector2 movementInput)
    {
        

        movementDirection = transform.forward * movementInput.y + transform.right * movementInput.x;

        rb.AddForce(movementDirection.normalized * moveSpeed * 10f, ForceMode.Force);


        /*

        

        if (isGrounded)
        {
            rb.AddForce(movementDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!isGrounded)
        {
            rb.AddForce(movementDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
        */
    }

    public void HandleCamera(Vector2 cameraInput)
    {
        float camX = cameraInput.x * Time.deltaTime * sensX;
        float camY = cameraInput.y * Time.deltaTime * sensY;

        yRotation += camX;

        xRotation -= camY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        
    }

    public void HandleJump()
    {
        //Debug.Log("test");
        readyToJump = false;

        Jump();

        Invoke(nameof(ResetJump), jumpCooldown);
    }

    

    private void SpeedControl()
        {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if(flatVelocity.magnitude > moveSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }

    

    

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
