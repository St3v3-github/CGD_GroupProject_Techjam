using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    //Projectile
    public GameObject projectile;
    public GameObject recharge;
    public GameObject particle;
    public string description;

    //Projectile force
    public float shootForce, upwardForce;

    //Projectile stats
    public int damage;
    public float timeBetweenShots, spread, range, lifetime, rechargeTime, burstDelay;
    public int totalCharges, projectilesPerTap;
    public bool allowButtonHold;

    //Projectile Status Effects
    public StatusEffect_Data statusEffect;

    //Projectile Sound Effects
  

    public StatusEffect_Data GetStatusEffect_Data()
    {
        return statusEffect;
    }
}

