using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spell/StrikeData")]
public class StrikeData : SpellData
{
    public float attackRadius = 10f;
    public int charges;
}
