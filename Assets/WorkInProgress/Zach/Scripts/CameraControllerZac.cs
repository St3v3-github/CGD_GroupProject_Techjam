using UnityEngine;

public class CameraControllerZac : MonoBehaviour
{
    InputManagerZac inputManager;

    public float sensitivityX;
    public float sensitivityY;

    public Transform orientation;

    float rotationX;
    float rotationY;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManagerZac>();
    }

    private void Start()
    {
/*        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;*/
    }

    private void Update()
    {
        //Access inputs
        float inputX = inputManager.cameraInput.normalized.x * Time.deltaTime * sensitivityX;
        float inputY = inputManager.cameraInput.normalized.y * Time.deltaTime * sensitivityY;

        rotationY += inputX;
        rotationX -= inputY;
        rotationX = Mathf.Clamp(rotationX, -80f, 80f);

        //Rotate Camera + orientation
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        orientation.rotation = Quaternion.Euler(0, rotationY, 0);

    }
}
