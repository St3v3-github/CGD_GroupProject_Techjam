using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemScript : MonoBehaviour, IInteractable
{
  
    private void Start()
    {

        
    }
    public void Interact()
    {
        GetComponentInParent<SpawnItem>().hasItem = false;
        Destroy(gameObject); 
    }
    private void OnTriggerEnter(Collider other)
    {
        GetComponentInParent<SpawnItem>().hasItem = false;
        Destroy(gameObject);
    }
}
