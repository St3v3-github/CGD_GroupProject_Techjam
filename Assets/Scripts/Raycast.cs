using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class Raycast : MonoBehaviour
{
    InputManager inputManager;
    public float range = 5;
    public GameObject player;
    public GameObject target;

    public void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
    }

    void Update()
    {
        //if(inputManager.interactInput)    !!Caused an Error
        target = null;
        Vector3 direction = Vector3.forward;
        Ray ray = new Ray(transform.position, transform.TransformDirection(direction * range));
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            if (hit.collider.gameObject.TryGetComponent(out ItemScript itemScript))
            {
                ItemData new_item_data = hit.collider.gameObject.GetComponent<ItemInfo>().GetItemData();
                switch(new_item_data.type)
                {
                case 0:
                    //display element pickup text
                    break;
                case 1:
                    //display spell rune pickup text
                    break;
                default:
                    break;
                }
                target = hit.collider.gameObject;
            }
        }
    }
}
