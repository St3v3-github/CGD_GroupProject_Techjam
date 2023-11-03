using UnityEngine;

public class CameraController : MonoBehaviour
{
    InputManager inputManager;

    public float sensitivityX;
    public float sensitivityY;

    public Transform orientation;

    float rotationX;
    float rotationY;

    //private Quaternion character_target_rotation;
    //private Transform character_transform;
   

    /*public void Init(Transform _character)
    {
        character_target_rotation = _character.localRotation;
        character_transform = _character;
    }*/

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
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

        //rotate player
       // character_target_rotation *= Quaternion.Euler(0f, rotationY, 0f);

        //character_transform.localRotation = character_target_rotation;

    }
}
