using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;


// Add one of these scripts to each scene. It will manage all the audio in that scene.
// HOW TO USE:
// on your object script, add public SoundFXManager soundManager;
// add [SerializeField] private AudioClip(s) nameOfSound;
// when you want to trigger the sound, if soundManager != null, SoundFXManager.instance.PlaySoundFXClip(nameOfSound, transform, volume(1f for default)); OR for random sounds from a selection, call PlayRandomSoundFXClip instead. 

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
