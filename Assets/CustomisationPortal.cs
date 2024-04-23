using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomisationPortal : MonoBehaviour
{
    public CharSetup CharSetup;
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
        if (other.transform.parent.CompareTag("Player"))
        {
            CharSetup.EnterCharSetup(other.transform.parent.gameObject.GetComponent<ComponentRegistry>().playerInput.playerIndex);
        }
    }
}
