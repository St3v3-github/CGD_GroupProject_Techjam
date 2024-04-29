using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

    private float startingAirMultiplier;

    public ComponentRegistry component;

    private GameObject jetPackEffect;

    // Start is called before the first frame update
    void Start()
    {
        currentFill = maxFill;
        startingAirMultiplier = component.playerController.airMultiplier;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(component.playerController.isGrounded)
        {
            canUseJetPack = false;
            if(currentFill < maxFill)
            {
                currentFill += Time.deltaTime;
            }

        }
        else if (!component.playerController.hasJumpedThisFrame && usingJetpack)
        {
            canUseJetPack = true;
        }

        bool JetPackInUse = canUseJetPack && usingJetpack && currentFill > 0f;

        if(JetPackInUse)
        {
            if (jetPackEffect == null)
            {
                jetPackEffect = Instantiate(component.moveAbilityPrefab, new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z), Quaternion.Euler(180f, 0f, 0f), transform.parent);
                jetPackEffect.transform.localScale = new Vector3(6f, 6f, 6f);
            }
            //component.playerController.airMultiplier = 0.01f;
            lastTimeOfUse = Time.deltaTime;
            currentFill -= Time.deltaTime;
            if (component.rigidBody.velocity.y < 1f)
            {
                Debug.Log("StrongThrustTest");
                component.rigidBody.AddForce((component.rigidBody.transform.up * jetPackThrust) * 1.5f, ForceMode.Impulse);
            }
            else
            {
                Debug.Log("WeakThrustTest");
                component.rigidBody.AddForce(component.rigidBody.transform.up * jetPackThrust, ForceMode.Impulse);
            }
        }
        else
        {
            //component.playerController.airMultiplier = startingAirMultiplier;
            if (jetPackEffect != null)
            {
                Destroy(jetPackEffect);
            }
        }
    }

    public void HandleJetPack()
    {

    }
}
