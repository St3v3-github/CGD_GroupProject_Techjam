using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LightFlicker : MonoBehaviour
{
    public GameObject Light;
    public bool lighton;
    public float v;
    public float x; 
    // Start is called before the first frame update
    void Start()
    {
       TurnOff();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void TurnOff()
    {
        v = Random.Range(0.5f, 4f);
        StartCoroutine(Off());
        IEnumerator Off() { yield return new WaitForSeconds(v); Light.SetActive(false); TurnOn(); }

    }

    void TurnOn()
    {
        x = Random.Range(0.5f, 4f);
        StartCoroutine(On());
        IEnumerator On() { yield return new WaitForSeconds(x); Light.SetActive(true); TurnOff(); }

    }
}
