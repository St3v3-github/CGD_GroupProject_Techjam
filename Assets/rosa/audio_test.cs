using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio_test : MonoBehaviour
{
    //single sound:
    
    /*public SoundFXManager soundFXManager;
    [SerializeField] private AudioClip iceWallSound;

    public KeyCode triggerKey = KeyCode.Space;

    void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            if (soundFXManager != null)
            {
                SoundFXManager.instance.PlaySoundFXClip(iceWallSound, transform, 1f);
            }
        }
    }
    */
    
    // random sound:
    
    public SoundFXManager soundFXManager;
    [SerializeField] private AudioClip[] iceWallSounds;

    public KeyCode triggerKey = KeyCode.Space;

    void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            if (soundFXManager != null)
            {
                SoundFXManager.instance.PlayRandomSoundFXClip(iceWallSounds, transform, 1f);
            }
        }
    }
}
