using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemData")]
public class ItemData : ScriptableObject
{
    public int ID = 0;
    public int type = 0;
    public elementEnum element = new elementEnum();
    public enum elementEnum
    {
        fire = 0,
        ice = 1,
        lightning = 2,
        air = 3
    }
    public string item_name = "Empty";
    public string description = "This is an empty inventory slot.";
    //public bool degrading = false;
    //public int condition = 100;
    public uint value = 0;
    public int spawn_weight = 0;
    public Sprite icon; 
}
