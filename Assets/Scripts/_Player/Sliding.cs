using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    public Transform playerObj;

    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;
    public bool slidePressed;

    public float slideYScale;
    private float startYScale;


    private float horizontalInput;
    private float verticalInput;

    private GameObject slideEffect;

    //References
    public ComponentRegistry components;


    private void Start()
    {

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
        if(components.playerController.sliding)
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
        components.playerController.sliding = true;

        slideEffect = Instantiate(components.moveAbilityPrefab, transform.position, Quaternion.Euler(0f, 0f, 0f), transform.parent);

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        components.rigidBody.AddForce(Vector3.down * 1f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        if (!components.playerController.OnSlope() || components.rigidBody.velocity.y > -0.1f)
        {


            components.rigidBody.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }
        else
        {
            components.rigidBody.AddForce(components.playerController.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }


        if (slideTimer <= 0)
            EndSlide();
    }

    public void EndSlide()
    {
        components.playerController.sliding = false;

        Destroy(slideEffect);

        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }
}
