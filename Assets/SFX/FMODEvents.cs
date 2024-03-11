using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    #region Music
    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }
    #endregion

    #region SpellSounds
    [field: Header("Fire SFX")]
    [field: SerializeField] public EventReference fireSound { get; private set; }

    [field: Header("Ice SFX")]
    [field: SerializeField] public EventReference iceSound { get; private set; }

    [field: Header("Electricity SFX")]
    [field: SerializeField] public EventReference zapSound { get; private set; }

    [field: Header("Wind SFX")]
    [field: SerializeField] public EventReference windSound { get; private set; }
    #endregion

    #region WorldSounds
    [field: Header("Pickup SFX")]
    [field: SerializeField] public EventReference pickupSound { get; private set; }
    #endregion

    [field: Header("Thunder SFX")]
    [field: SerializeField] public EventReference thunderSound { get; private set; }

    [field: Header("Stag SFX")]
    [field: SerializeField] public EventReference stagSound { get; private set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found >1 events. ");
        }
        instance = this;
    }
}
