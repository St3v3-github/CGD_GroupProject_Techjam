using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AdvancedProjectileSystem : Spell
{
    public ProjectileData projectile;
    int chargesLeft, chargesShot;

    //bools
    bool shooting, readyToShoot, recharging;

    //Testing :)
    public bool allowInvoke = true;

    //Reference
    public Camera playerCam;
    public Transform firePoint;
    public RaycastHit rayHit;
    public LayerMask hittable;

    //Graphic (Coming soon)
    //public GameObject spellFlash, spellImpact;
    //public CamShake camShake;
    //public float camShakeMagnitude, cameShakeDuration;
    //public Text text;



    private void Start()
    {
        setTargetTag();

    }

    private void Awake()
    {
        chargesLeft = projectile.totalCharges;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        //UI
        //text.SetText(chargesLeft / projectile.projectilesPerTap + " / " projectile.totalCharges / projectile.projectilesPerTap);
    }
    private void MyInput()
    {
        //If hold to fire
        if (projectile.allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        //If 1 shot per click
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);
        // Additional check for security
        if (Input.GetKeyUp(KeyCode.Mouse0)) shooting = false;


        //Recharging
        if (Input.GetKeyDown(KeyCode.R) && chargesLeft < projectile.totalCharges && !recharging) Recharge();
        //Automatic Recharging
        if (readyToShoot && shooting && !recharging && chargesLeft <= 0) Recharge();

        //Fire
        if (readyToShoot && shooting && !recharging && chargesLeft > 0)
        {
            chargesShot = 0;
            ProjectileShoot();

        }
    }

    private void ProjectileShoot()
    {
        readyToShoot = false;

        //Find exact hit position using raycat
        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f,0.5f, 0));
        RaycastHit hit;

        //check if ray hits somehting
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            //ray hit Something
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100); // Point far away from player
        }


        //Calculate direcion to target

        Vector3 directionWithoutSpread = targetPoint - firePoint.position;


        //Spread
        float x = Random.Range(-projectile.spread, projectile.spread);
        float y = Random.Range(-projectile.spread, projectile.spread);
        float z = Random.Range(-projectile.spread, projectile.spread);
        Debug.Log(Vector3.Magnitude(directionWithoutSpread));
        Vector3 directionWithSpread = directionWithoutSpread + (new Vector3(x, y, z) * Vector3.Magnitude(directionWithoutSpread)) / 15;

        //Instantiate Projectile
        GameObject currentProjectile = Instantiate(projectile.projectile, firePoint.position, Quaternion.identity);
        currentProjectile.transform.forward = directionWithSpread.normalized;

        //Add Forces to projctile
        currentProjectile.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * projectile.shootForce, ForceMode.Impulse);
        // For bouncing projectiles only
        currentProjectile.GetComponent<Rigidbody>().AddForce(playerCam.transform.up * projectile.upwardForce, ForceMode.Impulse);

        chargesLeft--;
        chargesShot++;

        //ShakeCamera
        //camShake.shake(camShakeDuration, camShakeMagnitude);

        //Graphics
        //Instantiate(spellImpact, rayHit.point, Quaternion.LookRotation(rayHit.normal));
        //Instantiate(spellFlash, firePoint.position, Quaternion.identity);

        //Invoke resetShot ( if not already invoked)
        if (allowInvoke)
        {
            Invoke("ResetShot", projectile.timeBetweenShots);
            allowInvoke = false;
        }
        

        // if multishot projectile, repeat function (for burst or shotgun)
        if (chargesShot < projectile.projectilesPerTap && chargesLeft > 0)
        {
            Invoke("ProjectileShoot", projectile.burstDelay);
        }
    }


    private void RayShoot()
    {
        readyToShoot = false;
        chargesLeft--;
        chargesShot++;

        //Spread
        float x = Random.Range(-projectile.spread, projectile.spread);
        float y = Random.Range(-projectile.spread, projectile.spread);

        Vector3 direction = playerCam.transform.forward + new Vector3(x, y, 0);

        //Raycast
        if (Physics.Raycast(playerCam.transform.position, direction, out rayHit, projectile.range, hittable))
        {
            Debug.Log(rayHit.collider.name);
            if (rayHit.collider.CompareTag(targetTag))
            {
                dealDamage(rayHit.collider.gameObject, projectile.damage);
            }
        }

        //ShakeCamera
        //camShake.shake(camShakeDuration, camShakeMagnitude);

        //Graphics
        //Instantiate(spellImpact, rayHit.point, Quaternion.LookRotation(rayHit.normal));
        //Instantiate(spellFlash, firePoint.position, Quaternion.identity);

        //Invoke resetShot ( if not already invoked)
        if (allowInvoke)
        {
            Invoke("ResetShot", projectile.timeBetweenShots);
            allowInvoke = false;
        }

        if (chargesShot < projectile.totalCharges && chargesLeft > 0)
        {
            Invoke("RayShoot", projectile.burstDelay);
        }
    }

    private void ResetShot()
    {
        allowInvoke = true;
        readyToShoot = true;
        Debug.Log("Ready to shoot");
    }

    private void Recharge()
    {
        recharging = true;
        Invoke("RechargeFinished", projectile.rechargeTime);
    }

    private void RechargeFinished()
    {
        chargesLeft = projectile.totalCharges;
        recharging = false;
    }
}
