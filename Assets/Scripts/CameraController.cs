using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform targetTransform;       //Object camera follows
    public Transform cameraPivot;             //Object camera pivots on
    public Transform cameraTransform;    //Transform of actual camera object

    public LayerMask collisionLayers;        //Layers we want camera to collide with
    private float defaultPosition;

    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition;

    [SerializeField]
    private float cameraCollisionOffset = 0.2f;       //how much camera will jump off objects.
    [SerializeField]
    private float minimumCollisionOffset = 0.2f;
    [SerializeField]
    private float cameraCollisionRadius = 0.2f;
    
    [SerializeField]
    private float cameraFollowSpeed = 0.2f;
    [SerializeField]
    private float cameraLookSpeed = 5f;
    [SerializeField] 
    private float cameraPivotSpeed = 5f;

    [SerializeField]
    private float lookAngle;     //up and down
    [SerializeField]
    private float pivotAngle;    //left and right
    [SerializeField]
    private float minPivotAngle = -80;
    [SerializeField] 
    private float maxPivotAngle = 80;

    private Vector2 cameraInput;

    private void Awake()
    {
        defaultPosition = cameraTransform.localPosition.z;
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        cameraInput = ctx.ReadValue<Vector2>();
    }

    void LateUpdate()
    {
        HandleAllCameraMovement();
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp
            (transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);

        transform.position = targetPosition;
    }

    private void RotateCamera()
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

    private void HandleCameraCollisions()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast
            (cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition = -(distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            targetPosition = targetPosition - minimumCollisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
