using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    public Transform playerCam;
    private Rigidbody rb;
    private UpdatedPlayerController pm;

    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;

    public float dashCooldown;
    public float dashCooldownTimer;

    private Vector3 delayedForceToApply;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<UpdatedPlayerController>();
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

        pm.dashing = true;
        Vector3 forceToApply = transform.forward * dashForce + transform.up * dashUpwardForce;

        Invoke(nameof(DelayDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);
    }

    private void DelayDashForce()
    {
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    public void ResetDash()
    {
        pm.dashing = false;
    }
}
