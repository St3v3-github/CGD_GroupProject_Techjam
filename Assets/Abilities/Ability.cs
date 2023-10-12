using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public string ability_name;
    public float ability_cooldown;
    public float active_time;

    public virtual void Activate(GameObject parent)
    {

    }
}
