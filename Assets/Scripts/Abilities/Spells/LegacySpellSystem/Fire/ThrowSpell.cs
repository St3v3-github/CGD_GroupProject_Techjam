using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class ThrowSpell : Spell
{
    public Transform firePoint;
    public GameObject prefab;
    public Transform playerCam;
    private Vector3 verticalOffset = new Vector3(0f, 0.5f, 0f);
    [Range(0, 100)]
    public float projectileForce = 10f;
    
    

    private void Start()
    {
     
    }

    void Update()
    {

       /* if (Input.GetKeyDown(KeyCode.V))
        {
            Cast();
            //AudioManager.instance.PlayOneShot(FMODEvents.instance.zapSound, this.transform.position);
        }*/
    }

    /*public override void Cast()
    {
        Debug.Log("Throw");
        GameObject projectile = Instantiate(prefab, firePoint.position, playerCam.rotation);
        projectile.GetComponent<Grenade>().source = this.gameObject;
        projectile.tag = this.tag + "Spell";
        //AudioManager.instance.PlayOneShot(FMODEvents.instance.iceSound, this.transform.position);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(playerCam.forward * projectileForce, ForceMode.Impulse);
            
        }
    }*/
}
