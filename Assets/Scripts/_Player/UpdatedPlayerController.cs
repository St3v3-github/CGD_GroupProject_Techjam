using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatedPlayerController : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Camera playerCam;

    float xRotation;
    float yRotation;

    public float moveSpeed;

    public Transform orientation; //this might not be needed

    Vector3 movementDirection;
    Rigidbody rb;

    public float playerHeight;
    public LayerMask groundMask;
    bool isGrounded;
    public float groundDrag;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleMovement(Vector2 movementInput)
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);

        SpeedControl();

        if(isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0f;
        }

        movementDirection = transform.forward * movementInput.y + transform.right * movementInput.x;

        rb.AddForce(movementDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    public void HandleCamera(Vector2 cameraInput)
    {
        float camX = cameraInput.x * Time.deltaTime * sensX;
        float camY = cameraInput.y * Time.deltaTime * sensY;

        Debug.Log(camX);
        Debug.Log(camY);

        yRotation += camX;

        xRotation -= camY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        
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
}
