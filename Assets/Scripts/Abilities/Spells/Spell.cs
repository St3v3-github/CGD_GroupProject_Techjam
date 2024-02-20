using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ItemData;
using static UnityEngine.GraphicsBuffer;

public class Spell : MonoBehaviour
{
    protected SpellData spell;
    protected string targetTag;
    public GameObject source;

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("layer_Spell");
        source = this.gameObject;
    }
    

    public void SetTargetTag()
    {
        if (tag != "Player1Spell" || tag != "Player2Spell")
        {
            if (source.CompareTag("Player1"))
            {
                this.tag = "Player1Spell";
            
            } 
            else if (source.CompareTag("Player2"))
            {
                this.tag = "Player2Spell";
            }
          
        }
        
        
        Debug.Log("TRYING TO SET TARGET TAG");
        if (this.tag == "Player1Spell" || this.tag == "Player1")
        {
            targetTag = "Player2";
        }
        else if (this.tag == "Player2Spell" || this.tag == "Player2")
        {
            targetTag = "Player1";
        }
       
    }

    public bool PlayerCheck(GameObject hitbox)
    {
        GameObject player;
        if (hitbox.transform.parent != null)
        {
            player = hitbox.transform.parent.gameObject;
        }
        else
        {
            player = hitbox;
        }

        
        if (player.layer == LayerMask.NameToLayer("layer_Player"))
        {
            return true;
        }
        return false;
    }

    public bool DealDamage(GameObject hitbox, float damage)
    {
        bool valid_target = false;
        
        GameObject player;
       /* if (hitbox.transform.parent != null)
        {
            player = hitbox.transform.parent.gameObject;
        }
        else
        {
            player = hitbox;
        }*/
        player = hitbox;
      
        
        if (player.layer == LayerMask.NameToLayer("layer_Player") && player.tag != source.tag)
        {
            Debug.Log("1");
            Debug.Log(hitbox.name);
           // AttributeManager attributes = hitbox.GetComponent<AttributeManager>();
           if (hitbox.tag == "Player1" || hitbox.tag == "Player2")
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

    public bool dealDamage(GameObject hitbox, float damage)
    {
        bool valid_target = false;

        GameObject player;
        if (hitbox.transform.parent != null)
        {
            player = hitbox.transform.parent.gameObject;
        }
        else
        {
            player = hitbox;
        }
        player = hitbox;


        if (player.layer == LayerMask.NameToLayer("layer_Player") && player.tag != source.tag)
        {
            Debug.Log("1");
            Debug.Log(hitbox.name);
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

    public bool DealDamage(GameObject hitbox, float damage, bool strike)
    {
        bool valid_target = false;
        
        GameObject player;
        if (hitbox.transform.parent != null)
        {
            player = hitbox.transform.parent.gameObject;
        }
        else
        {
            player = hitbox;
        }
     
        

        if (player.layer == LayerMask.NameToLayer("layer_Player"))
        {

            
           // AttributeManager attributes = hitbox.GetComponent<AttributeManager>();
           if (hitbox.name == "Player(Clone)" || hitbox.name == "AttributeController")
           {
               AttributeManager attributes = player.GetComponent<UIController>().attributeController.GetComponent<AttributeManager>();
                if(!attributes.hit_by_strike)
                {
                    attributes.hit_by_strike = true;
                    if (attributes != null)
                    {
                        attributes.TakeDamage(damage, source);
                        return true;
                    }
                }
                else
                {
                    attributes.hit_by_strike = false;
                }
               
           }
           

            //Hitmarker
            source.GetComponent<UIController>().Hit(damage);
        }
        return false;
        
    }

    public bool playerCheck(GameObject hitbox)
    {
        GameObject player;
        if (hitbox.transform.parent != null)
        {
            player = hitbox.transform.parent.gameObject;
        }
        else
        {
            player = hitbox;
        }


        if (player.layer == LayerMask.NameToLayer("layer_Player"))
        {
            return true;
        }
        return false;
    }

    public virtual void Cast()
    {

    }
}

public class ElementalSpell : Spell
{
    //Testing :)
    public bool testing = true;
    public spellEnum spellType = new spellEnum();

    //public StatusEffect currentStatus;

    public SpellData fireSpell;
    public SpellData iceSpell;
    public SpellData lightningSpell;
    public SpellData windSpell;

    public void testingSwitch()
    {
        if (testing)
        {
            SetType(spellType);
        }
    }

    public void SetStatus()
    {
        //REPLACE WITH ROSA LOGIC
        /*switch (spellType)
        {
            case spellEnum.fire:
                currentStatus = GetComponent<Fire>();
                break;
            case spellEnum.lightning:
                // Change Later
                currentStatus = GetComponent<Fire>();
                break;
            case spellEnum.ice:
                currentStatus = GetComponent<Ice>();
                break;
            case spellEnum.wind:
                // Change Later
                currentStatus = GetComponent<Fire>();
                break;
            default:
                currentStatus = null;
                break;
        }*/
    }

    public void SetStatus(spellEnum statusInput)
    {
        //REPLACE WITH ROSA LOGIC
        /*switch (statusInput)
        {
            case spellEnum.fire:
                currentStatus = GetComponent<Fire>();
                break;
            case spellEnum.lightning:
                // Change Later
                currentStatus = GetComponent<Fire>();
                break;
            case spellEnum.ice:
                currentStatus = GetComponent<Ice>();
                break;
            case spellEnum.wind:
                // Change Later
                currentStatus = GetComponent<Fire>();
                break;
            default:
                currentStatus = null;
                break;
        }*/
    }


    public virtual void SetType(spellEnum statusInput)
    {
       
        switch (statusInput)
        {
            case spellEnum.fire:
                spell = fireSpell;
                break;
            case spellEnum.ice:
                spell = iceSpell;
                break;
            case spellEnum.lightning:
                spell = lightningSpell;
                break;
            case spellEnum.wind:
                spell = windSpell;
                break;
            default:
                spell = fireSpell;
                break;
        }
    }

    public void SetType(ItemData.SpellType type)
    {
        switch (type)
        {
            case ItemData.SpellType.FIRE:
                spell = fireSpell;
                break;
            case ItemData.SpellType.ICE:
                spell = iceSpell;
                break;
            case ItemData.SpellType.LIGHTNING:
                spell = lightningSpell;
                break;
            case ItemData.SpellType.WIND:
                spell = windSpell;
                break;
            default:
                spell = fireSpell;
                break;
        }
    }

    public void SetType(ItemData spellInfo)
    {
        /*int temp;
        if (spell.ID <= 16)
        {
            temp = spell.ID % 4;
        }*/
        switch (spellInfo.type)
        {
            case ItemData.SpellType.FIRE:
                spell = fireSpell;
                break;
            case ItemData.SpellType.ICE:
                spell = iceSpell;
                break;
            case ItemData.SpellType.LIGHTNING:
                spell = lightningSpell;
                break;
            case ItemData.SpellType.WIND:
                spell = windSpell;
                break;
            default:
                spell = fireSpell;
                break;
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