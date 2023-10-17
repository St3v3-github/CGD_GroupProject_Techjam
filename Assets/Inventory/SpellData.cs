using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpellData")]
public class SpellData : ScriptableObject
{
    public int spell_components = 3;
    public List<ItemData> components = new();
    public string spell_name = "Empty";
    public string description = "This is an empty spell slot.";
    //public bool degrading = false;
    //public int condition = 100;
    public uint value = 0;
    public Sprite icon;
    public enum ComponentTypeList
    {
        ELEMENT = 0,
        TYPE = 1,
        AUXILIARY = 2
    }
}
