using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AdvancedProjectileSystem : Spell
{
    [Header("Spell Data")]
    private SpellDataTemplate spellDataTemplate;
    private List<SpellDataTemplate> spells = new List<SpellDataTemplate>();
    private int currentSpellIndex = 0;

    //Reference
    public Camera playerCam;
    public Transform firePoint;
    public RaycastHit rayHit;
    public LayerMask hittable;
    public AnimationManager animControl;

    //Graphic (Coming soon)
    //public GameObject spellFlash, spellImpact;
    //public CamShake camShake;
    //public float camShakeMagnitude, cameShakeDuration;
    //public Text text;*/

    public void SetSpellDataTemplate(SpellDataTemplate newTemplate)
    {
        spellDataTemplate = newTemplate;
    }

    private void Update()
    {
        if(spellDataTemplate != null)
        {
            MyInput();
        }
        

        //UI
        //text.SetText(chargesLeft / projectile.projectilesPerTap + " / " projectile.totalCharges / projectile.projectilesPerTap);
    }


    public void MyInput()
    {
        //If hold to fire
        if (spellDataTemplate.allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        //If 1 shot per click
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);


        // Additional check for security
        if (Input.GetKeyUp(KeyCode.Mouse0)) shooting = false;



        //Fire
        if (readyToShoot && shooting)
        {
            animControl.toggleCastingBool(true);
            if (shootWithRay)
            {
                RayShoot();
            }
            else
            {
                ProjectileShoot();
            }
        }
    }
    public void ToggleShooting()
    {
        shooting = !shooting;
    }

   


    public void ProjectileShoot()
    {
        readyToShoot = false;
       // Debug.Log("Shoot called");

        //Find exact hit position using raycat
        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f,0.5f, 0));
        RaycastHit hit;

        //check if ray hits somehting
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            //ray hit Something
            targetPoint = hit.point;
            targetPoint = ray.GetPoint(100);
        }
        else
        {
            targetPoint = ray.GetPoint(100); // Point far away from player
        }


        //Calculate direcion to target

        Vector3 directionWithoutSpread = targetPoint - firePoint.position;

        //Spread
        float x = Random.Range(-spellDataTemplate.spread, spellDataTemplate.spread);
        float y = Random.Range(-spellDataTemplate.spread, spellDataTemplate.spread);
        float z = Random.Range(-spellDataTemplate.spread, spellDataTemplate.spread);

        Vector3 directionWithSpread = directionWithoutSpread + (new Vector3(x, y, z) * Vector3.Magnitude(directionWithoutSpread)) / 15;
        //Instantiate Projectile
        GameObject currentProjectile = Instantiate(spellDataTemplate.Spellprefab, firePoint.position, Quaternion.identity);
        currentProjectile.transform.forward = directionWithSpread.normalized;
        Projectile currentProjectileScript = currentProjectile.GetComponent<Projectile>();
        currentProjectileScript.source = source;
        currentProjectileScript.damage = spellDataTemplate.damageValue;
        currentProjectileScript.setLifetime(spellDataTemplate.lifetime);

        playSound();

        //Add Forces to projctile
        Rigidbody rb = currentProjectile.GetComponent<Rigidbody>();
        rb.AddForce(directionWithSpread.normalized * spellDataTemplate.shootForce, ForceMode.Impulse);
        // For bouncing projectiles only    
        rb.AddForce(playerCam.transform.up * spellDataTemplate.upwardForce, ForceMode.Impulse);

        //ShakeCamera
        //camShake.shake(camShakeDuration, camShakeMagnitude);

        //Graphics
        //Instantiate(spellImpact, rayHit.point, Quaternion.LookRotation(rayHit.normal));
        //Instantiate(spellFlash, firePoint.position, Quaternion.identity);

        //Invoke resetShot ( if not already invoked)
/*        if (fireRateLimited)
        {
            Invoke("ResetShot", spellDataTemplate.timeBetweenShots);
            fireRateLimited = false;
        }*/


        // if multishot projectile, repeat function (for burst or shotgun)
/*        if (chargesShot < equippedProjectile.projectilesPerTap && chargesLeft > 0)
        {
            Invoke("ProjectileShoot", equippedProjectile.burstDelay);
        }*/
        
    }


    private void RayShoot()
    {
        readyToShoot = false;

        //Spread
        float x = Random.Range(-spellDataTemplate.spread, spellDataTemplate.spread);
        float y = Random.Range(-spellDataTemplate.spread, spellDataTemplate.spread);

        Vector3 direction = playerCam.transform.forward + new Vector3(x, y, 0);
        List<RaycastHit> hits = new List<RaycastHit>();
        //Raycast
        if (Physics.Raycast(playerCam.transform.position, direction, out rayHit, spellDataTemplate.range, hittable))
        {
            Debug.Log(rayHit.collider.gameObject.name);
           // dealDamage(rayHit.collider.gameObject, equippedProjectile.damage);
        }

        //ShakeCamera
        //camShake.shake(camShakeDuration, camShakeMagnitude);

        //Graphics
        //Instantiate(spellImpact, rayHit.point, Quaternion.LookRotation(rayHit.normal));
        //Instantiate(spellFlash, firePoint.position, Quaternion.identity);

        //Invoke resetShot ( if not already invoked)
/*        Invoke("ResetShot", spellDataTemplate.timeBetweenShots);*/

/*        if (chargesShot < equippedProjectile.totalCharges && chargesLeft > 0)
        {
            Invoke("RayShoot", equippedProjectile.burstDelay);
        }*/
    }

    private void ResetShot()
/*    {
        fireRateLimited = true;
        readyToShoot = true;
        if (!spellDataTemplate.allowButtonHold)
        {

            shooting = false;
        }
        // Debug.Log("Ready to shoot");
        animControl.toggleCastingBool(false);
    }*/

    private void playSound()
    {
        /*switch(equippedProjectile.Type)
        {
            case spellEnum.fire:
                AudioManager.instance.PlayOneShot(FMODEvents.instance.stagSound, this.transform.position);
                break;
            case spellEnum.ice:
                AudioManager.instance.PlayOneShot(FMODEvents.instance.iceSound, this.transform.position);
                break;
            case spellEnum.lightning:
                AudioManager.instance.PlayOneShot(FMODEvents.instance.zapSound, this.transform.position);
                break;
            case spellEnum.wind:
                AudioManager.instance.PlayOneShot(FMODEvents.instance.windSound, this.transform.position);
                break;
        }*/
    }
}
