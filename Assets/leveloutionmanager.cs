using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leveloutionmanager : MonoBehaviour
{
    public GameObject[] leveloutions; 
    
    void Start()
    {
        StartCoroutine(Delay());
        IEnumerator Delay() { yield return new WaitForSecondsRealtime(1);  }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void trigger()
    {


    }
}
