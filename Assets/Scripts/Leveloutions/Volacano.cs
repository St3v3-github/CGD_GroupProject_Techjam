using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volacano : MonoBehaviour
{
    public GameObject FireFX;
    public GameObject EruptionFX;
    // Start is called before the first frame update
    void Start()
    {
        EruptionStageOne();   
    } 

    // Update is called once per frame
    void Update()
    {
        
    }

    void EruptionStageOne () 
    {
        StartCoroutine(FireOFF());
        IEnumerator FireOFF() {
            yield return new WaitForSeconds(200); 
            FireFX.SetActive(false);  
            EruptionStageTwo();
            AudioManager.instance.PlayOneShot(FMODEvents.instance.build_upSound, this.transform.position);
        }
    }

    void EruptionStageTwo() 
    {
        StartCoroutine(Erupt());
        IEnumerator Erupt()
        {
            yield return new WaitForSeconds(6);
            EruptionFX.SetActive(true);
            EruptionStageThree();
            AudioManager.instance.PlayOneShot(FMODEvents.instance.eruptSound, this.transform.position);

        }
    }

    void EruptionStageThree () 
    {

        StartCoroutine(FireOn());
        IEnumerator FireOn()
        {
            yield return new WaitForSeconds(6);
            FireFX.SetActive(true);
            
        }
    }
}
