using System.Transactions;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;

    private CharacterController controller;

    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private Vector2 movementInput = Vector2.zero;
    private bool jumped = false;

    private Vector2 cameraInput;
    public Transform targetTransform;       //Object camera follows
    public Transform cameraPivot;             //Object camera pivots on
    public Transform cameraTransform;    //Transform of actual camera object

    [SerializeField]
    private float cameraLookSpeed = 0.5f;
    [SerializeField]
    private float cameraPivotSpeed = 0.5f;
    [SerializeField]
    private float lookAngle;     //up and down
    [SerializeField]
    private float pivotAngle;    //left and right
    [SerializeField]
    private float minPivotAngle = -80;
    [SerializeField]
    private float maxPivotAngle = 80;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        cameraInput = ctx.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
       movementInput = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        jumped = ctx.action.triggered;
    }

    private void Update()
    {
        HandleMovement();
        HandleCamera();
    }

    private void FixedUpdate()
    {
        HandleJump();
    }

    private void HandleCamera() 
    {
        Vector3 rotation;
        Quaternion targetRotation;

        lookAngle = lookAngle + (cameraInput.x * cameraLookSpeed);
        pivotAngle = pivotAngle - (cameraInput.y * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    private void HandleMovement()
    {
        Vector3 forward = transform.InverseTransformVector(cameraTransform.forward);
        Vector3 right = transform.InverseTransformVector(cameraTransform.right);

        forward.y = 0;
        right.y = 0;

        forward = forward.normalized;
        right = right.normalized;

        Vector3 FRVI = movementInput.y * forward;
        Vector3 RRVI = movementInput.x * right;

        Vector3 CRM = FRVI + RRVI;

        controller.transform.Translate(CRM * playerSpeed * Time.fixedDeltaTime);
    }

    private void HandleJump()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Changes the height position of the player..S
        if (jumped && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
    }
}




////TEMPLATE - CAR CONTROLLER.MOVE
/*    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }*/
