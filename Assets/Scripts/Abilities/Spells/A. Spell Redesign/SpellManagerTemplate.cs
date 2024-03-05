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

    //For the AOE projection in the scene
    private GameObject projection;
    public bool projectionOn = false;


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

        //Updating AoE Projection Position
        if (projectionOn)
        {
            UpdateProjection();
        }
    }

    #region Two Stage Spell Logic
    void UpdateProjection()
    {
        // Get the camera's position and forward direction
        Vector3 cameraPosition = componentRegistry.playerCamera.transform.position;
        Vector3 cameraForward = componentRegistry.playerCamera.transform.forward;

        // Create a ray from the camera's position in the forward direction
        Ray ray = new Ray(cameraPosition, cameraForward);
        RaycastHit hit;

        // Check if the ray hits something on the specified layer
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 targetPosition = hit.point;
            // Ensure the object stays on the ground by setting its y-coordinate to the hit point's y-coordinate
            targetPosition.y += 0.2f;

            // Set the object's position to the hit point
            projection.transform.position = targetPosition;
        }
    }

    public void switchProjectionOn(int slot)
    {
        spellSlotArray[slot].currentState = SpellDataTemplate.SpellState.ACTIVE;
        spellSlotArray[slot].isReadyState = false;
        projection = Instantiate(spellSlotArray[slot].projectionPrefab, Vector3.zero, Quaternion.identity);
        projectionOn = true;
    }

    public void switchProjectionOff()
    {
        projectionOn = false;
        Destroy(projection);
    }

    public void Strike(int slot)
    {
        projectionOn = false;
        Destroy(projection);

        // Creates Visual Prefab
        if (spellSlotArray[slot].currentState == SpellDataTemplate.SpellState.READY)
        {
            InstantiateStrike(projection.transform.position, slot);
        }


        if (spellSlotArray[slot].doesDamage)
        {
            DetectCharacters(projection.transform.position, slot);
        }
    }

    public void InstantiateStrike(Vector3 centre, int slot)
    {
        GameObject strike = Instantiate(spellSlotArray[slot].Spellprefab, centre + Vector3.up * 0.5f, componentRegistry.playerCamera.transform.rotation);
        // NEED TO ADD:
        // ENSURE THE INSTANTIATED OBJECT DESTROYS ITSELF

        //AudioManager.instance.PlayOneShot(FMODEvents.instance.thunderSound, this.transform.position);
    }

    public bool PlayerCheck(GameObject hitbox)
    {
        GameObject player = hitbox;

        if (player.layer == LayerMask.NameToLayer("layer_Player"))
        {
            return true;
        }

        return false;
    }

    public void DetectCharacters(Vector3 centre, int slot)
    {
        Collider[] colliders = Physics.OverlapSphere(centre, spellSlotArray[slot].radius);
        List<GameObject> players = new List<GameObject>();

        foreach (Collider collider in colliders)
        {
            Debug.Log(collider.name);
            if (PlayerCheck(collider.gameObject))
            {
                players.Add(collider.gameObject);
            }
        }


        foreach (var player in players)
        {
            float distance = Vector3.Distance(centre, player.transform.position);

            float damageMultiplier = spellSlotArray[slot].damageValue / spellSlotArray[slot].radius;

            // Adjust the damage based on distance (you can use any formula here)
            float adjustedDamage = spellSlotArray[slot].damageValue - distance * damageMultiplier;

            // Make sure the adjusted damage is not negative
            adjustedDamage = Mathf.Max(0, adjustedDamage);
            DealDamage(player, adjustedDamage);
        }
    }
    #endregion

    #region Active Spell Logic

    #endregion


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

    private void HandleTwoStageSpells(int slot)
    {
        if (spellSlotArray[slot].currentState == SpellDataTemplate.SpellState.READY)
        {
            if (projectionOn && spellSlotArray[slot].isReadyState)
            {
                spellSlotArray[slot].isReadyState = false;
                switchProjectionOff();
                Strike(slot);
                spellSlotArray[slot].currentState = SpellDataTemplate.SpellState.COOLDOWN;
            }
            else if (projection == null)
            {
                switchProjectionOn(slot);
            }
        }
      


    }

    private void HandleActiveSpells(int slot)
    {
        if (spellSlotArray[slot].currentState == SpellDataTemplate.SpellState.READY)
        {
            spellSlotArray[slot].currentState = SpellDataTemplate.SpellState.ACTIVE;
            GameObject activeSpell = Instantiate(spellSlotArray[slot].Spellprefab, spellSlotArray[slot].targetPoint.position, componentRegistry.playerCamera.transform.rotation);
            activeSpell.transform.parent = transform.parent.transform;
            activeSpell.GetComponent<FlameThrower>().setValues(transform.parent.gameObject, spellSlotArray[slot].activeTime, spellSlotArray[slot].damageValue);
        }
        
       
        


        //INSTANTIATE PREFAB UNDER PLAYER 
        //SET ACTIVE STATE
        //PASS SOURCE TO PREFAB SCRIPT
        //PASS OBJ ACTIVETIME INTO PREFAB
        //PREFAB DEL SELF AFTER ACTIVETIME

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
                if (!spellSlotArray[slot].changingState)
                {
                    spellSlotArray[slot].changingState = true;
                    StartCoroutine(SpellActiveTimer(slot, spellSlotArray[slot].activeTime, spellSlotArray[slot].activeForCooldown));
                }
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

    private IEnumerator SpellActiveTimer(int slot, float activeTime, bool setCooldown)
    {
        yield return new WaitForSeconds(spellSlotArray[slot].activeTime);
        if(setCooldown)
        {
            Debug.Log("WE ARE IN SETCOOLDOWN OF ACTIVE");
            spellSlotArray[slot].currentState = SpellDataTemplate.SpellState.COOLDOWN;
        }
        else
        {
            spellSlotArray[slot].currentState = SpellDataTemplate.SpellState.READY;
            spellSlotArray[slot].isReadyState = true;
        }
        spellSlotArray[slot].changingState = false;
    }

    private IEnumerator SpellCooldownTimer(int slot, float cooldown)
    {
        yield return new WaitForSeconds(spellSlotArray[slot].cooldownTime);
        spellSlotArray[slot].currentState = SpellDataTemplate.SpellState.READY;
        spellSlotArray[slot].changingState = false;
        spellSlotArray[slot].isReadyState = true;

    }

    #endregion


    public void Cast(int slotNumber)
    {
        Debug.Log("TRYING TO CAST SPELL");
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
                HandleActiveSpells(slotNumber);
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
                Debug.Log("ATTEMPTING TO ICE WALL");
                HandleTwoStageSpells(slotNumber);
                break;

            #endregion


            #region Wind
            case SpellDataTemplate.SpellID.WindProjectile:
                HandleProjectileSpells(slotNumber);
                break;

            case SpellDataTemplate.SpellID.WhirlwindBouncePad:
                HandleTwoStageSpells(slotNumber);
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
                HandleTwoStageSpells(slotNumber);
                break;

            case SpellDataTemplate.SpellID.LightningChainRaycast:
                HandleRaycastSpells();
                break;

            #endregion


            #region Ultimates
            case SpellDataTemplate.SpellID.Beam:
                HandleActiveSpells(slotNumber);
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

    private void DealDamage(GameObject player, float damgage)
    {

    }

    private void HandleSFX()
    {

    }


    //Function to equip ultimate

    //Set Active For function for ultimate

    //Set Cooldown function
}


