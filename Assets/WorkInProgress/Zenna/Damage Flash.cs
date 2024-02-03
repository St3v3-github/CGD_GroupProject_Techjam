using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{

    MeshRenderer meshRenderer;
    Color originalColour;
    float flashTime = .15f;

    public Material[] material;

    public bool Damaging = false;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalColour = meshRenderer.material.color;
        meshRenderer.enabled = true;
        meshRenderer.sharedMaterial = material[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(Damaging == true)
        {
            DamageFlashing();
            Debug.Log("DAMAGE");
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("COLLISIONpaint");
            DamageFlashing();
            //meshRenderer.sharedMaterial = material[1];
        }

        


    }

    void DamageFlashing()
    {
        meshRenderer.sharedMaterial = material[1];

        Invoke("FlashingStop", flashTime);
    }

    void FlashingStop()
    {

        meshRenderer.material.color = originalColour;

    }
}
