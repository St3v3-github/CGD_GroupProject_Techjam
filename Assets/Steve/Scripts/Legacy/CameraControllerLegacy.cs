/*using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControllerLegacy : MonoBehaviour
{
    InputManager inputManager;

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

    private void Awake()
    {
        inputManager = gameObject.GetComponent<InputManager>();
    }

    private void Update()
    {
        HandleCamera();
    }

    private void HandleCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;

        lookAngle = lookAngle + (inputManager.cameraInput.x * cameraLookSpeed);
        pivotAngle = pivotAngle - (inputManager.cameraInput.y * cameraPivotSpeed);
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
}*/