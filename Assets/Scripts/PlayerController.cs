using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    InputManager inputManager;

    public Vector3 moveDirection;
    public Transform cameraTransform;
    public Rigidbody playerRB;

    public float runSpeed = 5;
    public float sprintSpeed = 10;
    public float rotationSpeed = 10.0f;

    public float jump = 10f;
    public bool onGround = true;
    public int jumps = 0;

    public void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
    }

   private void Update()
    {
        /* HandleJump();
        HandleSelect();
        HandleAttack();

    *Good to have these things in update when we get to implementing them*
        */
    }

    void FixedUpdate()
    {
        HandleAllPlayerMovement();
    }

    void HandleAllPlayerMovement()
    {
        HandlePlayerRotation();
        HandlePlayerMovement();
    }

    void HandlePlayerRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraTransform.forward * inputManager.movementInputY;
        targetDirection = targetDirection + cameraTransform.right * inputManager.movementInputX;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    void HandlePlayerMovement()
    {
        Vector3 forward = transform.InverseTransformVector(cameraTransform.forward);
        Vector3 right = transform.InverseTransformVector(cameraTransform.right);

        forward.y = 0;
        right.y = 0;

        forward = forward.normalized;
        right = right.normalized;

        Vector3 FRVI = inputManager.movementInputY * forward;
        Vector3 RRVI = inputManager.movementInputX * right;

        Vector3 CRM = FRVI + RRVI;

        transform.Translate(CRM * runSpeed * Time.fixedDeltaTime);
    }
   
    ////dont worry about anything below, legacy code and might use it later :)

   /* private void HandleSelect()
    {
        if (inputManager.selectInput && buttonLogic.btnPressable)
        {
            CutsceneCam.SetActive(true);
            buttonLogic.DoorOpen();
            inputManager.selectInput = false;
        }

        else if (!buttonLogic.btnPressable)
        {
            CutsceneCam.SetActive(false);
        }
    }*/

    /*private void HandleAttack()
    {
        if (inputManager.attackInput)
        {
            animator.Play("Base Layer.MMAKick", 0, 0.25f);
        }
    }*/

    /*private void HandleJump()
    {
        if (inputManager.jumpInput && onGround)
        {
            inputManager.jumpInput = false;
            animator.Play("Base Layer.JumpFlip", 0, 0.25f);
            playerRB.AddForce(new Vector3(0, jump, 0), ForceMode.Impulse);
        }

        if (powerup.aquired == true)
        {
            if (inputManager.jumpInput && jumps == 1)
            {
                animator.Play("Base Layer.JumpFlip", 0, 0.25f);
                playerRB.AddForce(new Vector3(0, jump / 1.5f, 0), ForceMode.Impulse);
                jumps++;
            }
        }
    }*/

/*    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            animator.SetBool("isJumping", false);
            onGround = true;
            jumps = 0;
        }
    }*/

/*    private void OnCollisionExit(Collision collision)
    {
        onGround = false;
        jumps++;
    }*/
}

