using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public GameObject trainObj; 
    // Start is called before the first frame update
    void Start()
    {
        startTrain();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startTrain() 
    {
        StartCoroutine(train());
        IEnumerator train() { yield return new WaitForSecondsRealtime(1); trainObj.SetActive(true); }
    }

}
