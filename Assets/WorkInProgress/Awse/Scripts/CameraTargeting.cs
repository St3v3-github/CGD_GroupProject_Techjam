using UnityEngine;

public class CameraTargeting : MonoBehaviour
{
    public Transform cameraTarget;

    private void Update()
    {
        transform.position = cameraTarget.position;
    }
}
