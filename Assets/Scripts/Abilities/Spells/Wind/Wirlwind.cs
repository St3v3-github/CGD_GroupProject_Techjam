using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Wirlwind : MonoBehaviour
{
    public SliderInt SliderInt = new SliderInt();
    
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            GameObject player = other.gameObject;
            
            Rigidbody rb = player.GetComponent<Rigidbody>();

            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }
}
