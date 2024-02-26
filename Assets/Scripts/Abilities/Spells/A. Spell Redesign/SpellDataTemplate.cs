using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellData", menuName = "ScriptableObjects/SpellDataTemplate")]
public class SpellDataTemplate : ScriptableObject
{
    [Header("Spell Data")]

    #region General Variables
    private GameObject prefab;
    private StatusEffect_Data statusEffect_Data;

    private float damageAmount;

    private float cooldownTime;

    private enum SpellState
    {
        READY = 0,
        ACTIVE,
        COOLDOWN
    }

    #endregion

    #region Projectile Data
    private enum ProjectileList
    {

    }
    #endregion


    #region Grenade Data

    #endregion


    #region AOE Data

    #endregion


    #region Ultimate Data

    #endregion

}

