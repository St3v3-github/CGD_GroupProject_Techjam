using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemData")]
public class ItemData : ScriptableObject
{
    public enum SpellState
    {
        READY = 0,
        ACTIVE,
        COOLDOWN
    }

    public enum SpellList
    {
        EMPTY = 0,
        FIREBALL = 1,
        ICEBALL = 2,
        LIGHTNINGBALL = 3, 
        WINDBALL = 4,
        FIREWALL = 5
        //More
    }

    public SpellList ID = 0;
    public SpellState current_state = SpellState.READY;
    public string item_name = "Empty";
    public string description = "This is an empty inventory slot.";
    //public bool degrading = false;
    //public int condition = 100;
    public uint uses_left = 1;
    public int spawn_weight = 1;
    public Sprite icon; 
}
