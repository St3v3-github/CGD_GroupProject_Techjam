using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
    {
        public GameObject projectileHolographicPrefab;
        public GameObject projectilePrefab;
        private GameObject projectileHolographic;
        private GameObject chargedProjectile;
        private bool isCharging;
        private float chargeStartTime;
        private float chargeDuration = 2.0f;
        private float releaseDuration = 1.0f;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.X))
            {
                if (!isCharging)
                {
                    // Start charging the attack
                    StartChargingAttack();
                }
            }

            if (Input.GetKeyUp(KeyCode.X) && isCharging)
            {
                // Release the attack
                ReleaseAttack();
            }

            if (isCharging)
            {
                // Charge the attack
                ChargeAttack();
            }
        }

        void StartChargingAttack()
        {
            // Spawn the holographic in front of the player
            // Determine position based on camera direction

            isCharging = true;
            chargeStartTime = Time.time;
            // Instantiate the holographic attack
            projectileHolographic = Instantiate(projectileHolographicPrefab, transform.position + transform.forward * 5.0f, Quaternion.identity);
        }

        void ChargeAttack()
        {
            float chargeTime = Time.time - chargeStartTime;
            if (chargeTime >= chargeDuration)
            {
                // The player has fully charged the attack
                isCharging = false;
                // Continue with attack or despawn the holographic
                Destroy(projectileHolographic);
            }
        }

        void ReleaseAttack()
        {
            float releaseTime = Time.time - chargeStartTime;
            if (releaseTime <= releaseDuration)
            {
                // Fire the attack
                FireAttack();
            }
            else
            {
                // Cancel the charge
                isCharging = false;
                Destroy(projectileHolographic);
            }
        }

        void FireAttack()
        {
            // Instantiate the actual projectile based on the charged attack
            chargedProjectile = Instantiate(projectilePrefab, projectileHolographic.transform.position, Quaternion.identity);
            // Set the projectile's velocity to move in the direction the player is facing
            Rigidbody rb = chargedProjectile.GetComponent<Rigidbody>();
            rb.velocity = transform.forward * 10.0f; // Adjust speed as needed

            // Schedule the projectile to despawn after 10 seconds (or collision)
            StartCoroutine(DespawnAfterTime(chargedProjectile, 10.0f));
        }

        IEnumerator DespawnAfterTime(GameObject obj, float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(obj);
        }
    }


