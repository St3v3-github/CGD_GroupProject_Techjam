using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionLayers : MonoBehaviour
{
    public List<GameObject> projections;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetProjLayer(int layer)
    {
        foreach (GameObject projection in projections)
        {
            projection.layer = layer;
        }
        gameObject.layer = layer;
        

    }
}
