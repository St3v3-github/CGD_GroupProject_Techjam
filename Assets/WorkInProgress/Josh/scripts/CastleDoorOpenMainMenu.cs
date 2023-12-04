using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleDoorOpenMainMenu : MonoBehaviour
{
    
    public float moveSpeed = 5;
    

   
    void start()
    {
                    
    }

    private void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        
    }

    
}
