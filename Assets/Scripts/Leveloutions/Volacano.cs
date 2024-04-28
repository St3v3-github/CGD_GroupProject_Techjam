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
            //Sound For Build Up
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
            //Sound for erruption

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
