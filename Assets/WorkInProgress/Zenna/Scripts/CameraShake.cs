using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public ComponentRegistry registry;

    public bool Shaking = false;
    //public Transform ThirdPersonPosition;
    void Update()
    {
        

        if (Input.GetKeyDown("x"))
        {
            
            
            StartCoroutine(Shake(.15f, .05f));
        }

        if (Shaking == true)
        {
            StartCoroutine(Shake(.15f, .05f));
            Debug.Log("SHAKE");
        }

    }
    public IEnumerator Shake(float duration, float magnitude)
    {
       // Debug.Log("CAMERA SHAKE");

        Vector3 originalPos = registry.playerCamera.transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            registry.playerCamera.transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }
        registry.playerCamera.transform.localPosition = originalPos;
    }
}
