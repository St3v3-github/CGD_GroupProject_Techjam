using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class LevelMusic : MonoBehaviour
{
    //[field: Header("Music")]
    //[field: SerializeField] public EventReference music { get; private set; }
    private static FMOD.Studio.EventInstance Music;

    void Start()
    {
        Music = FMODUnity.RuntimeManager.CreateInstance("event:/BackgroundMusic");
        Music.start();
    }
}
