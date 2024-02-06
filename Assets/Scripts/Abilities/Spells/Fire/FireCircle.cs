using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCircle : Spell
{
    public float damagePerSecond = 5f; // Adjust the damage value
    public float delayBeforeDamage = 0f; // Adjust the delay before damage starts
    public float maxRadius = 2.5f; // Adjust the maximum collider radius
    public float sizeIncreaseDuration = 0.5f; // Adjust the duration over which the collider size increases
    public ParticleSystem particle;

    private bool isInFire = false;
    private float timeInFire = 0f;
    private float timeSinceStart = 0f;
    private List<GameObject> playersInFire; // Store the reference to the player
    

    private void Start()
    {
        StartCoroutine(timerCoroutine());
    }

    private void Update()
    {
        timeSinceStart += Time.deltaTime;

        float currentRadius = Mathf.Lerp(0f, maxRadius, timeSinceStart / sizeIncreaseDuration);

        ParticleSystem.ShapeModule ps = particle.shape;
        ps.radius = currentRadius;

        // Use Physics.OverlapSphere to find colliders within the radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, currentRadius);

        // Check for players in the poison cloud
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player1") || collider.CompareTag("Player2"))
            {
                // Player entered the poison cloud
                Debug.Log("Player entered the poison cloud");
                isInFire = true;
                playersInFire.Add(collider.gameObject);
                break;
            }
        }

        if (isInFire)
        {
            // Increment the timer while the player is in the poison cloud
            timeInFire += Time.deltaTime;
            foreach (GameObject player in playersInFire)
            {
                // Check if the delay period has passed
                if (timeInFire > delayBeforeDamage && player != null)
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
