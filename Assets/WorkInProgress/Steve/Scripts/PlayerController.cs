/*using JetBrains.Annotations;
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
    //public Inventory_UI inventory_display;
    //Fix this for me later, am lazy
    public float timer = 1.0f;
    float ui_cooldown = 0.0f;
*//*
    float horizontalInput;
    float verticalInput;*//*

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
        *//*HandleMovement(Vector2 MovementInput);*//*
        //HandleGroundCheck();

        //HandleJump();
    }

    public void HandleMovement(Vector2 MovementInput)
    {
        moveDirection = orientation.forward * MovementInput.y + orientation.right * MovementInput.x;
        gameObject.transform.Translate(moveDirection * Time.deltaTime * playerSpeed);
    }

    private void HandleGroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.5f, groundLayer);
    }

    public void HandleJump()
    {
        if (isReadyToJump && isGrounded)
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

    public void HandleSprint()
    {

    }


}*/
