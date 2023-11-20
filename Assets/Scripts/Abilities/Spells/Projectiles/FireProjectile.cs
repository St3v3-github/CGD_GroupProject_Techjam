using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    private Vector3 verticalOffset = new Vector3(0f, 0.5f, 0f);
    public float projectileForce = 100f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Fire();
        }
    }

    public void Fire()
    {
        //Vector3 projectileSpawnPoint = firePoint.position;
        //projectileSpawnPoint.x += (firePoint.forward * 0.5f).x;
        //projectileSpawnPoint.z += (firePoint.forward * 0.5f).z;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position - verticalOffset + firePoint.forward, firePoint.rotation);
        projectile.tag = this.tag + "Spell";
        AudioManager.instance.PlayOneShot(FMODEvents.instance.ProjectileSummoned, this.transform.position);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(firePoint.forward * projectileForce, ForceMode.Impulse);
            
        }
        
    }
}
