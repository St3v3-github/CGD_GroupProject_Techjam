using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName = "Movement Ability (TEST)", menuName = "Ability/Movement Ability(TEST)")]
public class MovementAbility : BaseAbility
{
    //scuffed implementation for testing!!
    //public float dash_velocity;
    public override void Activate(GameObject parent)
    {
        Rigidbody rigidbody = parent.GetComponent<Rigidbody>();
        PlayerController movement = parent.GetComponent<PlayerController>();

        //DASH STUFF HERE
        //CHANGING RUN SPEED JUST TO CHECK IF ABILITY ACTIVATES CORRECTLY

        movement.runSpeed = 20;
        Debug.Log("Used " + ability_name);
    }

    public override void BeginCooldown(GameObject parent)
    {
        PlayerController movement = parent.GetComponent<PlayerController>();
        movement.runSpeed -= 15;
    }
}
