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

    public enum SpellList
    {
        // CLASS SPELLS
        EMPTY = 0,
        FIREGRENADE = 1,
        FLAMETHROWER = 2,
        ICESLOW = 3,
        ICEWALL = 4,
        WIRLWIND = 5,
        WINDRUSH = 6,
        LIGHTNINGSTRIKE = 7,
        CHAINEDLIGHTNING = 8,
        GROUNDSMASH = 9,
        EARTHARENA = 10,
        // ULTIMATE PICKUP SPELLS
        BLACKHOLE = 17,
        HEALSPELL = 18,
        POISONCLOUD = 19,
        POWERBEAM = 20,
        SUMMON = 21,
        //More
    }

    public ItemData BLANK_COPY;
    public SpellList ID = 0;
    public SpellState current_state = SpellState.READY;
    public string item_name = "EMPTY";
    public int spawn_weight = 1;
    public Sprite icon;
    public bool Ultimate = true;
    public float cooldown_duration = 0;
  
    private void ResetObject()
    {
        ID = BLANK_COPY.ID;
        current_state = SpellState.COOLDOWN;
        icon = BLANK_COPY.icon;
    }
}
