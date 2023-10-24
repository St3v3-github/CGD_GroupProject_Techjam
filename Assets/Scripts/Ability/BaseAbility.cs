using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAbility : ScriptableObject
{
    //should be private for final (as we use getters and setters) but keep public for dev so we can eyeball inspector
    public string ability_name;
    public float ability_cooldown;
    public float active_time;
    public int ability_cost;
    public float cast_time;

    //image and description for hotbar icon/pickup?
    //public Texture2D ability_icon;
    //public string ability_description;

    public enum AbilityState
    {
        READY,
        ACTIVE,
        COOLDOWN
    }

    public enum AbilityControlType
    {
        INSTANT,
        CASTING
        //MULTIPRESS, etc.
    }

    public AbilityState state = AbilityState.READY;
    public AbilityControlType control_type;
    public virtual void Awake()
    {

    }

    public virtual void Activate(GameObject parent)
    {

    }
    public virtual void BeginCooldown(GameObject parent)
    {

    }
    public virtual void ResetCooldown()
    {

    }

    public virtual void ResetCastTime()
    {

    }


    public string GetAbilityName()
    {
        return ability_name;
    }
    public void SetAbilityName(string _ability_name)
    {
        ability_name = _ability_name;
    }

    public float GetAbilityCooldown()
    {
        return ability_cooldown;
    }
    public void SetAbilityCooldown(float _cooldown)
    {
        ability_cooldown = _cooldown;
    }

    public float GetAbilityActiveTime()
    {
        return active_time;
    }
    public void SetAbilityActiveTime(float _time)
    {
        active_time = _time;
    }

    public int GetAbilityCost()
    {
        return ability_cost;
    }
    public void SetAbilityCost(int _cost)
    {
        ability_cost = _cost;
    }

    public AbilityState GetAbilityState()
    {
        return state;
    }
    public void SetAbilityState(AbilityState _state)
    {
        state = _state;
    }

    /*public Texture2D GetAbilityIcon()
    {
        return ability_icon;
    }

    public string GetAbilityDesc()
    {
        return ability_description;
    }
    public void SetAbilityDescription(string _ability_description)
    {
        ability_description = _ability_description;
    }*/

    public float GetAbilityCastTime()
    {
        return cast_time;
    }
    public void SetAbilityCastTime(float _cast_time)
    {
        cast_time = _cast_time;
    }

    public AbilityControlType GetAbilityControlType()
    {
        return control_type;
    }
    public void SetAbilityControlType(AbilityControlType _control_type)
    {
        control_type = _control_type;
    }
}
