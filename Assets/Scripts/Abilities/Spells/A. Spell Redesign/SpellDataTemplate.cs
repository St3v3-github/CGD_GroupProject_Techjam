using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellData", menuName = "ScriptableObjects/SpellDataTemplate")]

public class SpellDataTemplate : ScriptableObject
{

    #region General Spell Variables
    [Header("General Spell Variables")]

    //
    public GameObject Spellprefab;
    public StatusEffect_Data statusEffect_Data;

    //Sprite for UI
    public Sprite Icon;

    public float damageValue;

    public float activeTime;
    public float cooldownTime;

    public bool isReadyState;

    //Adv Proj System
    public Transform targetPoint;
    public bool usesAdvProjSystem = false;

    //SoundEffects


    #endregion

    //-----------------------------------------------------------------------------------------------

    #region Spell State
    public enum SpellState
    {
        READY = 0,
        ACTIVE,
        COOLDOWN
    }
    public bool changingState = false;
    public bool activeForCooldown = false;
    public float waitTime = 0;

    public SpellState currentState = SpellState.READY;

    #endregion

    //-----------------------------------------------------------------------------------------------

    #region Spell ID
    public enum SpellID
    {
        #region Null
        EMPTY = 0,
        #endregion
        //FIRE = 1 - 10
        #region Fire
        FireProjectile = 1,
        FireGrenade = 2,
        FlameThrower = 3,
        #endregion

        // 11 - 20
        #region Ice
        IceProjectile = 4,
        IceGrenade = 5,
        IceWall = 6,
        #endregion

        // 21-30
        #region Wind
        WindProjectile = 21,
        WhirlwindBouncePad = 22,
        WindRushKnockback = 23,
        WindWall = 24,
        #endregion

        //31-40
        #region Lightning
        LightningProjectile = 31,
        LightningStrikeAOE = 32,
        LightningChainRaycast = 33,
        LightningGrenade = 34,
        #endregion

        //41+
        #region Ultimates
        BlackHole = 41,
        Heal = 42,
        PoisonCloud = 43,
        Beam = 44,
        FireStrike = 45,
        #endregion

    }

    public SpellID ID;

    #endregion

    //-----------------------------------------------------------------------------------------------

    #region Projetile Settings
    [Header("Projectile Settings")]
    //Projectile force
    public float shootForce, upwardForce;
    public float timeBetweenShots, spread, range, lifetime, rechargeTime, burstDelay;
    public int projectilesPerTap;
    public bool shootWithRay, shooting, readyToShoot, recharging;
    public bool allowButtonHold;
    public bool fireRateLimited = true;

    #endregion


    #region Grenade Data
    [Header("Grenade Settings")]
    public float throwForce;

    #endregion


    #region AOE Settings
    [Header("AOE Settings")]
    public float radius;

    public GameObject projectionPrefab;
   
    public bool doesDamage;

    #endregion



    #region Ultimate Settings
    [Header("Ultimate Settings")]
    public bool isUltimate = true;

    #endregion

}

