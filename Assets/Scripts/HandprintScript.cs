using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandprintScript : MonoBehaviour
{
    public GameObject[] handprints;
    public Animator coffin;
    public GameObject WallBefore; 
    public GameObject WallAfter;
    public GameObject Fx1;
    public bool Trigger = false;
   
     
    
     
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Delay());
        IEnumerator Delay() { yield return new WaitForSecondsRealtime(30); CoffinAnimaton(); }
    }

    // Update is called once per frame
    void Update()
    {
         if (Trigger)
        {
            CoffinAnimaton();
            Trigger = false;
        }
    }

    
    public void CoffinAnimaton()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.coffinSound, this.transform.position);
        coffin.Play("Open");
        StartCoroutine(Delay());
        IEnumerator Delay() { yield return new WaitForSecondsRealtime(1); Handprintsappear();}
        
        
    }

    public void Handprintsappear()
    {
        int i = 0;
        int k = handprints.Length;
        float v = 0.1f;
        StartCoroutine(Hands());
        IEnumerator Hands()
        {
            while (i < k)
         {
            
            handprints[i].SetActive(true);
            i ++; 
            yield return new WaitForSeconds(v);
          }
            if (i == k )
                {
                
                wallchange();
                }
        
        }

    }

    public void wallchange()
    {

        AudioManager.instance.PlayOneShot(FMODEvents.instance.smashSound, this.transform.position);
        WallBefore.SetActive(false);
        Fx1.SetActive(true); 
        WallAfter.SetActive(true);
        StartCoroutine(Turnoff());
        IEnumerator Turnoff() 
        { 
            yield return new WaitForSecondsRealtime(2);
            Fx1.SetActive(false); 
        }
        
    }
}
