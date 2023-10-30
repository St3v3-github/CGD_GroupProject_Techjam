using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int spell_slots = 4;
    public int element_selection = 0;

    //2D array, spell slots x spell components
    public List<List<ItemData>> dd_spell_inventory = new();

    void Start()
    {
        //Dynamic init to avoid out of bounds
        SpellData data_guide = SpellData.CreateInstance<SpellData>();
        for(int i = 0; i < data_guide.spell_components; i++)
        {
            dd_spell_inventory.Add(new List<ItemData>());
            for(int j = 0; j< spell_slots; j++)
            {
                dd_spell_inventory[i].Add(ItemData.CreateInstance<ItemData>());
            }
        }
    }

    public ItemData equipFromWorld(ItemData item_to_equip, int slot)
    {
        //Temporary copy
        Debug.Log(slot.ToString());
        ItemData swapped_item = dd_spell_inventory[item_to_equip.type][slot];
        if (dd_spell_inventory[item_to_equip.type][slot].ID == item_to_equip.ID)
        {
            //Upgrade system ->
            dd_spell_inventory[item_to_equip.type][slot].value += item_to_equip.value;
            //Check if value is beyond certain thresholds?
            swapped_item = ItemData.CreateInstance<ItemData>();
            //return blank item data if upgrading to avoid duplication
        }
        else
        {
            //Equip if different
            dd_spell_inventory[item_to_equip.type][slot] = item_to_equip;
        }
        //TODO: updateInvDisplay();
        Debug.Log(dd_spell_inventory[item_to_equip.type][slot].ID.ToString());
        return swapped_item;
        //return this in case the old item should be dropped
    }

    public bool setElementSelection(int slot)
    {
        if (dd_spell_inventory[0][slot].ID != 0)
        {
            element_selection = slot;
            return true;
        }
        return false;
    }

    public int getSelectedElement()
    {
        return dd_spell_inventory[0][element_selection].ID;
    }

    public int getSpellType(int slot)
    {
        return dd_spell_inventory[1][slot].ID;
    }
}
