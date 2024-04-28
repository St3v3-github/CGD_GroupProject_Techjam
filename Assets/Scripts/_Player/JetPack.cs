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

    [SerializeField] private GameObject jetPackEffectPrefab;
    private GameObject jetPackEffect;

    public ComponentRegistry component;


    // Start is called before the first frame update
    void Start()
    {
        currentFill = maxFill;
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
            lastTimeOfUse = Time.deltaTime;
            currentFill -= Time.deltaTime;
            component.rigidBody.AddForce(component.rigidBody.transform.up * jetPackThrust, ForceMode.Impulse);
        }
        else
        {
            if(jetPackEffect != null)
            {
                Destroy(jetPackEffect);
            }
        }
            
    }

    public void HandleJetPack()
    {

    }
}
