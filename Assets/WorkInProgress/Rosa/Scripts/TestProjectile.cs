using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileForce = 100f;
    private Vector3 verticalOffset = new Vector3(0f, 0.5f, 0f);

    private bool abilityReady = true;
    public float abilityCooldown;

    public StatusEffect_Data effect;

    private void Awake()
    {
        Debug.Log("awake");

        effect = GetComponent<StatusEffect_Data>();
        if (effect == null)
        {
            Debug.Log("Effect not found");
        }
        Debug.Log("Data: " + effect.name);
    }
    public GameObject CastSpell(Transform playerTransform)
    {
        if (abilityReady)
        {
            return Fire(playerTransform);
        }
        return null;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected");
        if (collision.gameObject.CompareTag("Player")/* || collision.gameObject.layer == 3*/)
        {
            Debug.Log("collision = Player");

            

            enemystatuseffects effects = collision.gameObject.GetComponentInParent<enemystatuseffects>();

            if(effects != null)
            {
                Debug.Log("effects detected");
                effects.ApplyEffect(effect);
            }
            
        }
        Destroy(gameObject);

        return;

        AttributeManager attributes = collision.gameObject.GetComponent<AttributeManager>();       
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
