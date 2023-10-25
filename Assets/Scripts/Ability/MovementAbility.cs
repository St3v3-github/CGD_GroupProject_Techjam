using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName = "Movement Ability (TEST)", menuName = "Ability/Movement Ability(TEST)")]
public class MovementAbility : BaseAbility
{
    public override void Awake()
    {
        SetAbilityName("TEST");
        SetAbilityCooldown(1.5f);
        SetAbilityActiveTime(1.0f);
        SetAbilityCost(0);

        SetAbilityControlType(AbilityControlType.CASTING);
    }
    //scuffed implementation for testing!!
    //public float dash_velocity;
    public override void Activate(GameObject parent)
    {
        //CHANGING RUN SPEED JUST TO CHECK IF ABILITY ACTIVATES CORRECTLY

        //movement.runSpeed = 20;       !!Caused an Error
        Debug.Log("Used " + GetAbilityName());
    }

    public override void BeginCooldown(GameObject parent)
    {
        PlayerController movement = parent.GetComponent<PlayerController>();
        //movement.runSpeed -= 15;      !!Caused an Error
    }
    public override void ResetCooldown()
    {
        SetAbilityCooldown(1.5f);
        SetAbilityActiveTime(1.0f);
        SetAbilityCost(20);
    }
}
