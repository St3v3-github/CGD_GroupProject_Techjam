using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;

     void Update()
    {
        transform.localPosition = cameraPosition.position;
    }
}
