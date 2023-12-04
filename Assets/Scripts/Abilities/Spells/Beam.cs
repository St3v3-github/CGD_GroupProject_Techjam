using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Beam : Spell
{
    public float damagePerTick = 5f;
    public float tickInterval = 0.2f;
    public float beamRange = 50f;
    public float abilityDuration = 3f;  // Duration of the beam ability in seconds
    public Transform playerCam;
    public Transform firePoint;
    public LayerMask targetLayer;
    public GameObject particlePrefab;
    private GameObject beam;
    public bool active = false;


    private float lastTickTime;
    private float abilityEndTime;

    void Start()
    {
        setTargetTag();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && !active)
        {

            cast();

        }

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
                    dealDamage(hit.transform.gameObject, damagePerTick);

                    // Update the last tick time
                    lastTickTime = Time.time;
                }
            }
        }
        else
        {
            // Disable the beam ability when the duration is over
            active = false;
            Destroy(beam);
        }
    }

   public void cast()
    {
        Vector3 spawnPosition = playerCam.position;
        spawnPosition.y -= 0.4f;
        active = true;
        abilityEndTime = Time.time + abilityDuration; // Set the end time of the ability
        beam = Instantiate(particlePrefab, spawnPosition + (playerCam.forward), playerCam.rotation);
        beam.transform.SetParent(playerCam.transform, true);
    }
}

