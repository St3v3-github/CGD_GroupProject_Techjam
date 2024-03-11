using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class FireCircle : Spell
{
    public float damagePerSecond = 5f; // Adjust the damage value
    public float maxRadius = 5f; // Adjust the maximum collider radius
    public float sizeIncreaseDuration = 5f; // Adjust the duration over which the collider size increases
    public float duration = 10f;
    private ParticleSystem particle;

    private GameObject playerInPoisonCloud; // Store the reference to the player
    private float timeSinceStart = 0f;

    private void Start()
    {
        particle = GetComponent<ParticleSystem>();
        StartCoroutine(TimerCoroutine());
    }

    private void Update()
    {
        timeSinceStart += Time.deltaTime;

        // Increase the collider radius gradually over the specified duration
        float currentRadius = Mathf.Lerp(0f, maxRadius, timeSinceStart / sizeIncreaseDuration);

        ParticleSystem.ShapeModule ps = particle.shape;
        ps.radius = currentRadius;

        // Check for players within the poison cloud
        Collider[] colliders = Physics.OverlapSphere(transform.position, currentRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.transform.parent != null && collider.transform.parent.CompareTag("Player"))
            {
                // Apply damage over time
                ApplyDamageOverTime(collider.gameObject);
            }
        }
    }

    private void ApplyDamageOverTime(GameObject player)
    {
        // Calculate damage based on time
        float damage = damagePerSecond * Time.deltaTime;

        AttributeManager attributes = player.gameObject.GetComponentInChildren<AttributeManager>();

        if (attributes != null)
        {
            attributes.TakeDamage(damage);
            player.GetComponentInParent<ComponentRegistry>().playerScoreInfo.lastDamagedBy = source;
            source.GetComponent<ComponentRegistry>().uiHandler.Hit();
        }
    }

    private IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
