using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PoisonCloud : Spell
{
    public float damagePerSecond = 5f; // Adjust the damage value
    public float delayBeforeDamage = 2f; // Adjust the delay before damage starts
    public float maxColliderRadius = 5f; // Adjust the maximum collider radius
    public float sizeIncreaseDuration = 5f; // Adjust the duration over which the collider size increases

    private bool isInPoisonCloud = false;
    private float timeInPoisonCloud = 0f;
    private float timeSinceStart = 0f;
    private GameObject playerInPoisonCloud; // Store the reference to the player
    private SphereCollider poisonCollider; // Reference to the sphere collider

    private void Start()
    {
        // Get the reference to the SphereCollider component
        poisonCollider = GetComponent<SphereCollider>();
        StartCoroutine(timerCoroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            // Player entered the poison cloud
            Debug.Log("Player entered the poison cloud");
            isInPoisonCloud = true;
            playerInPoisonCloud = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.layer == LayerMask.NameToLayer("layer_player"))
        {
            // Player exited the poison cloud
            Debug.Log("Player exited the poison cloud");
            isInPoisonCloud = false;
            timeInPoisonCloud = 0f; // Reset the timer
            playerInPoisonCloud = null;
        }
    }

    private void Update()
    {
        timeSinceStart += Time.deltaTime;

        // Increase the collider radius gradually over the specified duration
        float currentColliderRadius = Mathf.Lerp(0f, maxColliderRadius, timeSinceStart / sizeIncreaseDuration);

        // Update the sphere collider's radius
        poisonCollider.radius = currentColliderRadius;

        if (isInPoisonCloud)
        {
            // Increment the timer while the player is in the poison cloud
            timeInPoisonCloud += Time.deltaTime;

            // Check if the delay period has passed
            if (timeInPoisonCloud > delayBeforeDamage && playerInPoisonCloud != null)
            {
                // Apply damage over time
                ApplyDamageOverTime(playerInPoisonCloud);
            }
        }
    }


    private void ApplyDamageOverTime(GameObject player)
    {
        // Calculate damage based on time
        float damage = damagePerSecond * Time.deltaTime;

        Debug.Log("Applying damage to player: " + damage);

        AttributeManager attributes = player.gameObject.GetComponent<AttributeManager>();

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


