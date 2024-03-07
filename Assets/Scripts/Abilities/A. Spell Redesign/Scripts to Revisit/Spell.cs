using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spell : MonoBehaviour
{
    protected string targetTag;
    public GameObject source;

    private void Awake()
    {
        source = this.gameObject;
    }
    

    public bool PlayerCheck(GameObject hitbox)
    {
        GameObject player = hitbox;
        
        if (player.layer == LayerMask.NameToLayer("layer_Player"))
        {
            return true;
        }
        return false;
    }
    private void Cast()
    {

    }

    public bool DealDamage(GameObject hitbox, float damage)
    {
        bool valid_target = false;

        GameObject player = hitbox;


        if (player.CompareTag("Player"))
        {
            // AttributeManager attributes = hitbox.GetComponent<AttributeManager>();
           
                AttributeManager attributes = player.GetComponent<ComponentRegistry>().attributeManager;
               
                if (attributes != null)
                {
                    Debug.Log("3");
                    attributes.TakeDamage(damage, source);
                    return true;
                }
            else
            {

                return false;
            }
        }
        return false;


            //Hitmarker
            //source.GetComponent<UIController>().Hit(damage);
        }
    
    
}