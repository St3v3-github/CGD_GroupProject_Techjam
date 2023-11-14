using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class FireProjectile : ElementalSpell
{
    public Transform firePoint;
    private Vector3 verticalOffset = new Vector3(0f, 0.5f, 0f);
    private float projectileForce = 100f;
    public float fireForce = 80f;
    public float iceForce = 40f;
    public float windForce = 100f;
    public float lightForce = 120f;

    private void Start()
    {
        setStatus();
        setPrefab(currentStatus);
    }

    public override void setPrefab(StatusEffect status)
    {

        switch (status.GetStatusType())
        {
            case "fire":
                spellPrefab = firePrefab;
                damage = fireDamage;
                projectileForce = fireForce;
                break;
            case "ice":
                spellPrefab = icePrefab;
                damage = iceDamage;
                projectileForce = iceForce;
                break;
            case "lightning":
                spellPrefab = lightningPrefab;
                damage = lightDamage;
                projectileForce = lightForce;
                break;
            case "wind":
                spellPrefab = windPrefab;
                damage = windDamage;
                projectileForce = windForce;
                break;
            default:
                spellPrefab = firePrefab;
                damage = fireDamage;
                projectileForce = fireForce;
                break;
        }
    }

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
        GameObject projectile = Instantiate(spellPrefab, firePoint.position - verticalOffset + firePoint.forward, firePoint.rotation);
        projectile.tag = this.tag + "Spell";

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(firePoint.forward * projectileForce, ForceMode.Impulse);
            
        }
    }
}
