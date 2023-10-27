using UnityEngine;

public class CameraTargetingZac : MonoBehaviour
{
    public Transform cameraTarget;

    private void Update()
    {
        transform.position = cameraTarget.position;
    }
}
