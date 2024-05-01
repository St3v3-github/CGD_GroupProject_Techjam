using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCulling : MonoBehaviour
{
    public LayerMask p1layerMask;
    public LayerMask p2layerMask;
    public LayerMask p3layerMask;
    public LayerMask p4layerMask;
    private  LayerMask targetMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetGameLayerRecursive(GameObject targetObject, int layer)
    {
        targetObject.layer = layer;
        foreach (Transform child in targetObject.transform)
        {
            child.gameObject.layer = layer;
 
            Transform _HasChildren = child.GetComponentInChildren<Transform>();
            if (_HasChildren != null)
                SetGameLayerRecursive(child.gameObject, layer);
             
        }

       
        switch (layer)
        {
            case 12:
                targetMask = p1layerMask;
                break;
            case 13:
                targetMask = p2layerMask;
                break;
            case 14:
                targetMask = p3layerMask;
                break;
            case 15:
                targetMask = p4layerMask;
                break;
            
        }

        GetComponentInParent<ComponentRegistry>().playerCamera.cullingMask = targetMask;
    }
    public void SetRagdollLayerRecursive(GameObject targetObject, int layer)
    {
        targetObject.layer = layer;
        foreach (Transform child in targetObject.transform)
        {
            child.gameObject.layer = layer;
 
            Transform _HasChildren = child.GetComponentInChildren<Transform>();
            if (_HasChildren != null)
                SetRagdollLayerRecursive(child.gameObject, layer);
             
        }
    }
    public void SetRagdollLayerRecursive(int layer)
    {
        this.gameObject.layer = layer;
        foreach (Transform child in this.gameObject.transform)
        {
            child.gameObject.layer = layer;
 
            Transform _HasChildren = child.GetComponentInChildren<Transform>();
            if (_HasChildren != null)
                SetRagdollLayerRecursive(child.gameObject, layer);
             
        }
    }
}
