using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killbox : MonoBehaviour
{
     [SerializeField] private LayerMask layer_Player;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == layer_Player)
        {
            //AttributeManager playerAttribute = other.GetComponent<ComponentRegistry>().attributeMananger;
            //playerAttribute.currentHealth = -1;
            other.GetComponent<PlayerScoreInfo>().lastDamagedBy = null;
            Debug.Log("Dead");

        }
    }
}
