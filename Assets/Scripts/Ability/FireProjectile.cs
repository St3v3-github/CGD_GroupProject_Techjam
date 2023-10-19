using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    private Vector3 verticalOffset = new Vector3(0f, 0.5f, 0f);
    private Vector3 horizontalOffset = new Vector3(1f, 0f, 1f);
    public float projectileForce = 100f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Fire();
        }
    }

    void Fire()
    {
        //Vector3 projectileSpawnPoint = firePoint.position;
        //projectileSpawnPoint.x += (firePoint.forward * 0.5f).x;
        //projectileSpawnPoint.z += (firePoint.forward * 0.5f).z;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position - verticalOffset + Vector3.Scale(horizontalOffset, firePoint.forward), firePoint.rotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(firePoint.forward * projectileForce, ForceMode.Impulse);
            Debug.Log("used");
        }
    }
}
