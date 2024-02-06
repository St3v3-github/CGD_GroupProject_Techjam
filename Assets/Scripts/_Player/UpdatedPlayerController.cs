using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Audio;
using static InputManager;

public class UpdatedPlayerController : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Camera playerCam;

    float xRotation;
    float yRotation;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    public float speedIncreaseMultiplier;
    public float sloperIncreaseMultiplier;

    public float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float dashSpeed;

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

    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    bool exitingSlope;

    public bool sliding;
    public bool dashing;
    public bool hasDoubleJumped = false;
    public bool hasJumpedThisFrame;
    private int jumpCount;
    public int maxJumpCount;

    public MovementState state;

    //Keycode for hardcoding - when implementation done hook up to input system later on :) - Matt
    public KeyCode crouchKey = KeyCode.LeftControl;

    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        sliding,
        dashing,
        air
    }

    private void StateHandler()
    {
        //Crouch just compresses the character right now, need to hook up the crouch to the animation manager in the future
        //and just make the hitbox of the player smaller.
        

        if(dashing)
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashSpeed;
        }
        else if(sliding)
        {
            state = MovementState.sliding;

            if(OnSlope() && rb.velocity.y < 0.1f)
            {
                desiredMoveSpeed = slideSpeed;
            }
            else
            {
                desiredMoveSpeed = sprintSpeed;
            }
        }
        else if(Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }

        else if (isGrounded && sprintPressed)
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }
        else if (isGrounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
        }

        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;

        jumpCount = maxJumpCount;
    }

    // Update is called once per frame
    void Update()
    {
        
        SpeedControl();

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);

        if(isGrounded)
        {
            jumpCount = maxJumpCount;
            
        }

        if (state == MovementState.walking || state == MovementState.sprinting || state == MovementState.crouching)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        else if (Input.GetKeyUp(crouchKey)) 
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

        StateHandler();

        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            Debug.Log(moveSpeed);
        }
    }

    

    public void HandleMovement(Vector2 movementInput)
    {
        

        movementDirection = transform.forward * movementInput.y + transform.right * movementInput.x;

        if (OnSlope() && !exitingSlope)
        {
            //Debug.Log(1);
            rb.AddForce(GetSlopeMoveDirection(movementDirection) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        else if (isGrounded)
        {
            rb.AddForce(movementDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!isGrounded)
        {
            rb.AddForce(movementDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        rb.useGravity = !OnSlope();
        
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
        jumpCount -= 1;

        if (jumpCount > 0)
        {
            Jump();
        }
       

        Invoke(nameof(ResetJump), jumpCooldown);
    }

    

    private void SpeedControl()
    {
        if(OnSlope())
        {
            if(rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
            
        }
        else
        {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

                if(flatVelocity.magnitude > moveSpeed)
                {
                    Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
                    rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
                }
        }        
    }

    

    

    private void Jump()
    {
        exitingSlope = true;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        //jumpCount -= 1;
    }

    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
            
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if(OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * sloperIncreaseMultiplier * slopeAngleIncrease;
            }
            else
            {
                time += Time.deltaTime * speedIncreaseMultiplier;
            }
            
            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }
}
