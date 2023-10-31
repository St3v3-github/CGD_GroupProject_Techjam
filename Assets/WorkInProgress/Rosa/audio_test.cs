using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio_test : MonoBehaviour
{
    //single sound:
    
    public SoundFXManager soundManager;
    [SerializeField] private AudioClip nameOfSound;

    public KeyCode triggerKey = KeyCode.Space;

    void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            if (soundManager != null)
            {
                SoundFXManager.instance.PlaySoundFXClip(nameOfSound, transform, 1f);
            }
        }
    }
    
    
    // random sound:
    /*
    public SoundFXManager soundManager;
    [SerializeField] private AudioClip[] nameOfSounds;

    public KeyCode triggerKey = KeyCode.Space;

    void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            if (soundManager != null)
            {
                SoundFXManager.instance.PlayRandomSoundFXClip(nameOfSounds, transform, 1f);
            }
        }
    }*/
}
