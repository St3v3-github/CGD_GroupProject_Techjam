using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct killing_data
{
    public GameObject killer;
    public GameObject deaded;
}
public class AttributeManager : MonoBehaviour
{
    //should be private for final (as we use getters and setters) but keep public for dev so we can eyeball inspector
    [SerializeField] public float currentHealth;
    [SerializeField] public float maxHealth;
    [SerializeField] public int mp;
    [SerializeField] public float speed;
    [SerializeField] public int score;


    //examples of other values we might eventually have. all values relating to the player would probably be held in this one manager
    //[SerializeField] private int move_speed;
    //[SerializeField] private int defensive_power;
    //[SerializeField] private int offensive_power;

    //CURRENT STATUS HERE
    public StatusEffect player_status;
    public GameObject last_damage_player;

    void Start()
    {
        //set all values to whatever default value we want
        currentHealth = maxHealth;
        maxHealth = 100;
        mp = 100;
    }

    void Update()
    {
        //attribute manager would check players current status and call status functions here!!
        //example: player is on fire via fire status, HP reduced by 5 every 1 second?
    }

    public float GetPlayerHealth()
    {
        return currentHealth;
    }
    public void SetPlayerHealth(float _health)
    {
        currentHealth = _health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetPlayerMP()
    {
        return mp;
    }
    public void SetPlayerMP(int _mp)
    {
        mp = _mp;
    }

    //delete this later fix spells ya dumbass but go to sleep now
    public float TakeDamage(float damage)
    {

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            
        }

        //Particles and Shaders called here


        return currentHealth;
    }

    public float TakeDamage(float damage, GameObject attacker)
    {
        
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die(attacker);
        }

        //Particles and Shaders called here


        return currentHealth;
    }

    
    public void Die(GameObject killer)
    {
        killing_data data;
        data.killer = killer;
        data.deaded = gameObject;
        GameObject god = GameObject.Find("GameController");
        god.SendMessage("PrayToGod", data);
    }


    public float TakeDamage(float damage, StatusEffect statusEffect)
    {

        currentHealth -= damage;
        ChangeStatus(statusEffect);

        //Particles and Shaders called here
        Debug.Log("Health: " + currentHealth);





        return currentHealth;
    }

    public float Heal(float heal)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += heal;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
        

        //Particles and Shaders called here


        return currentHealth;
    }

    public float OverHeal(float heal)
    {
        currentHealth += heal;


        //Particles and Shaders called here


        return currentHealth;
    }

    public float ChangeStatus(StatusEffect newStatus)
    {

        if (player_status != newStatus)
        {
            player_status.RemoveEffect();

            player_status = newStatus;

            player_status.ApplyEffect();

        }

        return currentHealth;
    }

    public void SpeedModifier(float speedMod)
    {
        speed *= speedMod;
    
    }
}
