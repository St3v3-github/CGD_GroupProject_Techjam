using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeManager : MonoBehaviour
{
    //should be private for final (as we use getters and setters) but keep public for dev so we can eyeball inspector
    [SerializeField] public float health;
    [SerializeField] public int mp;
    [SerializeField] public float speed;


    //examples of other values we might eventually have. all values relating to the player would probably be held in this one manager
    //[SerializeField] private int move_speed;
    //[SerializeField] private int defensive_power;
    //[SerializeField] private int offensive_power;

    //CURRENT STATUS HERE
    public StatusEffect player_status;

    void Start()
    {
        //set all values to whatever default value we want
        health = 100;
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

    public float TakeDamage(float damage)
    {

        health -= damage;

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
