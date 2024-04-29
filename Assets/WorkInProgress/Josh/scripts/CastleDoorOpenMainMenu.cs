using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleDoorOpenMainMenu : MonoBehaviour
{
    
    public float moveSpeed = 5;
    public GameObject gate; 


    void Start()
    {
        StartCoroutine(Gate());
        IEnumerator Gate()
        {
            yield return new WaitForSecondsRealtime(5); gate.SetActive(false) ;
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        
    }

    
}
