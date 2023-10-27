using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemData")]
public class ItemData : ScriptableObject
{
    public enum RuneTypes
    {
        ELEMENTAL = 0,
        SPELLTYPE = 1
    }
    public enum ElementList
    {
        FIRE = 1,
        ICE = 2,
        WIND = 3,
        LAST
    }
    public enum SpellTypeList
    {
        BALL = 1,
        SUMMONS = 2,
        WALL = 3,
        LAST
    }

    public int ID = 0;
    public int type = 0;
    public string item_name = "Empty";
    public string description = "This is an empty inventory slot.";
    //public bool degrading = false;
    //public int condition = 100;
    public uint value = 0;
    public int spawn_weight = 0;
    public Sprite icon; 
}
