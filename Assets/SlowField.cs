using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowField : MonoBehaviour
{
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
        if(other.gameObject.transform.parent != null && other.transform.parent.CompareTag("Player")) {

            var compReg = other.transform.parent.GetComponent<ComponentRegistry>();
            compReg.playerController.speedMultiplier = 0.5f;
            compReg.playerController.inZone = true;
            compReg.playerController.movespeedtimer = 0;

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.transform.parent != null && other.transform.parent.CompareTag("Player"))
        {

            var compReg = other.transform.parent.GetComponent<ComponentRegistry>();
            compReg.playerController.speedMultiplier = 0.5f;
            compReg.playerController.inZone = true;
            compReg.playerController.movespeedtimer = 0;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.parent != null && other.transform.parent.CompareTag("Player"))
        {

            var compReg = other.transform.parent.GetComponent<ComponentRegistry>();
            compReg.playerController.inZone = false;

        }
    }

}
