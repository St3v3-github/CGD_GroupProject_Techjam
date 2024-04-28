using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MonsterEvent : MonoBehaviour
{
    public Animator Light;
    public GameObject Before; 
    public GameObject After;
    public GameObject Effect;
    public GameObject LightObj; 
    public bool trigger; 
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Delay());
        IEnumerator Delay() { yield return new WaitForSecondsRealtime(120); lightphaseone(); }
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger) 
        {
            lightfall(); 
            trigger = false;
        
        }    
    }

    void lightphaseone()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.floor_creakSound, this.transform.position);
        StartCoroutine(Delay2());
        IEnumerator Delay2() { yield return new WaitForSecondsRealtime(3); lightfall(); }

    }
    void lightfall()
    {
        float v = 0.34f; 
        Light.Play("Fall");
        StartCoroutine(Delay());
        IEnumerator Delay() { yield return new WaitForSeconds(v);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.floor_crackSound, this.transform.position);
            LightObj.SetActive(false); Before.SetActive(false); After.SetActive(true); Effect.SetActive(true); }

    }
}
