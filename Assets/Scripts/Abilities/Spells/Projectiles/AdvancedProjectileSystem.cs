using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AdvancedProjectileSystem : Spell
{
    public ProjectileData equippedProjectile;
    private List<ProjectileData> spells = new List<ProjectileData>();
    private int currentSpellIndex = 0;

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
    public AnimationManager animControl;

    //Graphic (Coming soon)
    //public GameObject spellFlash, spellImpact;
    //public CamShake camShake;
    //public float camShakeMagnitude, cameShakeDuration;
    //public Text text;



    private void Start()
    {
        setTargetTag();

    }

    private void OnEnable()
    {
        PickupSpell.OnSpellPickedUp += HandleSpellPickedUp;
    }

    private void OnDisable()
    {
        PickupSpell.OnSpellPickedUp -= HandleSpellPickedUp;
    }

    private void Awake()
    {
        chargesLeft = equippedProjectile.totalCharges;
        readyToShoot = true;
    }

    private void Update()
    {
        if(equippedProjectile != null)
        {
            MyInput();
        }
        

        //UI
        //text.SetText(chargesLeft / projectile.projectilesPerTap + " / " projectile.totalCharges / projectile.projectilesPerTap);
    }
    public void MyInput()
    {
        //If hold to fire
        if (equippedProjectile.allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        //If 1 shot per click
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);
        // Additional check for security
        if (Input.GetKeyUp(KeyCode.Mouse0)) shooting = false;


        //Recharging
        if (Input.GetKeyDown(KeyCode.R) && chargesLeft < equippedProjectile.totalCharges && !recharging) Recharge();
        //Automatic Recharging
        if (readyToShoot && shooting && !recharging && chargesLeft <= 0) Recharge();

        //Fire
        if (readyToShoot && shooting && !recharging && chargesLeft > 0)
        {
            chargesShot = 0;
            animControl.toggleCastingBool(true);
            ProjectileShoot();

        }
    }


    private void ProjectileShoot()
    {
        readyToShoot = false;
        Debug.Log("Shoot called");

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
        float x = Random.Range(-equippedProjectile.spread, equippedProjectile.spread);
        float y = Random.Range(-equippedProjectile.spread, equippedProjectile.spread);
        float z = Random.Range(-equippedProjectile.spread, equippedProjectile.spread);
        Debug.Log(Vector3.Magnitude(directionWithoutSpread));
        Vector3 directionWithSpread = directionWithoutSpread + (new Vector3(x, y, z) * Vector3.Magnitude(directionWithoutSpread)) / 15;

        //Instantiate Projectile
        GameObject currentProjectile = Instantiate(equippedProjectile.projectile, firePoint.position, Quaternion.identity);
        currentProjectile.transform.forward = directionWithSpread.normalized;

        //Add Forces to projctile
        currentProjectile.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * equippedProjectile.shootForce, ForceMode.Impulse);
        // For bouncing projectiles only    
        currentProjectile.GetComponent<Rigidbody>().AddForce(playerCam.transform.up * equippedProjectile.upwardForce, ForceMode.Impulse);

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
            Invoke("ResetShot", equippedProjectile.timeBetweenShots);
            allowInvoke = false;
        }
        

        // if multishot projectile, repeat function (for burst or shotgun)
        if (chargesShot < equippedProjectile.projectilesPerTap && chargesLeft > 0)
        {
            Invoke("ProjectileShoot", equippedProjectile.burstDelay);
        }
    }


    private void RayShoot()
    {
        readyToShoot = false;
        chargesLeft--;
        chargesShot++;

        //Spread
        float x = Random.Range(-equippedProjectile.spread, equippedProjectile.spread);
        float y = Random.Range(-equippedProjectile.spread, equippedProjectile.spread);

        Vector3 direction = playerCam.transform.forward + new Vector3(x, y, 0);

        //Raycast
        if (Physics.Raycast(playerCam.transform.position, direction, out rayHit, equippedProjectile.range, hittable))
        {
            Debug.Log(rayHit.collider.name);
            if (rayHit.collider.CompareTag(targetTag))
            {
                dealDamage(rayHit.collider.gameObject, equippedProjectile.damage);
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
            Invoke("ResetShot", equippedProjectile.timeBetweenShots);
            allowInvoke = false;
        }

        if (chargesShot < equippedProjectile.totalCharges && chargesLeft > 0)
        {
            Invoke("RayShoot", equippedProjectile.burstDelay);
        }
    }

    private void ResetShot()
    {
        allowInvoke = true;
        readyToShoot = true;
        Debug.Log("Ready to shoot");
        animControl.toggleCastingBool(false);
    }

    private void Recharge()
    {
        recharging = true;
        Invoke("RechargeFinished", equippedProjectile.rechargeTime);
    }

    private void RechargeFinished()
    {
        chargesLeft = equippedProjectile.totalCharges;
        recharging = false;
    }

    private void HandleSpellPickedUp(ProjectileData spellObject)
    {
        Debug.Log("Subscribed to handler");
        CollectSpell(spellObject);        

    }

    private void CollectSpell(ProjectileData spellObject)
    {
        Debug.Log("Collect Spell Called");

        if (spells.Count < 2)
        {
            equippedProjectile = spellObject;

            //store collected spell
            spells.Add(spellObject);
            Debug.Log("Spell collected: " + spellObject.name);

            //equip if first spell
            if (spells.Count == 1)
            {
                EquipSpell(currentSpellIndex);
            }
        }
        else
        {
            //replace equipped spell with new pickup
            spells[currentSpellIndex] = spellObject;
            Debug.Log("Replaced equipped spell with: " + spellObject.name);
            EquipSpell(currentSpellIndex);
        }
    }

    private void EquipSpell(int index)
    {
        if (index >= 0 && index < spells.Count)
        {
            equippedProjectile = spells[index];
            Debug.Log("Equipped spell: " + equippedProjectile.name);
            readyToShoot = true;
        }
    }
}
