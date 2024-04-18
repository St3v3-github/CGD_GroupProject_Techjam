using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public GameObject trainObj; 
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(train());
        IEnumerator train() { yield return new WaitForSecondsRealtime(60); animator.Play("Train go BRRRRR"); StartCoroutine(train()); }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
