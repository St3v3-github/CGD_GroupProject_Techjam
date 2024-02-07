using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookcaseDoor : MonoBehaviour
{
    public Animator bookcase;
    public Animator Dust;
    public GameObject wall;
    public bool trigger; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger)
        {
            wall.SetActive(false);
            bookcase.Play("Open");
            Dust.Play("Dust Move");
        }
        
    }
}
