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

    [field: Header("bookcase SFX")]
    [field: SerializeField] public EventReference bookcaseSound { get; private set; }

    [field: Header("Coffin SFX")]
    [field: SerializeField] public EventReference coffinSound { get; private set; }

    [field: Header("floor crack SFX")]
    [field: SerializeField] public EventReference floor_crackSound { get; private set; }

    [field: Header("floor creak SFX")]
    [field: SerializeField] public EventReference floor_creakSound { get; private set; }

    [field: Header("Glass SFX")]
    [field: SerializeField] public EventReference glassSound { get; private set; }

    [field: Header("train SFX")]
    [field: SerializeField] public EventReference trainSound { get; private set; }

    [field: Header("Wall break SFX")]
    [field: SerializeField] public EventReference smashSound { get; private set; }

    [field: Header("Volcano build up SFX")]
    [field: SerializeField] public EventReference build_upSound { get; private set; }

    [field: Header("Volcano erupt SFX")]
    [field: SerializeField] public EventReference eruptSound { get; private set; }
    #endregion

    [field: Header("Thunder SFX")]
    [field: SerializeField] public EventReference thunderSound { get; private set; }

    [field: Header("Stag SFX")]
    [field: SerializeField] public EventReference stagSound { get; private set; }

    [field: Header("Hit SFX")]
    [field: SerializeField] public EventReference hitSound { get; private set; }

    [field: Header("Death SFX")]
    [field: SerializeField] public EventReference deathSound { get; private set; }

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
