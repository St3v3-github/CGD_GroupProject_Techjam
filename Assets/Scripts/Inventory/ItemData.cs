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
        WINDBALL = 3,
        LIGHTNINGBALL = 4,
        FIREWALL = 5,
        ICEWALL = 6,
        WINDGWALL = 7,
        LIGHTNINGWALL = 8,
        FIRESUMMON = 9,
        ICESUMMON = 10,
        WINDSUMMON = 11,
        LIGHTNINGSUMMON = 12,
        FIRESTRIKE = 13,
        ICESTRIKE = 14,
        WINDSTRIKE = 15,
        LIGHTNINGSTRIKE = 16,
        BLACKHOLE = 17,
        HEALSPELL = 18,
        POISONCLOUD = 19,
        POWERBEAM = 20,
        //More
    }

    public enum SpellType
    {
        EMPTY = 0,
        FIRE = 1,
        ICE = 2,
        WIND = 3,
        LIGHTNING = 4,
        //More
    }

    public SpellList ID = 0;
    public SpellType type = 0;
    public SpellState current_state = SpellState.READY;
    public string item_name = "Empty";
    public string description = "This is an empty inventory slot.";
    //public bool degrading = false;
    //public int condition = 100;
    public uint uses_left = 1;
    public int spawn_weight = 1;
    public Sprite icon; 
}
