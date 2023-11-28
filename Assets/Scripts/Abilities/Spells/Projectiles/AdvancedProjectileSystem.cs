using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AdvancedProjectileSystem : Spell
{
    public ProjectileData spell;
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
        chargesLeft = spell.totalCharges;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        //UI
        //text.SetText(chargesLeft / spell.projectilesPerTap + " / " spell.totalCharges / spell.projectilesPerTap);
    }
    private void MyInput()
    {
        //If hold to fire
        if (spell.allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        //If 1 shot per click
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);
        // Additional check for security
        if (Input.GetKeyUp(KeyCode.Mouse0)) shooting = false;


        //Recharging
        if (Input.GetKeyDown(KeyCode.R) && chargesLeft < spell.totalCharges && !recharging) Recharge();
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

        Vector3 directonWihoutSpread = targetPoint - firePoint.position;


        //Spread
        float x = Random.Range(-spell.spread, spell.spread);
        float y = Random.Range(-spell.spread, spell.spread);

        Vector3 directonWithSpread = directonWihoutSpread + new Vector3(x, y, 0);

        //Instantiate Projectile
        GameObject currentProjectile = Instantiate(spell.projectile, firePoint.position, Quaternion.identity);
        currentProjectile.transform.forward = directonWithSpread.normalized;

        //Add Forces to projctile
        currentProjectile.GetComponent<Rigidbody>().AddForce(directonWithSpread.normalized * spell.shootForce, ForceMode.Impulse);
        // For bouncing projectiles only
        currentProjectile.GetComponent<Rigidbody>().AddForce(playerCam.transform.up * spell.upwardForce, ForceMode.Impulse);

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
            Invoke("ResetShot", spell.timeBetweenShots);
            allowInvoke = false;
        }
        

        // if multishot projectile, repeat function (for burst or shotgun)
        if (chargesShot < spell.projectilesPerTap && chargesLeft > 0)
        {
            Invoke("ProjectileShoot", spell.burstDelay);
        }
    }


    private void RayShoot()
    {
        readyToShoot = false;
        chargesLeft--;
        chargesShot++;

        //Spread
        float x = Random.Range(-spell.spread, spell.spread);
        float y = Random.Range(-spell.spread, spell.spread);

        Vector3 direction = playerCam.transform.forward + new Vector3(x, y, 0);

        //Raycast
        if (Physics.Raycast(playerCam.transform.position, direction, out rayHit, spell.range, hittable))
        {
            Debug.Log(rayHit.collider.name);
            if (rayHit.collider.CompareTag(targetTag))
            {
                AttributeManager attributes = rayHit.collider.gameObject.GetComponent<AttributeManager>();

                if (attributes != null)
                {
                    attributes.TakeDamage(damage);
                }
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
            Invoke("ResetShot", spell.timeBetweenShots);
            allowInvoke = false;
        }

        if (chargesShot < spell.totalCharges && chargesLeft > 0)
        {
            Invoke("RayShoot", spell.burstDelay);
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
        Invoke("RechargeFinished", spell.rechargeTime);
    }

    private void RechargeFinished()
    {
        chargesLeft = spell.totalCharges;
        recharging = false;
    }
}
