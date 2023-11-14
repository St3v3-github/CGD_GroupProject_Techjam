using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float startScale = 0.1f;
    public float targetScale = 2.0f;
    public float scaleSpeed = 0.1f; // Adjust the speed of scaling
    private Transform objectTransform;
    private bool isScaling = true;
    // Start is called before the first frame update
    void Start()
    {
        objectTransform = transform;
        objectTransform.localScale = new Vector3(startScale, startScale, startScale);
    }

    // Update is called once per frame
    void Update()
    {
         if (isScaling)
        {
            // Interpolate the current scale towards the target scale over time
            float currentScale = objectTransform.localScale.x;
            float newScale = Mathf.MoveTowards(currentScale, targetScale, scaleSpeed * Time.deltaTime);

            // Apply the new scale to all dimensions (assuming uniform scale)
            objectTransform.localScale = new Vector3(newScale, newScale, newScale);

            // Check if the target scale has been reached
            if (newScale >= targetScale)
            {
                isScaling = false;
            }
        }
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
