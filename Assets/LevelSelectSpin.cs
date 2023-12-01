using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectSpin : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public float hoverHeight = 0.1f;
    public float hoverSpeed = 0.5f;

    private void Update()
    {
        // Rotate the object
        RotateObject();

        // Hover the object up and down
        HoverObject();
    }

    private void RotateObject()
    {
        // Rotate the object around its up axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void HoverObject()
    {
        
    }
}
