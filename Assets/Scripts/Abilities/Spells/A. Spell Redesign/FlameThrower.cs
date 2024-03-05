using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    public GameObject source;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Flamethrower has hit " + other.name);
        if(other.gameObject.CompareTag("Player") && other != source)
        {
            other.GetComponentInParent<ComponentRegistry>().attributeManager.TakeDamage(0.2F);
        }
    }
    
    public void SetSource(GameObject newSource)
    {
        source = newSource;
    }
}
