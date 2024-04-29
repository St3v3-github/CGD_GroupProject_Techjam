using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trainNoise : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(whistle());
        IEnumerator whistle()
        {
            yield return new WaitForSecondsRealtime(30);

            AudioManager.instance.PlayOneShot(FMODEvents.instance.trainSound, this.transform.position);
            StartCoroutine(whistle());
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
