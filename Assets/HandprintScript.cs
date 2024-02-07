using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandprintScript : MonoBehaviour
{
    public GameObject[] handprints;
    public Animator coffin;
    public GameObject WallBefore; 
    public GameObject WallAfter;
    public ParticleSystem FX;
    public bool Trigger = false;
    bool allhands = false;
     
    
     
    // Start is called before the first frame update
    void Start()
    {
       
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
        coffin.Play("Open");
        StartCoroutine(Delay());
        IEnumerator Delay() { yield return new WaitForSecondsRealtime(1); Handprintsappear();}
        
        
    }

    public void Handprintsappear()
    {
        int i = 0;
        int k = handprints.Length;
        float v = 0.25f;
        StartCoroutine(Hands());
        IEnumerator Hands()
        {
            while (i < k)
         {
            
            handprints[i].SetActive(true);
            i ++; 
            yield return new WaitForSeconds(v);
            if (i == k )
                { 
                wallchange();
                }
        }
        }

    }

    public void wallchange()
    {
        WallBefore.SetActive(false);
       
        WallAfter.SetActive(true); 
    }
}
