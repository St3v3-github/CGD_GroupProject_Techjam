using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemData;

[CreateAssetMenu(fileName = "SpellData", menuName = "ScriptableObjects/SpellDataTemplate")]

public class SpellDataTemplate : ScriptableObject
{

    #region Spell Variables
    [Header("Spell Variables")]

    //
    public GameObject prefab;
    public StatusEffect_Data statusEffect_Data;

    //Sprite for UI
    public Sprite Icon;

    public bool isUltimate = true;

    public float damageValue;

    public bool isReadyState;
    public float activeTime;
    public float cooldownTime;

    #endregion

    //-----------------------------------------------------------------------------------------------

    #region Spell State
    public enum SpellState
    {
        READY = 0,
        ACTIVE,
        COOLDOWN
    }

    public SpellState currentState = SpellState.READY;

    #endregion

    //-----------------------------------------------------------------------------------------------

    #region Spell ID
    public enum SpellID
    {
        #region Null
        EMPTY = 0,
        #endregion

        #region Fire
        FireProjectile = 1,
        FireGrenade = 2,
        FlameThrower = 3,
        #endregion


        #region Ice
        IceProjectile = 4,
        IceGrenade = 5,
        IceWall = 6,
        #endregion


        #region Wind
        WindProjectile = 7,
        WhirlwindBouncePad = 8,
        WindRushKnockback = 9,        
        #endregion


        #region Lightning
        LightningProjectile = 10,
        LightningStrikeAOE = 11,
        LightningChainRaycast = 12,
        #endregion


        #region Ultimates
        BlackHole = 13,
        Heal = 14,
        PoisonCloud = 15,
        Beam = 16,
        #endregion

    }

    public SpellID ID;

    #endregion

    //-----------------------------------------------------------------------------------------------

    #region Projectile Data
    //element specific projectile goes here: (drag & drop in inspector)
    private ProjectileData projectile;
    

    #endregion


    #region Grenade Data
    
    #endregion


    #region AOE Data
   
    #endregion



    #region Ultimate Data

    #endregion

}

