using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemScript : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Destroy(gameObject); 
    }
}
