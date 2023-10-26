using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int inv_width = 0;
    public int inv_height = 0;
    public int spell_slots = 4;

    //public Inventory_UI inventory_ui;
    
    //public List<ItemData> inventory_items = new();

    public List<List<ItemData>> dd_spell_inventory = new();

    public List<ItemData> crafting_menu = new();
    public List<SpellData> c_spell_inventory = new();
    SpellData c_new_spell;

    void Start()
    {
        SpellData data_guide = SpellData.CreateInstance<SpellData>();
        for(int i = 0; i < spell_slots; i++)
        {
            dd_spell_inventory.Add(new List<ItemData>());
            for(int j = 0; j<data_guide.spell_components;j++)
            {
                dd_spell_inventory[i].Add(ItemData.CreateInstance<ItemData>());
            }
        }
    }

    public ItemData equipFromWorld(ItemData item_to_equip, int slot)
    {
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
        //updateInvDisplay();
        return swapped_item;
        //return this in case the old item should be dropped
    }
}
