using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Destroy(transform.gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
