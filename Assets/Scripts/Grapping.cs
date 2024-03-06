using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Grappling : MonoBehaviour
{

    private UpdatedPlayerController pm;
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

    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<UpdatedPlayerController>();
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

        pm.freeze = true;

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
        pm.freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if(grapplePointRelativeYPos < 0)
        {
            highestPointOnArc = overshootYAxis;
        }

        pm.JumpToPosition(grapplePoint, highestPointOnArc);
        Invoke(nameof(stopGrapple), 1f);
    }

    public void stopGrapple()
    {
        grappling = false;
        pm.freeze = false;

        grapplingCooldownTimer = grapplingCooldown;

        lr.enabled = false;
    }
}
