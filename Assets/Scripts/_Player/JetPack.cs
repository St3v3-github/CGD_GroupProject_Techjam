using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPack : MonoBehaviour
{
    public bool canUseJetPack;
    public bool usingJetpack;
    public float jetPackThrust;

    /*
    [Range(0f, 1f)]
    public float jetPackDownwardVelocityCancelFactor;

    public float consumeDuration;
    public float refillDurationGrounded;
    public float refillDurationAir;
    public float refillDelay;
    */
    public float currentFill;
    public float maxFill;
    public float lastTimeOfUse;
    
    UpdatedPlayerController pm;
    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<UpdatedPlayerController>();
        rb = GetComponent<Rigidbody>();

        currentFill = maxFill;
    }

    // Update is called once per frame
    void Update()
    {
        if(pm.isGrounded)
        {
            canUseJetPack = false;
            if(currentFill < maxFill)
            {
                currentFill += Time.deltaTime;
            }

        }
        else if (!pm.hasJumpedThisFrame && usingJetpack)
        {
            canUseJetPack = true;
        }

        bool JetPackInUse = canUseJetPack && usingJetpack && currentFill > 0f;

        if(JetPackInUse)
        {
            lastTimeOfUse = Time.deltaTime;
            currentFill -= Time.deltaTime;
            rb.AddForce(rb.transform.up * jetPackThrust, ForceMode.Impulse);
        }
    }

    public void HandleJetPack()
    {

    }
}