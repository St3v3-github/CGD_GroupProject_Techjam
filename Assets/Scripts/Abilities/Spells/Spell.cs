using System.Collections;
using System.Collections.Generic;
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
    }

    public void setTargetTag()
    {
        if (this.tag == "Player1Spell" || this.tag == "Player1")
        {
            targetTag = "Player2";
        }
        else if (this.tag == "Player2Spell" || this.tag == "Player2")
        {
            targetTag = "Player1";
        }
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

        
        if (player.layer == LayerMask.NameToLayer("layer_Player") && player.tag != source.tag)
        {
            return true;
        }
        return false;
    }

    public bool dealDamage(GameObject hitbox, float damage)
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
        if (player.layer == LayerMask.NameToLayer("layer_Player") && player.tag != source.tag)
        {
            AttributeManager attributes = hitbox.GetComponent<AttributeManager>();

            if (attributes != null)
            {
                attributes.TakeDamage(damage, source);
                return true;
            }

            //Hitmarker
            source.GetComponent<UIController>().Hit(damage);
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
            setType(spellType);
        }
    }

    public void setStatus()
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

    public void setStatus(spellEnum statusInput)
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


    public virtual void setType(spellEnum statusInput)
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

    public void setType(ItemData.SpellType type)
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

    public void setType(ItemData spellInfo)
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