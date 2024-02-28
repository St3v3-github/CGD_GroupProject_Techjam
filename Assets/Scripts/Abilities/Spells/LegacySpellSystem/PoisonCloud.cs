using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PoisonCloud : MonoBehaviour
{
    public float damagePerSecond = 5f; // Adjust the damage value
    public float maxRadius = 5f; // Adjust the maximum collider radius
    public float sizeIncreaseDuration = 5f; // Adjust the duration over which the collider size increases
    public float duration = 10f;

    private GameObject playerInPoisonCloud; // Store the reference to the player
    private float timeSinceStart = 0f;

    private void Start()
    {
        StartCoroutine(TimerCoroutine());
    }

    private void Update()
    {
        timeSinceStart += Time.deltaTime;

        // Increase the collider radius gradually over the specified duration
        float currentColliderRadius = Mathf.Lerp(0f, maxRadius, timeSinceStart / sizeIncreaseDuration);

        // Check for players within the poison cloud
        Collider[] colliders = Physics.OverlapSphere(transform.position, currentColliderRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player1") || collider.CompareTag("Player2"))
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

        Debug.Log("Applying damage to player: " + damage);

        AttributeManager attributes = player.gameObject.GetComponentInChildren<AttributeManager>();

        if (attributes != null)
        {
            attributes.TakeDamage(damage);
        }
    }

    private IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}

