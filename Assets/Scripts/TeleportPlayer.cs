using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public GameObject teleportLocation;
    public GameObject playercamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TELEPORTER TRIGGERED");
        
            other.gameObject.transform.position = teleportLocation.transform.position;
        playercamera.transform.position = teleportLocation.transform.position;
       
            Debug.Log("Teleported object.");

    }
}
