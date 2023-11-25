using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDummy : MonoBehaviour
{
    [SerializeField] float currentHealth;
    public float maxHealth;
    public Color redColour;
    public Color baseColour;

    private StatusEffect_Data data;

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
}
