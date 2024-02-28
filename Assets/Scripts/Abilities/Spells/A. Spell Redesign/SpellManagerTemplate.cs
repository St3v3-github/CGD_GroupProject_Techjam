using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Member;

public class SpellManagerTemplate : MonoBehaviour
{
    public SpellDataTemplate[] spellSlotArray = new SpellDataTemplate[4];

    private SpellDataTemplate spellDataTemplate;

    private void Start()
    {
        spellDataTemplate = FindObjectOfType<SpellDataTemplate>();

        for (int i = 0; i < spellSlotArray.Length; i++)
        {
            spellSlotArray[i] = Instantiate(spellSlotArray[i]);

            if (spellSlotArray[i].usesAdvProjSystem)
            {
                spellSlotArray[i].shooting = false;
                spellSlotArray[i].fireRateLimited = true;
                // NEATEN THIS LATER MAYBE IDK, IT ONLY RUNS ONCE PER PLAYER
                GetComponent<AdvancedProjectileSystem>().SetSpellDataTemplate(spellSlotArray[i]);
                spellSlotArray[i].firePoint = GameObject.FindWithTag("AdvProjSys_Firepoint").transform;
            }
        }

    }

    private void Update()
    {
        SpellStateManagement(spellSlotArray.Length);
    }


    private void HandleProjectileSpells(int slot)
    {
        playSound();

        //SET SHOOTING ON SPELLDATA TEMPLATE
        //SHOOTING LOGIC
        //-> READY TO SHOOT CHECK -- INVOKE RESETSHOT ETC
        //-> RAY OR PROJECTILE SHOOTING
        // 

        //Find exact Ray hit position using raycat
        /// COME BACK TO THIS WHEN COMPONENTREGISTRY IS MERGED
        ///Ray ray = GetComponentInParent<ComponentRegistry>().playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Ray ray = new Ray();
        RaycastHit hit;

        //Check if ray hits anything
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit)) {
            //ray hit Something
            targetPoint = hit.point;
            targetPoint = ray.GetPoint(100);
        }

        else {
            targetPoint = ray.GetPoint(100); // Point far away from player
        }

        //Calculate direcion to target
        Vector3 directionWithoutSpread = targetPoint - spellSlotArray[slot].firePoint.position;

        //Spread
        float x = Random.Range(-spellDataTemplate.spread, spellDataTemplate.spread);
        float y = Random.Range(-spellDataTemplate.spread, spellDataTemplate.spread);
        float z = Random.Range(-spellDataTemplate.spread, spellDataTemplate.spread);

        Vector3 directionWithSpread = directionWithoutSpread + (new Vector3(x, y, z) * Vector3.Magnitude(directionWithoutSpread)) / 15;
        //Instantiate Projectile
        GameObject currentProjectile = Instantiate(spellDataTemplate.Spellprefab, spellSlotArray[slot].firePoint.position, Quaternion.identity);
        currentProjectile.transform.forward = directionWithSpread.normalized;
        Projectile currentProjectileScript = currentProjectile.GetComponent<Projectile>();
        currentProjectileScript.source = this.gameObject;
        currentProjectileScript.damage = spellDataTemplate.damageValue;
        currentProjectileScript.setLifetime(spellDataTemplate.lifetime);

        //Add Forces to projctile
        Rigidbody rb = currentProjectile.GetComponent<Rigidbody>();
        rb.AddForce(directionWithSpread.normalized * spellDataTemplate.shootForce, ForceMode.Impulse);
        // For bouncing projectiles only    
        ///rb.AddForce(GetComponentInParent<ComponentRegistry>().playerCamera.transform.up * spellDataTemplate.upwardForce, ForceMode.Impulse);

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

   

    private void HandleThrowableSpells()
    {


    }

    private void HandleOneStageSpells()
    {



    }

    private void HandleTwoStageSpells()
    {


    }

    private void HandleContinuousSpells()
    {

    }


    private void HandleRaycastSpells()
    {

    }

    #region Spell State Machine

    private void SpellStateManagement(int slot)
    {
        spellSlotArray[slot].currentState = SpellDataTemplate.SpellState.READY;

        switch (spellSlotArray[slot].currentState)
        {
            case SpellDataTemplate.SpellState.READY:
                HandleSpellReady();
                break;

            case SpellDataTemplate.SpellState.ACTIVE:
                StartCoroutine(SpellActiveTimer(slot, spellDataTemplate.activeTime));
                break;

            case SpellDataTemplate.SpellState.COOLDOWN:
                StartCoroutine(SpellCooldownTimer(slot, spellDataTemplate.activeTime));
                break;

        }
    }

    private void HandleSpellReady()
    {
        if (spellDataTemplate.isReadyState)
        {
            Debug.Log("Spell Ready");

            //Put conditional for moving into active state Here

        }
    }

    private IEnumerator SpellActiveTimer(int slot, float activeTime)
    {
        yield return new WaitForSeconds(spellDataTemplate.activeTime);
        spellSlotArray[slot].currentState = SpellDataTemplate.SpellState.COOLDOWN;
    }

    private IEnumerator SpellCooldownTimer(int slot, float cooldown)
    {
        yield return new WaitForSeconds(spellDataTemplate.cooldownTime);
        spellSlotArray[slot].currentState = SpellDataTemplate.SpellState.READY;

    }

    #endregion


    public void Cast(int slotNumber)
    {

        switch (spellSlotArray[slotNumber].ID)
        {
            #region Fire
            case SpellDataTemplate.SpellID.FireProjectile:
                HandleProjectileSpells();
                break;

            case SpellDataTemplate.SpellID.FireGrenade:
                HandleThrowableSpells();
                break;

            case SpellDataTemplate.SpellID.FlameThrower:
                HandleContinuousSpells();
                break;

            #endregion


            #region Ice
            case SpellDataTemplate.SpellID.IceProjectile:
                HandleProjectileSpells();
                break;

            case SpellDataTemplate.SpellID.IceGrenade:
                HandleThrowableSpells();
                break;

            case SpellDataTemplate.SpellID.IceWall:
                HandleTwoStageSpells();
                break;

            #endregion


            #region Wind
            case SpellDataTemplate.SpellID.WindProjectile:
                HandleProjectileSpells();
                break;

            case SpellDataTemplate.SpellID.WhirlwindBouncePad:
                HandleTwoStageSpells();
                break;

            case SpellDataTemplate.SpellID.WindRushKnockback:
                HandleOneStageSpells();
                break;

            #endregion


            #region Lightning
            case SpellDataTemplate.SpellID.LightningProjectile:
                HandleProjectileSpells();
                break;

            case SpellDataTemplate.SpellID.LightningStrikeAOE:
                HandleTwoStageSpells();
                break;

            case SpellDataTemplate.SpellID.LightningChainRaycast:
                HandleRaycastSpells();
                break;

            #endregion


            #region Ultimates
            case SpellDataTemplate.SpellID.Beam:
                HandleContinuousSpells();
                break;

            case SpellDataTemplate.SpellID.BlackHole:
                HandleOneStageSpells();
                break;

            case SpellDataTemplate.SpellID.PoisonCloud:
                HandleRaycastSpells();
                break;

            case SpellDataTemplate.SpellID.Heal:
                HandleOneStageSpells();
                break;

                #endregion

        }
    }

    public void EquipUltimate(SpellDataTemplate newSlotData)
    {
        spellSlotArray[3] = newSlotData;
    }

    private void DealDamage()
    {

    }

    private void HandleSFX()
    {

    }


    //Function to equip ultimate

    //Set Active For function for ultimate

    //Set Cooldown function
}


