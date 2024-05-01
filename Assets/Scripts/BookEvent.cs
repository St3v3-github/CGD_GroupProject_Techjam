using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookEvent : MonoBehaviour
{
    public GameObject book;
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
            Instantiate(book,transform.position,transform.rotation);
            
            
        }
    }

    
}
