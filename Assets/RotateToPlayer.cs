using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToPlayer : MonoBehaviour

{
    public FlameThrower ft;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = ft.source.GetComponent<ComponentRegistry>().playerCamera.transform.rotation;
        transform.position = ft.source.GetComponent<ComponentRegistry>().firePoint.transform.position;

        
    }
}
