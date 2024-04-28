using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    private List<EventInstance> eventInstances;
    private EventInstance musicEventInstance;
    private EventInstance ambienceEventInstance;

    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene. ");
        }
        instance = this;

        eventInstances = new List<EventInstance>();
    }

    private void Start()
    {
        Cleanup();
        InitializeMusic(FMODEvents.instance.music);
    }

    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateEventInstance(musicEventReference);
        musicEventInstance.start();
    }
    public void EndMusic(EventReference musicEventReference)
    {
        musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicEventInstance.release();
    }

    public void InitializeAmbience(EventReference ambienceEventReference)
    {
        ambienceEventInstance = CreateEventInstance(ambienceEventReference);
        ambienceEventInstance.start();
    }
    public void EndAmbience(EventReference ambienceEventReference)
    {
        ambienceEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        ambienceEventInstance.release();
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void Cleanup()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        Cleanup();
    }
}
