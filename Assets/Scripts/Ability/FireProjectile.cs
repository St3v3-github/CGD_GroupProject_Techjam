using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileForce = 100f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
    }

    void Fire()
    {
        //Vector3 projectileSpawnPoint = firePoint.position;
        //projectileSpawnPoint.x += (firePoint.forward * 0.5f).x;
        //projectileSpawnPoint.z += (firePoint.forward * 0.5f).z;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(firePoint.forward * projectileForce, ForceMode.Impulse);
            Debug.Log("used");
        }
    }
}
