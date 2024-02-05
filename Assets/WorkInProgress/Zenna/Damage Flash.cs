using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{

    [SerializeField] SkinnedMeshRenderer meshRendererToUse;
    public Material originalMat;
    float flashTime = .15f;

    public Material materialToUse;

    public bool Damaging = false;

    // Start is called before the first frame update
    void Start()
    {
        //meshRendererToUse = GetComponent<SkinnedMeshRenderer>();
        //originalColour = meshRendererToUse.material.color;
        //meshRendererToUse.enabled = true;
        //meshRendererToUse.sharedMaterial = materialToUse;
    }

    // Update is called once per frame
    void Update()
    {
        if(Damaging == true)
        {
            DamageFlashing();
            Debug.Log("DAMAGE");
        }

        if (Input.GetKeyDown("space"))
        {
            Debug.Log("CHANGEMATERIAL");
            DamageFlashing();
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BOX"))
        {
            Debug.Log("COLLISIONpaint");
            DamageFlashing();
            //meshRenderer.sharedMaterial = material[1];
        }

        


    }

    void DamageFlashing()
    {
        meshRendererToUse.material = materialToUse;

        Invoke("FlashingStop", flashTime);
    }

    void FlashingStop()
    {

        meshRendererToUse.material = originalMat;

    }
}
