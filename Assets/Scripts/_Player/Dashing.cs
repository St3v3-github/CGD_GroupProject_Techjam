using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{

    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;

    public float dashCooldown;
    public float dashCooldownTimer;

    private Vector3 delayedForceToApply;

    private GameObject dashEffect;

    //References
    public ComponentRegistry components;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    public void Dash()
    {
        if(dashCooldownTimer > 0)
        {
            return;
        }
        else
        {
            dashCooldownTimer = dashCooldown;
        }

        components.playerController.dashing = true;
        //Vector3 forceToApply = transform.forward * dashForce + transform.up * dashUpwardForce;
        Vector3 forceToApply = components.playerCamera.transform.forward * dashForce + components.playerCamera.transform.up * dashUpwardForce;
        delayedForceToApply = forceToApply;

        Invoke(nameof(DelayDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);
    }

    private void DelayDashForce()
    {
        if (dashEffect == null)
        {
            dashEffect = Instantiate(components.moveAbilityPrefab, transform.position, components.playerCamera.transform.rotation, transform.parent);
        }
        components.rigidBody.velocity = Vector3.zero;
        components.rigidBody.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    public void ResetDash()
    {
        if (dashEffect != null)
        {
            Destroy(dashEffect);
        }
        components.playerController.dashing = false;
    }
}
