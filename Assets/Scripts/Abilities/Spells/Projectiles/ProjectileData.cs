using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    //Projectile
    public GameObject projectile;
    public GameObject recharge;
    public string description;

    //Projectile force
    public float shootForce, upwardForce;

    //Projectile stats
    public int damage;
    public float timeBetweenShots, spread, range, rechargeTime, burstDelay;
    public int totalCharges, projectilesPerTap;
    public bool allowButtonHold;
    
}
