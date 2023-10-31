using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

public class FireButton : MonoBehaviour, ISelectHandler
{
    public Button Fire;
    public Button Ice;
    public Button Electric;
    public Button Wind;
    public bool On;
    public bool Off;
    public ParticleSystem fireEffects;

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Word"); 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
