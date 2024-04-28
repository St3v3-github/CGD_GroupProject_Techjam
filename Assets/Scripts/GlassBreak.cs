using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBreak : MonoBehaviour
{
    public GameObject before; 
    public GameObject after;
    public bool trigger;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Delay());
        IEnumerator Delay() { yield return new WaitForSecondsRealtime(60); Trigger(); }

    }

    // Update is called once per frame
    void Update()
    {
        if (trigger)
        {   
            after.SetActive(true);
            before.SetActive(false);
        }
        
    }

    void Trigger()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.glassSound, this.transform.position);
        after.SetActive(true);
        before.SetActive(false);

    }
}
