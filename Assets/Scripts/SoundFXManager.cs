using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;
    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //spawn gameobject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //assign audio clip
        audioSource.clip = audioClip;
        
        //assign volume
        audioSource.volume = volume;
        
        //play sound
        audioSource.Play();
        
        //get length of sound clip
        float clipLength = audioSource.clip.length;
        
        //destroy clip after done playing
        Destroy(audioSource.gameObject, clipLength);
    }
    
    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        //assign random 
        int rand = Random.Range(0, audioClip.Length);
        
        //spawn gameobject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //assign audio clip
        audioSource.clip = audioClip[rand];
        
        //assign volume
        audioSource.volume = volume;
        
        //play sound
        audioSource.Play();
        
        //get length of sound clip
        float clipLength = audioSource.clip.length;
        
        //destroy clip after done playing
        Destroy(audioSource.gameObject, clipLength);
    }
}
