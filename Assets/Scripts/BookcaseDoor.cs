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
        StartCoroutine(Delay());
        IEnumerator Delay() { yield return new WaitForSecondsRealtime(90); Trigger(); }
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

    public void Trigger()
    {
        wall.SetActive(false);
        bookcase.Play("Open");
        Dust.Play("Dust Move");
        //AUDIO FOR BOOKCASE DOOR OPEN, LIKE SCOOBY DOO HIDDEN DOOR NOISE 
    }
}
