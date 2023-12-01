using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spell/SpellData")]
public class SpellData : ScriptableObject
{
    public float damage;
    public GameObject prefab;
    public float radius;
}
