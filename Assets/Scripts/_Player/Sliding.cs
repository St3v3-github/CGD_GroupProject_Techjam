using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    public Transform playerObj;
    private Rigidbody rb;
    private UpdatedPlayerController pm;

    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;
    public bool slidePressed;

    public float slideYScale;
    private float startYScale;


    private float horizontalInput;
    private float verticalInput;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<UpdatedPlayerController>();

        startYScale = playerObj.localScale.y;
    }

    private void Update()
    {
        /*if (slidePressed && (horizontalInput != 0 || verticalInput != 0))
        {
            StartSlide();
        }

        if(!slidePressed && sliding)
        {
            EndSlide();
        }
*/
        //Debug.Log(slideTimer);
    }

    private void FixedUpdate()
    {
        if(pm.sliding)
        {
            SlidingMovement();
        }
    }

    public void AssignValues(Vector2 moveInput)
    {
        horizontalInput = moveInput.x;
        verticalInput = moveInput.y; 
    }

    public void StartSlide()
    {
        pm.sliding = true;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 1f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        if (!pm.OnSlope() || rb.velocity.y > -0.1f)
        {
            

            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }
        else
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }


        if (slideTimer <= 0)
            EndSlide();
    }

    public void EndSlide()
    {   
        pm.sliding = false;

        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }
}
