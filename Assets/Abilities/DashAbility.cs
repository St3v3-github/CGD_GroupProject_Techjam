using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dash Ability", menuName = "Ability/Dash Ability")]
public class DashAbility : Ability
{
    public float dash_velocity;

    public override void Activate(GameObject parent)
    {
        PlayerController movement = parent.GetComponent<PlayerController>();
        Rigidbody rigidbody = parent.GetComponent<Rigidbody>();

        //DASH STUFF HERE
        //CHANGING RUN SPEED JUST TO CHECK IF ABILITY ACTIVATES CORRECTLY
        movement.runSpeed = 20;
    }
}
