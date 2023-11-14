using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CameraController : MonoBehaviour
{
    //InputManager input_manager;

    [SerializeField] private float sensitivity_x = 1f;
    [SerializeField] private float sensitivity_y = 1f;
    [SerializeField] private bool clamp_vertical_rot = true;
    [SerializeField] private float minimum_x = -90F;
    [SerializeField] private float maximum_x = 90F;
    [SerializeField] private bool smooth = false;
    [SerializeField] private float smooth_time = 5f;
    [SerializeField] private bool cursor_lock = true;

    private Quaternion character_rotation;
    private Quaternion camera_rotation;

    public void Init(Transform _character, Transform _camera)
    {
        character_rotation = _character.localRotation;
        camera_rotation = _camera.localRotation;
    }

    private void Awake()
    {
        //input_manager = FindObjectOfType<InputManager>();
    }

    private void Start()
    {
        /*      Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;*/
    }

    public void LookRotation(Vector2 cameraInput, Transform _character, Transform _camera)
    {
        //Access inputs
        //float inputX = inputManager.cameraInput.normalized.x * Time.deltaTime * sensitivityX;
        //float inputY = inputManager.cameraInput.normalized.y * Time.deltaTime * sensitivityY;

        //i dont know why this has to be set up like this, but any other way and the camera rotation is even more fucked
        //this is the only thing ive tried that seems to be fine
        float rotation_y = cameraInput.x * sensitivity_x;
        float rotation_x = cameraInput.y * sensitivity_y;

        character_rotation *= Quaternion.Euler(0f, rotation_y, 0f);
        camera_rotation *= Quaternion.Euler(-rotation_x, 0f, 0f);

        if(clamp_vertical_rot)
        {
            camera_rotation = ClampRotationAroundXAxis(camera_rotation);
        }

        if(smooth)
        {
            _character.localRotation = Quaternion.Slerp(_character.localRotation, character_rotation, smooth_time * Time.deltaTime);
            _camera.localRotation = Quaternion.Slerp(_camera.localRotation, camera_rotation, smooth_time * Time.deltaTime);
        }
        else
        {
            _character.localRotation = character_rotation;
            _camera.localRotation = camera_rotation;
        }

        CursorLock();
    }

    public void CursorLock()
    {
        if(cursor_lock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angle_x = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angle_x = Mathf.Clamp(angle_x, minimum_x, maximum_x);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angle_x);

        return q;
    }
}
