using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class reference : MonoBehaviour
{
    public ParticleSystem nuke;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
       // nuke.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 180 ) { nuke.Play(); }
    }
}
