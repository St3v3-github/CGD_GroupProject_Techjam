using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class MonsterEvent : MonoBehaviour
{
    public Animator Light;
    public GameObject Before; 
    public GameObject After;
    public GameObject Effect;
    public GameObject LightObj; 
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
            lightfall(); 
            trigger = false;
        
        }    
    }

    void lightfall()
    {
        float v = 0.34f; 
        Light.Play("Fall");
        StartCoroutine(Delay());
        IEnumerator Delay() { yield return new WaitForSeconds(v); LightObj.SetActive(false); Before.SetActive(false); After.SetActive(true); Effect.SetActive(true); }
    }
}
