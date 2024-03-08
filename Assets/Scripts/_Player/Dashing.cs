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
        Vector3 forceToApply = transform.forward * dashForce + transform.up * dashUpwardForce;

        Invoke(nameof(DelayDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);
    }

    private void DelayDashForce()
    {
        components.rigidBody.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    public void ResetDash()
    {
        components.playerController.dashing = false;
    }
}
