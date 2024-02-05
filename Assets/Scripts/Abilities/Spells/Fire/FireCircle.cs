using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCircle : Spell
{
    public float damagePerSecond = 5f; // Adjust the damage value
    public float delayBeforeDamage = 0f; // Adjust the delay before damage starts
    public float Radius = 2.5f; // Adjust the maximum collider radius
    public float sizeIncreaseDuration = 5f; // Adjust the duration over which the collider size increases

    private bool isInPoisonCloud = false;
    private float timeInPoisonCloud = 0f;
    private float timeSinceStart = 0f;
    private List<GameObject> playersInPoisonCloud; // Store the reference to the player

    private void Start()
    {
        StartCoroutine(timerCoroutine());
    }

    private void Update()
    {
        timeSinceStart += Time.deltaTime;

        // Use Physics.OverlapSphere to find colliders within the radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, Radius);

        // Check for players in the poison cloud
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player1") || collider.CompareTag("Player2"))
            {
                // Player entered the poison cloud
                Debug.Log("Player entered the poison cloud");
                isInPoisonCloud = true;
                playersInPoisonCloud.Add(collider.gameObject);
                break;
            }
        }

        if (isInPoisonCloud)
        {
            // Increment the timer while the player is in the poison cloud
            timeInPoisonCloud += Time.deltaTime;
            foreach (GameObject player in playersInPoisonCloud)
            {
                // Check if the delay period has passed
                if (timeInPoisonCloud > delayBeforeDamage && player != null)
                {
                    // Apply damage over time
                    ApplyDamageOverTime(player);
                }
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

    private IEnumerator timerCoroutine()
    {
        yield return new WaitForSeconds(10f);
        Destroy(transform.gameObject);
    }
}
