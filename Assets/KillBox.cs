using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    LayerMask playerLayer;
    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null && other.transform.parent.gameObject.CompareTag("Player"))
        {
            var compReg = other.transform.parent.GetComponent<ComponentRegistry>();
            compReg.attributeManager.SetPlayerHealth(0);
            compReg.playerScoreInfo.lastDamagedBy = other.transform.parent.gameObject;
        }
    }
}
