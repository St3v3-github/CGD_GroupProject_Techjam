using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEdit : MonoBehaviour
{
    public int spell_slots = 4;
    //public int element_selection = 0;

    //2D array, spell slots x spell components
    //public List<List<ItemData>> dd_spell_inventory = new();
    public List<ItemData> dd_spell_inventory = new();

    void Start()
    {
        //Dynamic init to avoid out of bounds
        for (int j = 0; j < spell_slots; j++)
        {
            dd_spell_inventory.Add(ItemData.CreateInstance<ItemData>());
        }
    }

    public ItemData equipFromWorld(ItemData item_to_equip, int slot)
    {
        //Temporary copy
        //Debug.Log(slot.ToString());
        ItemData swapped_item = dd_spell_inventory[slot];
        if (dd_spell_inventory[slot].ID == item_to_equip.ID)
        {
            //Upgrade system ->
            dd_spell_inventory[slot].uses_left += item_to_equip.uses_left;
            //Check if value is beyond certain thresholds?
            swapped_item = ItemData.CreateInstance<ItemData>();
            //return blank item data if upgrading to avoid duplication
        }
        else
        {
            //Equip if different
            dd_spell_inventory[slot] = item_to_equip;
        }
        //TODO: updateInvDisplay();
        //Debug.Log(dd_spell_inventory[item_to_equip.type][slot].ID.ToString());

        return swapped_item;
        //return this in case the old item should be dropped
    }



    public ItemData.SpellList getSpellData(int slot)
    {
        return dd_spell_inventory[slot].ID;
    }
}
