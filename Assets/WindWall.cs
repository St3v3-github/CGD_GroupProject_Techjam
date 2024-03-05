using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WindWall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // LAYER 11 AT THE MOMENT AS THIS LINKS TO THE LAYER_SPELL, PLS CHANGE IF YOU CAN FIND A  BETTER SOLUTION
        // TAG CHECK FOR WIND ORB AS THIS ALLOWS THE WIND PLAYER TO SHOOT THROUGH THE WALL
        if (other.gameObject.layer == 11 && !other.CompareTag("WindOrb"))
        {
            Destroy(other.gameObject);
        }
    }
}
