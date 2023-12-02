using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProjectile : Spell
{
    public GameObject projectilePrefab;
    public float projectileForce = 100f;
    private Vector3 verticalOffset = new Vector3(0f, 0.5f, 0f);

    private bool abilityReady = true;
    public float abilityCooldown;

    [SerializeField] private StatusEffect_Data _data;

    public GameObject CastSpell(Transform playerTransform)
    {
        if (abilityReady)
        {
            return Fire(playerTransform);
        }
        return null;
    }

    private GameObject Fire(Transform playerTransform)
    {
        if (playerTransform == null)
        {
            Debug.LogError("Firepoint not assigned");
            return null;
        }

        Quaternion playerRotation = Quaternion.Euler(0f, playerTransform.eulerAngles.y, 0f);
        
        GameObject projectile = Instantiate(projectilePrefab, playerTransform.position - verticalOffset + playerTransform.forward, playerRotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if(rb != null)
        {
            rb.AddForce(playerTransform.forward * projectileForce, ForceMode.Impulse);
            Debug.Log("Projectile Spell Casted");
        }
        return projectile;
    }

    private void ResetAbilityCooldown()
    {
        abilityReady = true;
    }
}
