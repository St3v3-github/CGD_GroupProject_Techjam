using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public void DecreaseCharges()
    {
        Debug.Log("DECREASING CHARGES");
        uses_left--;
        if (uses_left <= 0)
        {       
            Debug.Log("DESTROYING SPELL");
            ResetObject();
            
        }
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
        OTHER = 5,
        //More
    } 
    public enum SlotType
    {
        EMPTY = 0,
        BASIC = 1,
        SPELL = 2,
        
        //More
    }

    public ItemData BLANK_COPY;
    public SpellList ID = 0;
    public SpellType type = 0;
    public SlotType slot = 0;
    public SpellState current_state = SpellState.READY;
    public string item_name = "EMPTY";
    public string description = "This is an empty inventory slot.";
    //public bool degrading = false;
    //public int condition = 100;
    public uint uses_left = 0;
    public int spawn_weight = 1;
    public Sprite icon;
    public bool has_uses = true;
  
    private void ResetObject()
    {
        type = BLANK_COPY.type;
        slot = BLANK_COPY.slot;
        ID = BLANK_COPY.ID;
        current_state = SpellState.COOLDOWN;
        uses_left = BLANK_COPY.uses_left;
        icon = BLANK_COPY.icon;
    }
}
