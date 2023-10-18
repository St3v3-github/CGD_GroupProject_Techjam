using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "Blink Ability (TEST)", menuName = "Ability/Blink Ability(TEST)")]
public class BlinkAbilityTest : BaseAbility
{
    float original_ability_cooldown;
    float original_ability_active_time;
    int original_ability_cost;

    //below are specific to this ability
    public float distance;
    public float speed;
    public float dest_multiplier;
    public float camera_height;
    Vector3 destination;
    bool blinking = false;
    public Transform camera;
    public LayerMask layer_mask;
    public ParticleSystem blink_trail;
    public override void Awake()
    {
        SetAbilityName("BLINK ABILITY TEST");
        SetAbilityCooldown(2.0f);
        SetAbilityActiveTime(0.5f);
        SetAbilityCost(30);

        original_ability_cooldown = GetAbilityCooldown();
        original_ability_active_time = GetAbilityActiveTime();
        original_ability_cost = GetAbilityCost();
    }

    public override void Activate(GameObject parent)
    {
        camera = parent.GetComponent<Transform>();
        Rigidbody rigidbody = parent.GetComponent<Rigidbody>();
        PlayerController movement = parent.GetComponent<PlayerController>();

        blink_trail.Play();
        RaycastHit hit;
        if(Physics.Raycast(camera.position, camera.forward, out hit, distance, layer_mask))
        {
            //if dest is 1, player ends on the target if there is an object. 0.9, very close next to it etc.
            destination = hit.point * dest_multiplier;
            Debug.DrawLine(camera.position, hit.point * dest_multiplier, Color.green, 2);
        }
        else
        {
            destination = (camera.position + camera.forward.normalized * distance) * dest_multiplier;
            Debug.DrawRay(camera.position, (camera.forward * distance) * dest_multiplier, Color.yellow, 2);
        }
        destination.y += camera_height;

        /*if(blinking)
        {
            var distance = Vector3.Distance(parent.transform.position, destination);
            if (distance > 0.5f)
            {
                parent.transform.position = Vector3.MoveTowards(parent.transform.position, destination, Time.deltaTime * speed);
            }
            else
            {
                blinking = false;
            }
        }*/

        Debug.Log("Used " + GetAbilityName());
    }

    public override void BeginCooldown(GameObject parent)
    {
       
    }
    public override void ResetCooldown()
    {
        SetAbilityCooldown(original_ability_cooldown);
        SetAbilityActiveTime(original_ability_active_time);
        SetAbilityCost(original_ability_cost);
    }
}
