using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Spell : MonoBehaviour
{

    public statusEnum status = new statusEnum();
    public StatusEffect currentStatus;

    public string targetTag;

    public void setStatus()
    {
        switch (status)
        {
            case statusEnum.fire:
                currentStatus = new Fire();
                break;
            case statusEnum.lightning:
                // Change Later
                currentStatus = new Fire();
                break;
            case statusEnum.ice:
                currentStatus = new Ice();
                break;
            case statusEnum.wind:
                // Change Later
                currentStatus = new Fire();
                break;
            default:
                currentStatus = null;
                break;
        }
    }

    public void setStatus(statusEnum statusInput)
    {
        switch (statusInput)
        {
            case statusEnum.fire:
                currentStatus = new Fire();
                break;
            case statusEnum.lightning:
                // Change Later
                currentStatus = new Fire();
                break;
            case statusEnum.ice:
                currentStatus = new Ice();
                break;
            case statusEnum.wind:
                // Change Later
                currentStatus = new Fire();
                break;
            default:
                currentStatus = null;
                break;
        }
    }

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


public enum statusEnum
{
    fire,
    lightning,
    ice,
    wind
};