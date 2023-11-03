using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public float damagePerTick = 5f;
    public float tickInterval = 0.2f;
    public float beamRange = 50f;
    public float abilityDuration = 5f;  // Duration of the beam ability in seconds
    public LayerMask targetLayer;

    private float lastTickTime;
    private float abilityEndTime;

    void Start()
    {
        abilityEndTime = Time.time + abilityDuration; // Set the end time of the ability
    }

    void Update()
    {
        // Check if the ability is still active based on the duration
        if (Time.time < abilityEndTime)
        {
            // Cast a ray from the transform position forward
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            // Check if the ray hits an opponent within the specified range
            if (Physics.Raycast(ray, out hit, beamRange, targetLayer))
            {
                // Check if enough time has passed to deal damage again
                if (Time.time - lastTickTime >= tickInterval)
                {
                    // Deal damage to the opponent
                    

                    // Update the last tick time
                    lastTickTime = Time.time;
                }
            }
        }
    }
}
