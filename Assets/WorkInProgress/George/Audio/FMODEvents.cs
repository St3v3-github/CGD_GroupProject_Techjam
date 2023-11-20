using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Stag SFX")]
    [field: SerializeField] public EventReference SpellSummoned { get; private set; }

    [field: Header("Projectile SFX")]
    [field: SerializeField] public EventReference ProjectileSummoned { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }

    public static FMODEvents instance { get; private set; }

   private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events script in the scene. ");
        }
        instance = this;
    }
}
