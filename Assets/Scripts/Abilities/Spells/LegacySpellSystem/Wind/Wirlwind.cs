using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Wirlwind : MonoBehaviour
{
    public float bounceStrength = 5;
    
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            GameObject player = other.gameObject;
            
            Rigidbody rb = player.GetComponent<Rigidbody>();

            rb.AddForce(Vector3.up * bounceStrength, ForceMode.Impulse);
        }
    }
}
