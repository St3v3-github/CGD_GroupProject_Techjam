using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public bool dead = false;
    public bool hit_by_strike = false;


    //examples of other values we might eventually have. all values relating to the player would probably be held in this one manager
    //[SerializeField] private int move_speed;
    //[SerializeField] private int defensive_power;
    //[SerializeField] private int offensive_power;/./

    //CURRENT STATUS HERE
    public StatusEffect player_status;
    public GameObject last_damage_player;

    public Slider healthbar;
    public GameObject ScoreText;
    public string scorefloat; 
    void Start()
    {
        //set all values to whatever default value we want
        currentHealth = maxHealth;
        maxHealth = 100;
        mp = 100;

        // Set tags
        string parentTag = transform.parent.tag;
        Debug.Log("parent tag: " + parentTag);
        if(parentTag == "Player1")
        {
            this.tag = "Player1";
        }
        else if(parentTag == "Player2")
        {
            this.tag = "Player2";
        }
    }

    void Update()
    {
        //attribute manager would check players current status and call status functions here!!
        //example: player is on fire via fire status, HP reduced by 5 every 1 second?

        healthbar.value = currentHealth;
        //TODO: Fix this for new GameController
        scorefloat = score.ToString(); 
        ScoreText.GetComponent<TextMeshProUGUI>().text = scorefloat; 
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
    public void Reset()
    {
        currentHealth = maxHealth;
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
        data.deaded = gameObject.transform.parent.gameObject;
        GameObject god = GameObject.Find("GameController");
        god.SendMessage("PrayToGod", data);
        Invoke("unDie", 0.1f);
    }

    public void unDie()
    {
        currentHealth = maxHealth;
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
