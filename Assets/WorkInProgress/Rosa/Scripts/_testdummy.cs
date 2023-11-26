using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class _testDummy : MonoBehaviour
{
    [SerializeField] float currentHealth;
    public float maxHealth;
    public Color redColour;
    public Color baseColour;

    private StatusEffect_Data data;

    //movement to test slow
    private Vector3 startPos;
    private float baseMoveSpeed = 20f;
    private float currentMoveSpeed;
    private Rigidbody rb;

    void Awake()
    {
        currentHealth = maxHealth;
        startPos = transform.position;  
        currentMoveSpeed = baseMoveSpeed;
        rb = GetComponent<Rigidbody>();
        
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if(currentHealth <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        Move();
    }


    private void OnCollisionEnter(Collision other)
    {
        
        
    }

    private void SetRed()
    {
        GetComponent<Renderer>().material.color = redColour;

    }

    private void ResetColour()
    {
        GetComponent<Renderer>().material.color = baseColour;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player1Spell"))
        {
            SetRed();
            Invoke("ResetColour",0.3F);
            Debug.Log("ENEMY DAMAGED");
        }
        
        
    }

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    public void setCurrentHealth(float health)
    {
        currentHealth = health;
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }

    public bool moveRight = true;
    void Move()
    {
        //if(moveRight && Vector3.Distance(transform.position, startPos + (-transform.right * 3f)) < 0.01)
        //{
        //    moveRight = false;
        //}
        //if(!moveRight && Vector3.Distance(transform.position, startPos + (-transform.right * 3f)) < 0.01)
        //{
        //    moveRight = true;
        //}

        //if(moveRight)
        //{
        //    transform.position += transform.right * currentMoveSpeed * Time.deltaTime;
        //}
        //else
        //{
        //    transform.position += -transform.right * currentMoveSpeed * Time.deltaTime;
        //}

        
        Vector3 targetPosition = moveRight ? startPos + (transform.right * 3f) : startPos - (transform.right * 3f);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01)
        {
            moveRight = !moveRight;
        }

        // Calculate the new position
        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, currentMoveSpeed * Time.deltaTime);

        // Move the Rigidbody to the new position
        rb.MovePosition(newPosition);
       
    }

    public float getBaseMoveSpeed()
    {
        return baseMoveSpeed;
    }

    public float getCurrentMoveSpeed()
    {
        return currentMoveSpeed;
    }

    public void setNewMoveSpeed(float speed) 
    {
        currentMoveSpeed = speed;
    }
}
