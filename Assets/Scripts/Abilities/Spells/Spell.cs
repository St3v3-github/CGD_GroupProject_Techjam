using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Spell : MonoBehaviour
{

    


    public string targetTag;



    public void setTargetTag()
    {
        if (this.tag == "Player1Spell")
        {
            targetTag = "Player2";
        }
        else if (this.tag == "Player2Spell")
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
    protected float damage;

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
                currentStatus = new Fire();
                break;
            case spellEnum.lightning:
                // Change Later
                currentStatus = new Fire();
                break;
            case spellEnum.ice:
                currentStatus = new Ice();
                break;
            case spellEnum.wind:
                // Change Later
                currentStatus = new Fire();
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
                currentStatus = new Fire();
                break;
            case spellEnum.lightning:
                // Change Later
                currentStatus = new Fire();
                break;
            case spellEnum.ice:
                currentStatus = new Ice();
                break;
            case spellEnum.wind:
                // Change Later
                currentStatus = new Fire();
                break;
            default:
                currentStatus = null;
                break;
        }
    }


    public virtual void setPrefab(StatusEffect status)
    {
       
        switch (status.GetStatusType())
        {
            case "fire":
                spellPrefab = firePrefab;
                damage = fireDamage;
                break;
            case "ice":
                spellPrefab = icePrefab;
                damage = iceDamage;
                break;
            case "lightning":
                spellPrefab = lightningPrefab;
                damage = lightDamage;
                break;
            case "wind":
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