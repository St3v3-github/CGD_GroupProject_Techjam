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
    [SerializeField] public float health;
    [SerializeField] public float maxHealth;
    [SerializeField] public int mp;
    [SerializeField] public float speed;


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
        health = 100;
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
        return health;
    }
    public void SetPlayerHealth(int _health)
    {
        health = _health;
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

        health -= damage;
        if (health < 0)
        {
            
        }

        //Particles and Shaders called here


        return health;
    }

    public float TakeDamage(float damage, GameObject attacker)
    {
        
        health -= damage;
        if (health < 0)
        {
            Die(attacker);
        }

        //Particles and Shaders called here


        return health;
    }

    
    public void Die(GameObject killer)
    {
        killing_data data;
        data.killer = killer;
        data.deaded = gameObject;
        SendMessage("PrayToGod", data);
    }


    public float TakeDamage(float damage, StatusEffect statusEffect)
    {

        health -= damage;
        ChangeStatus(statusEffect);

        //Particles and Shaders called here
        Debug.Log("Health: " + health);





        return health;
    }

    public float Heal(float heal)
    {
        if (health < maxHealth)
        {
            health += heal;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
        

        //Particles and Shaders called here


        return health;
    }

    public float OverHeal(float heal)
    {
        health += heal;


        //Particles and Shaders called here


        return health;
    }

    public float ChangeStatus(StatusEffect newStatus)
    {

        if (player_status != newStatus)
        {
            player_status.RemoveEffect();

            player_status = newStatus;

            player_status.ApplyEffect();

        }

        return health;
    }

    public void SpeedModifier(float speedMod)
    {
        speed *= speedMod;
    
    }
}