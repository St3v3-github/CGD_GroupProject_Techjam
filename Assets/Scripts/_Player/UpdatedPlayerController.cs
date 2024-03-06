using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Audio;
using static InputManager;

public class UpdatedPlayerController : MonoBehaviour
{
    [Header("Camera Settings")] //Cinemachine stuff
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float rotationSpeed = 1f;

    [SerializeField] private GameObject playerObject;

    [Header("Movement Settings")]
    [Header("Movement Speed Values")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float dashSpeed;
    public float swingSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    [Header("Movement Speed Scalers")]
    public float speedMultiplier;
    public float speedIncreaseMultiplier;
    public float sloperIncreaseMultiplier;

    [Header("Component Registry")]
    public ComponentRegistry components;

    [Header("Player Height & Ground Settings")]
    public float playerHeight;
    public LayerMask groundMask;
    public bool isGrounded;
    public float groundDrag;

    [Header("Crouch Settings")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;
    public bool crouchPressed = false;

    [Header("Jump Settings")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public float jumpForceMultiplier;
    public float maxJumpForce;
    private float existingJumpForce;
    public bool readyToJump;
    public bool chargingJump;
    public bool sprintPressed = false;

    [Header("Slide Settings")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    bool exitingSlope;

    [Header("Booleans")]
    public bool sliding;
    public bool dashing;
    public bool freeze;
    public bool activeGrapple;
    public bool swinging;
    public bool hasDoubleJumped = false;
    public bool hasJumpedThisFrame;
    private int jumpCount;
    public int maxJumpCount;

    private bool enableMovementOnNextTouch;

    Vector3 movementDirection;
    Vector3 velocityToSet;

    public MovementState state;

    [SerializeField] private GameObject player;

    public enum MovementState
    {
        freeze,
        idle,
        walking,
        sprinting,
        crouching,
        sliding,
        dashing,
        swinging,
        air
    }

    private void StateHandler()
    {
        //Crouch just compresses the character right now, need to hook up the crouch to the animation manager in the future
        //and just make the hitbox of the player smaller.
        
        if(freeze)
        {
            state = MovementState.freeze;
            moveSpeed = 0;
            components.rigidBody.velocity = Vector3.zero;
        }

        else if(dashing)
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashSpeed;
        }

        else if(sliding)
        {
            state = MovementState.sliding;

            if(OnSlope() && components.rigidBody.velocity.y < 0.1f)
            {
                desiredMoveSpeed = slideSpeed;
            }

            else
            {
                desiredMoveSpeed = sprintSpeed;
            }
        }

        else if(swinging)
        {
            state = MovementState.swinging;
            moveSpeed = swingSpeed;
        }

        else if(crouchPressed)
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }

        else if (isGrounded && sprintPressed)
        {
            state = MovementState.sprinting;
            components.animationManager.toggleGroundedBool(true);

            if (movementDirection.x != 0 || movementDirection.z != 0)
            {
                components.animationManager.toggleWalkingBool(true);
            }

            else
            {
                components.animationManager.toggleWalkingBool(false);
            }

            desiredMoveSpeed = sprintSpeed;
        }
        else if (isGrounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
            components.animationManager.toggleGroundedBool(true);

            if (movementDirection.x != 0 || movementDirection.z != 0)
            {
                components.animationManager.toggleWalkingBool(true);                
            }

            else 
            {
                components.animationManager.toggleWalkingBool(false);
            }
        }

        /*
        else if(isGrounded && (movementDirection.x == 0 && movementDirection.z == 0) && readyToJump)
        {
            state = MovementState.idle;
            moveSpeed = 0;
            animControl.toggleWalkingBool(false);
            animControl.toggleGroundedBool(true);
        }
        */

        else
        {
            state = MovementState.air;
            components.animationManager.toggleGroundedBool(false);
            components.animationManager.toggleEmotingBool(false);
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

    void Start()
    {
        cameraTransform = components.playerCamera.transform;
       
        readyToJump = true;

        startYScale = transform.localScale.y;

        jumpCount = maxJumpCount;
        existingJumpForce = jumpForce;
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation();

        SpeedControl();

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);

        if(isGrounded)
        {
            jumpCount = maxJumpCount;
            components.animationManager.toggleJumpingBool(false);
        }

        if (state == MovementState.walking || state == MovementState.sprinting || state == MovementState.crouching && !activeGrapple)
        {
            components.rigidBody.drag = groundDrag;
        }

        else
        {
            components.rigidBody.drag = 0;
        }

        if (crouchPressed)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            components.rigidBody.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        else if (!crouchPressed) 
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

        StateHandler();

        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            Debug.Log(moveSpeed);
        }

        if(chargingJump && jumpForce < maxJumpForce)
        {
            jumpForce += (jumpForceMultiplier * Time.deltaTime);
        }
    }

    public void HandleRotation()
    {
        float targetAngle = cameraTransform.rotation.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        playerObject.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation.normalized, rotationSpeed * Time.deltaTime);

        Debug.Log(targetRotation);
    }

    public void HandleMovement(Vector2 movementInput)
    {
        if(activeGrapple)
        {
            return;
        }

        if(swinging)
        {
            return;
        }

        //Sets player directionto camera direction
        movementDirection = cameraTransform.forward.normalized * movementInput.y + cameraTransform.right.normalized * movementInput.x;

        components.animationManager.updateMovementFloats(new Vector2(movementInput.x * desiredMoveSpeed, movementInput.y * desiredMoveSpeed));

        if (OnSlope() && !exitingSlope)
        {
            //Debug.Log(1);
            components.rigidBody.AddForce(GetSlopeMoveDirection(movementDirection) * moveSpeed * speedMultiplier * 20f, ForceMode.Force);

            if (components.rigidBody.velocity.y > 0)
                components.rigidBody.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        else if (isGrounded)
        {
            if (movementInput.y > 0)
            {
                components.rigidBody.AddForce(movementDirection.normalized * moveSpeed * speedMultiplier * 20f, ForceMode.Force);
            }

            else if (movementInput.x != 0)
            {
                components.rigidBody.AddForce(movementDirection.normalized * moveSpeed * speedMultiplier * 15f, ForceMode.Force);
            }

            else
            {
                components.rigidBody.AddForce(movementDirection.normalized * moveSpeed * speedMultiplier * 10f, ForceMode.Force);
            }
        }

        else if (!isGrounded)
        {
            components.rigidBody.AddForce(movementDirection.normalized * moveSpeed * 10f * speedMultiplier * airMultiplier, ForceMode.Force);
        }

        components.rigidBody.useGravity = !OnSlope();
        
    }

    public void HandleCamera(Vector2 cameraInput)
    {
        float camX = cameraInput.x * Time.deltaTime * sensX;
        float camY = cameraInput.y * Time.deltaTime * sensY;

        yRotation += camX;

        xRotation -= camY;
        xRotation = Mathf.Clamp(xRotation, -10, 30);

        components.playerCamera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        player.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        
    }

    public void HandleJump()
    {
        //Debug.Log("test");
        readyToJump = false;
        jumpCount -= 1;

        if (jumpCount > 0)
        {
            crouchPressed = false;
            Jump();
        }
      
        Invoke(nameof(ResetJump), jumpCooldown);
    }

    public void HandlePound()
    {
        if(!isGrounded)
        {
            components.rigidBody.AddForce(Vector3.down * 1000000f, ForceMode.Impulse);
        }
        
    }

    private void SpeedControl()
    {
        if(activeGrapple)
        {
            return;
        }

        if(OnSlope())
        {
            if(components.rigidBody.velocity.magnitude > moveSpeed)
            {
                components.rigidBody.velocity = components.rigidBody.velocity.normalized * moveSpeed;
            }
        }

        else
        {
            Vector3 flatVelocity = new Vector3(components.rigidBody.velocity.x, 0f, components.rigidBody.velocity.z);

            if (flatVelocity.magnitude > moveSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
                components.rigidBody.velocity = new Vector3(limitedVelocity.x, components.rigidBody.velocity.y, limitedVelocity.z);
            }
        }        
    }

    private void Jump()
    {
        exitingSlope = true;

        components.rigidBody.velocity = new Vector3(components.rigidBody.velocity.x, 0f, components.rigidBody.velocity.z);

        components.animationManager.toggleJumpingBool(true);

        components.rigidBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        //jumpCount -= 1;
    }

    private void ResetJump()
    {
        readyToJump = true;

        components.animationManager.toggleJumpingBool(false);

        jumpForce = existingJumpForce;
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

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacement = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 yVelocity = Vector3.up * Mathf.Sqrt(displacement - 2 * gravity * trajectoryHeight);
        Vector3 xzVelocity = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) + Mathf.Sqrt(2 * (displacement - trajectoryHeight) / gravity));


        return yVelocity + xzVelocity;
    }

    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        components.rigidBody.velocity = velocityToSet;
    }

    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;

        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);

        Invoke(nameof(ResetRestriction), 3f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestriction();

            components.grappling.stopGrapple();
        }
    }

    private void ResetRestriction()
    {
        activeGrapple = false;
    }
}
