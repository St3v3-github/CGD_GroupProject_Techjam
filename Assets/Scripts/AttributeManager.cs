using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeManager : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int mp;

    //examples of other values we might eventually have. all values relating to the player would probably be held in this one manager
    //[SerializeField] private int move_speed;
    //[SerializeField] private int defensive_power;
    //[SerializeField] private int offensive_power;

    // Start is called before the first frame update
    void Start()
    {
        //set all values to whatever default value we want
        health = 100;
        mp = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetPlayerHealth()
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
}
