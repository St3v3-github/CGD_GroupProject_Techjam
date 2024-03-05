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
    public ComponentRegistry componentRegistry;

    //private SpellDataTemplate spellDataTemplate;

    private void Start()
    {
        componentRegistry = GetComponentInParent<ComponentRegistry>();
       

        for (int i = 0; i < spellSlotArray.Length; i++)
        {
            spellSlotArray[i] = Instantiate(spellSlotArray[i]);
            spellSlotArray[i].targetPoint = GameObject.FindWithTag("AdvProjSys_Firepoint").transform;
            if (spellSlotArray[i].usesAdvProjSystem)
            {
                spellSlotArray[i].shooting = false;
                spellSlotArray[i].fireRateLimited = true;
                // NEATEN THIS LATER MAYBE IDK, IT ONLY RUNS ONCE PER PLAYER
               // GetComponent<AdvancedProjectileSystem>().SetSpellDataTemplate(spellSlotArray[i]);
               
            }
        }

    }

    private void Update()
    {
        for(int i = 0; i < spellSlotArray.Length; i++)
        SpellStateManagement(i);
    }


    private void HandleProjectileSpells(int slot)
    {
        //playSound();

       
        //SET SHOOTING ON SPELLDATA TEMPLATE
        //SHOOTING LOGIC
        //-> READY TO SHOOT CHECK -- INVOKE RESETSHOT ETC
        //-> RAY OR PROJECTILE SHOOTING
        // 

        if(spellSlotArray[slot].currentState == SpellDataTemplate.SpellState.READY)
        {
            spellSlotArray[slot].currentState = SpellDataTemplate.SpellState.COOLDOWN;
            //Find exact Ray hit position using raycat
            Ray ray = componentRegistry.playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            //Check if ray hits anything
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
            Vector3 directionWithoutSpread = targetPoint - spellSlotArray[slot].targetPoint.position;

            //Spread
            float x = Random.Range(-spellSlotArray[slot].spread, spellSlotArray[slot].spread);
            float y = Random.Range(-spellSlotArray[slot].spread, spellSlotArray[slot].spread);
            float z = Random.Range(-spellSlotArray[slot].spread, spellSlotArray[slot].spread);

            Vector3 directionWithSpread = directionWithoutSpread + (new Vector3(x, y, z) * Vector3.Magnitude(directionWithoutSpread)) / 15;

            //Instantiate Projectile
            GameObject currentProjectile = Instantiate(spellSlotArray[slot].Spellprefab, spellSlotArray[slot].targetPoint.position, Quaternion.identity);
            currentProjectile.transform.forward = directionWithSpread.normalized;
            Projectile currentProjectileScript = currentProjectile.GetComponent<Projectile>();
            currentProjectileScript.source = this.gameObject;   //Change when player prefab fixed
            currentProjectileScript.damage = spellSlotArray[slot].damageValue;
            currentProjectileScript.setLifetime(spellSlotArray[slot].lifetime);

            //Add Forces to projctile
            Rigidbody rb = currentProjectile.GetComponent<Rigidbody>();
            rb.AddForce(directionWithSpread.normalized * spellSlotArray[slot].shootForce, ForceMode.Impulse);
            // For bouncing projectiles only    
            rb.AddForce(componentRegistry.playerCamera.transform.up * spellSlotArray[slot].upwardForce, ForceMode.Impulse);

            #region camera shake
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
            #endregion

            //Enters Cooldown once shot
            spellSlotArray[slot].currentState = SpellDataTemplate.SpellState.COOLDOWN;
        }
    }



    private void HandleThrowableSpells(int slot)
    {
        if (spellSlotArray[slot].currentState == SpellDataTemplate.SpellState.READY)
        {
            spellSlotArray[slot].currentState = SpellDataTemplate.SpellState.COOLDOWN;
            Debug.Log("Throw");
            GameObject projectile = Instantiate(spellSlotArray[slot].Spellprefab, spellSlotArray[slot].targetPoint.position, componentRegistry.playerCamera.transform.rotation);
            projectile.GetComponent<Grenade>().source = this.gameObject;
            projectile.tag = this.transform.parent.tag + "Spell";
            //AudioManager.instance.PlayOneShot(FMODEvents.instance.iceSound, this.transform.position);

            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(componentRegistry.playerCamera.transform.forward * spellSlotArray[slot].shootForce, ForceMode.Impulse);

            }

        }



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
        //spellSlotArray[slot].currentState = SpellDataTemplate.SpellState.READY;

        Debug.Log("SLOT IS  " + slot);
        switch (spellSlotArray[slot].currentState)
        {
            case SpellDataTemplate.SpellState.READY:
                break;

            case SpellDataTemplate.SpellState.ACTIVE:

                StartCoroutine(SpellActiveTimer(slot, spellSlotArray[slot].activeTime));
                break;

            case SpellDataTemplate.SpellState.COOLDOWN:
                if (!spellSlotArray[slot].changingState)
                {
                    spellSlotArray[slot].changingState = true;
                    StartCoroutine(SpellCooldownTimer(slot, spellSlotArray[slot].cooldownTime));
                }
                
                break;

        }
    }

    private IEnumerator SpellActiveTimer(int slot, float activeTime)
    {
        yield return new WaitForSeconds(spellSlotArray[slot].activeTime);
        spellSlotArray[slot].currentState = SpellDataTemplate.SpellState.COOLDOWN;
    }

    private IEnumerator SpellCooldownTimer(int slot, float cooldown)
    {
        yield return new WaitForSeconds(spellSlotArray[slot].cooldownTime);
        spellSlotArray[slot].currentState = SpellDataTemplate.SpellState.READY;
        spellSlotArray[slot].changingState = false;

    }

    #endregion


    public void Cast(int slotNumber)
    {

        switch (spellSlotArray[slotNumber].ID)
        {
            #region Fire
            case SpellDataTemplate.SpellID.FireProjectile:
                HandleProjectileSpells(slotNumber);
                break;

            case SpellDataTemplate.SpellID.FireGrenade:
                HandleThrowableSpells(slotNumber);
                break;

            case SpellDataTemplate.SpellID.FlameThrower:
                HandleContinuousSpells();
                break;

            #endregion


            #region Ice
            case SpellDataTemplate.SpellID.IceProjectile:
                HandleProjectileSpells(slotNumber);
                break;

            case SpellDataTemplate.SpellID.IceGrenade:
                HandleThrowableSpells(slotNumber);
                break;

            case SpellDataTemplate.SpellID.IceWall:
                HandleTwoStageSpells();
                break;

            #endregion


            #region Wind
            case SpellDataTemplate.SpellID.WindProjectile:
                HandleProjectileSpells(slotNumber);
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
                HandleProjectileSpells(slotNumber);
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


