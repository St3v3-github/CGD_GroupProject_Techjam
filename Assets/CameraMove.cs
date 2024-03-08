using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform FirstPersonPosition;
    public Transform ThirdPersonPosition;
    public bool isThirdPerson;

    void Update()
    {
        if (isThirdPerson)
        {
            transform.position = ThirdPersonPosition.position;
        }

        else
        {
            transform.position = FirstPersonPosition.position;
        }
    }
}
