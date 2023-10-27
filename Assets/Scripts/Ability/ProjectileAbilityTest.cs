using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Ability (TEST)", menuName = "Ability/Projectile Ability(TEST)")]
public class ProjectileAbilityTest : BaseAbility
{
    float original_ability_cooldown;
    float original_ability_active_time;
    int original_ability_cost;
    float original_ability_cast_time;

    //below are specific to this ability
    public GameObject projectilePrefab;
    public Transform firePoint;
    private Vector3 verticalOffset;
    public float projectileForce;

    public override void Awake()
    {
        SetAbilityName("PROJECTILE ABILITY TEST");
        verticalOffset = new Vector3(0f, 0.5f, 0f);
        projectileForce = 1f;
        SetAbilityCooldown(2.0f);
        SetAbilityActiveTime(1.5f);
        SetAbilityCost(0);
        SetAbilityCastTime(2.0f);

        SetAbilityControlType(AbilityControlType.CASTING);

        original_ability_cooldown = GetAbilityCooldown();
        original_ability_active_time = GetAbilityActiveTime();
        original_ability_cast_time = GetAbilityCastTime();
    }

    public override void Activate(GameObject parent)
    {
        firePoint = parent.GetComponent<Transform>();
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position - verticalOffset + firePoint.forward, firePoint.rotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(firePoint.forward * projectileForce, ForceMode.Impulse);
            Debug.Log("used");
        }
    }

    public override void BeginCooldown(GameObject parent)
    {
        
    }

    public override void ResetCooldown()
    {
        SetAbilityCooldown(original_ability_cooldown);
        SetAbilityActiveTime(original_ability_active_time);
    }

    public override void ResetCastTime()
    {
        SetAbilityCastTime(original_ability_cast_time);
    }
}
