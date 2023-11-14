using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDummy : MonoBehaviour
{
    int currentHealth;
    public int maxHealth;
    public Color redColour;
    public Color baseColour;

    void Awake()
    {
        currentHealth = maxHealth;
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
        SetRed();
        Invoke("ResetColour",0.3F);
        Debug.Log("ENEMY DAMAGED");
        
    }
}
