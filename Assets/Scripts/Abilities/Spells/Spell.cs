using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ItemData;
using static UnityEngine.GraphicsBuffer;

public class Spell : MonoBehaviour
{
    protected SpellData spell;
    protected float CooldownTimer;
    protected SpellState state;
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

    public bool DealDamage(GameObject hitbox, float damage)
    {
        bool valid_target = false;

        GameObject player = hitbox;


        if (player.layer == LayerMask.NameToLayer("layer_Player") && player.tag != source.tag)
        {
            // AttributeManager attributes = hitbox.GetComponent<AttributeManager>();
            if (hitbox.tag == "PLayer1" || hitbox.tag == "Player2")
            {
                AttributeManager attributes = player.GetComponent<UIController>().attributeController.GetComponent<AttributeManager>();
                Debug.Log("2");
                if (attributes != null)
                {
                    Debug.Log("3");
                    attributes.TakeDamage(damage, source);
                    return true;
                }
            }


            //Hitmarker
            source.GetComponent<UIController>().Hit(damage);
        }
        return false;

        // THIS IS USED FOR THE STRIKE ONLY, NEEDED A SEPERATE FUNCTION TO ALLOW FOR HITTING SELF WITH THE SPELL
    }

    public virtual void Cast()
    {

    }

    public bool CooldownCheck()
    {
        if (state == SpellState.READY)
        {
            return true;
        }
        return false;
    }

    public void UpdateCooldown()
    {
        if(state == SpellState.COOLDOWN)
        {
            CooldownTimer += Time.deltaTime;
            
            if (CooldownTimer >= spell.cooldown)
            {
                state = SpellState.READY;
                CooldownTimer = 0;
            }
        }
    }
}



public enum spellEnum
{
    fire,
    lightning,
    ice,
    wind
};

public enum SpellState
{
    READY,
    CASTING,
    COOLDOWN
};