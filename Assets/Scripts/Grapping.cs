using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Grappling : MonoBehaviour
{

    public Transform cam;
    public Transform grappleTip;
    public LayerMask Grappleable;

    public float maxGrappleDistance;
    public float grappleDelayTime;

    private Vector3 grapplePoint;

    public float grapplingCooldown;
    private float grapplingCooldownTimer;
    public float overshootYAxis;

    private bool grappling;

    public LineRenderer lr;

    [Header("Component Registry")]
    public ComponentRegistry components;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(grapplingCooldownTimer > 0)
        {
            grapplingCooldownTimer -= Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        if(grappling)
        {
            lr.SetPosition(0, grappleTip.position);
        }
    }

    public void StartGrapple()
    {
        if(grapplingCooldownTimer > 0)
        {
            return;
        }
        grappling = true;

        components.playerController.freeze = true;

        RaycastHit hit;
        if (Physics.Raycast(cam.position,cam.forward, out hit, maxGrappleDistance, Grappleable))
        {
            grapplePoint = hit.point;

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;

            Invoke(nameof(stopGrapple), grappleDelayTime);
        }

        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }

    private void ExecuteGrapple()
    {
        components.playerController.freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if(grapplePointRelativeYPos < 0)
        {
            highestPointOnArc = overshootYAxis;
        }

        components.playerController.JumpToPosition(grapplePoint, highestPointOnArc);
        Invoke(nameof(stopGrapple), 1f);
    }

    public void stopGrapple()
    {
        grappling = false;
        components.playerController.freeze = false;

        grapplingCooldownTimer = grapplingCooldown;

        lr.enabled = false;
    }
}
