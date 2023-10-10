using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControlsAsset playerControlsAsset;

    public Vector2 leftStickInput;
    public Vector2 rightStickInput;

    public float movementInputX;  // horizontalInput
    public float movementInputY;  // verticalInput

    public float cameraInputX;
    public float cameraInputY;

    public bool jumpInput = false;
    public bool sprintInput = false;

    //also legacy

/*     public bool lockOnInput = false;
    public bool selectInput = false;
    public bool attackInput = false;*/

/*    private void Awake()
    {
        animator = GetComponent<Animator>();
    }*/

    private void Update()
    {
        HandleAllInputs();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleCameraInput();
    }

    private void HandleMovementInput()
    {
        movementInputX = leftStickInput.x;
        movementInputY = leftStickInput.y;

        //old animation stuff

/*        if (movementInputX != 0 || movementInputY != 0)
        {
            animator.SetBool("isRunning", true);
        }

        else
        {
            animator.SetBool("isRunning", false);
        }*/

    }

    private void HandleCameraInput()
    {
        cameraInputX = rightStickInput.x;
        cameraInputY = rightStickInput.y;
    }

    private void OnEnable()
    {
        if (playerControlsAsset == null)
        {
            playerControlsAsset = new PlayerControlsAsset();

            playerControlsAsset.Player.Movement.performed += ctx => leftStickInput = ctx.ReadValue<Vector2>();
            playerControlsAsset.Player.Camera.performed += ctx => rightStickInput = ctx.ReadValue<Vector2>();

            playerControlsAsset.Player.Jump.performed += ctx => jumpInput = true;
            playerControlsAsset.Player.Jump.canceled += ctx => jumpInput = false;

            playerControlsAsset.Player.Sprint.started += ctx => sprintInput = true;
            playerControlsAsset.Player.Sprint.canceled += ctx => sprintInput = false;

            //legacy

/*         playerControlsAsset.Player.Select.started += ctx => selectInput = true;
            playerControlsAsset.Player.Select.canceled += ctx => selectInput = false;

            playerControlsAsset.Player.Attack.started += ctx => attackInput = true;
            playerControlsAsset.Player.Attack.canceled += ctx => attackInput = false;

            playerControlsAsset.Player.LockOn.started += ctx => lockOnInput = true;
            playerControlsAsset.Player.LockOn.canceled += ctx => lockOnInput = false;*/
        }

        playerControlsAsset.Enable();
    }

    private void OnDisable()
    {
        playerControlsAsset.Disable();
    }
}