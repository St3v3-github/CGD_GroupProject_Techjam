using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class playerInArea
{
    public GameObject player;
    public float timeInArea;
}

public class PoisonCloud : Spell
{
    public float damagePerSecond = 5f; // Adjust the damage value
    public float delayBeforeDamage = 2f; // Adjust the delay before damage starts
    public float maxRadius = 5f; // Adjust the maximum collider radius
    public float sizeIncreaseDuration = 5f; // Adjust the duration over which the collider size increases

    private bool isInPoisonCloud = false;
    private float timeInPoisonCloud = 0f;
    private float timeSinceStart = 0f;
    private List<playerInArea> playersInPoisonCloud; // Store the reference to the player

    private void Start()
    {
        StartCoroutine(timerCoroutine());
    }

    private void Update()
    {
        timeSinceStart += Time.deltaTime;

        // Increase the collider radius gradually over the specified duration
        float currentRadius = Mathf.Lerp(0f, maxRadius, timeSinceStart / sizeIncreaseDuration);

        // Use Physics.OverlapSphere to find colliders within the radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, currentRadius);

        // Check for players in the poison cloud
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player1") || collider.CompareTag("Player2"))
            {
                // Player entered the poison cloud
                Debug.Log("Player entered the poison cloud");
                playerInArea pia = new playerInArea();
                pia.player = collider.gameObject;
                pia.timeInArea = 0;
                playersInPoisonCloud.Add(pia);
                break;
            }
        }

        if (playersInPoisonCloud.Count != 0)
        {
            // Increment the timer while the player is in the poison cloud
            
            foreach (playerInArea pia in playersInPoisonCloud)
            {
                pia.timeInArea += Time.deltaTime;
                // Check if the delay period has passed
                if (pia.timeInArea > delayBeforeDamage && pia.player != null)
                {
                    // Apply damage over time
                    ApplyDamageOverTime(pia.player);
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


