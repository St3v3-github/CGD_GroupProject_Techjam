using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatedCameraController : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateCamera(Vector2 cameraInput)
    {
        float camX = cameraInput.x * Time.deltaTime * sensX;
        float camY = cameraInput.y * Time.deltaTime * sensY;

        yRotation += camX;

        xRotation -= camY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        orientation.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        transform.rotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
