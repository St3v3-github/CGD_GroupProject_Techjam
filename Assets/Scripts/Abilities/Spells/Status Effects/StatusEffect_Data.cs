using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect")]

public class StatusEffect_Data : ScriptableObject
{
    public string Name;
    public float DOT_Amount;
    public float MovementPen;
    public float TickSpeed;
    public float Lifetime;

    public GameObject EffectParticles;

    public bool isFire;
    public bool isIce;
    public bool isLightning;
    public bool isWind;

}
