using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sensitivity = 2f;
    public float movementSmoothness = 0.1f; // Smaller values for smoother movement
    public float sprintMultiplier = 1.5f; // Multiplier for sprinting
    public float sneakMultiplier = 0.5f; // Multiplier for sneaking

    private float rotationX = 0f;
    private float rotationY = 0f;

    // Store the current movement direction
    private Vector3 movementDirection = Vector3.zero;

    void Start()
    {
        // Hide the mouse cursor and lock it to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Rotation
        rotationX += Input.GetAxis("Mouse X") * sensitivity;
        rotationY -= Input.GetAxis("Mouse Y") * sensitivity;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f); // Limit vertical rotation

        transform.rotation = Quaternion.Euler(rotationY, rotationX, 0f);

        // Movement
        movementDirection = Vector3.zero;

        float currentSpeed = moveSpeed; // Default speed

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= sprintMultiplier;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            currentSpeed *= sneakMultiplier;
        }

        if (Input.GetKey(KeyCode.W))
        {
            movementDirection += transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movementDirection -= transform.forward;
        }

        if (Input.GetKey(KeyCode.A))
        {
            movementDirection -= transform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movementDirection += transform.right;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            movementDirection += Vector3.up;
        }
        if (Input.GetKey(KeyCode.E))
        {
            movementDirection -= Vector3.up;
        }

        // Normalize to prevent faster diagonal movement
        movementDirection.Normalize();

        // Calculate target position
        Vector3 targetPosition = transform.position + movementDirection * currentSpeed * Time.deltaTime;

        // Smoothly move the camera
        transform.position = Vector3.Lerp(transform.position, targetPosition, movementSmoothness);
    }
}

