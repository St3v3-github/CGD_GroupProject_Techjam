using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Spell : MonoBehaviour
{
    protected float damage;
    protected string targetTag;

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
}

public class ElementalSpell : Spell
{

    public spellEnum spellType = new spellEnum();
    public StatusEffect currentStatus;

    public float fireDamage;
    public float lightDamage;
    public float iceDamage;
    public float windDamage;


    public GameObject firePrefab;
    public GameObject lightningPrefab;
    public GameObject icePrefab;
    public GameObject windPrefab;
    protected GameObject spellPrefab;


    public void setStatus()
    {
        switch (spellType)
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
        }
    }

    public void setStatus(spellEnum statusInput)
    {
        switch (statusInput)
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
        }
    }


    public virtual void setPrefab(spellEnum statusInput)
    {
       
        switch (statusInput)
        {
            case spellEnum.fire:
                spellPrefab = firePrefab;
                damage = fireDamage;
                break;
            case spellEnum.ice:
                spellPrefab = icePrefab;
                damage = iceDamage;
                break;
            case spellEnum.lightning:
                spellPrefab = lightningPrefab;
                damage = lightDamage;
                break;
            case spellEnum.wind:
                spellPrefab = windPrefab;
                damage = windDamage;
                break;
            default:
                spellPrefab = firePrefab;
                damage = fireDamage;
                break;
        }
    }

    public void setPrefab(ItemData spell)
    {
        ItemData.elementEnum spelltype = spell.element;
        int temp;
        if (spell.ID <= 16)
        {
            temp = spell.ID % 4;
        }
        switch (spell.ID)
        {
            case 1:
                spellPrefab = firePrefab;
                damage = fireDamage;
                break;
            case 2:
                spellPrefab = icePrefab;
                damage = iceDamage;
                break;
            case 3:
                spellPrefab = lightningPrefab;
                damage = lightDamage;
                break;
            case 4:
                spellPrefab = windPrefab;
                damage = windDamage;
                break;
            default:
                spellPrefab = firePrefab;
                damage = fireDamage;
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