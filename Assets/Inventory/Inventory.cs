using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public enum InventoryModes
    {
        DRAGDROP=0,
        CRAFT=1
    }
    public InventoryModes inventory_mode;
    public int inv_width = 3;
    public int inv_height = 4;
    public int spell_slots = 4;

    public Inventory_UI inventory_ui;
    
    public List<ItemData> inventory_items = new();

    public List<List<ItemData>> dd_spell_inventory = new();

    public List<ItemData> crafting_menu = new();
    public List<SpellData> c_spell_inventory = new();
    SpellData c_new_spell;

    void Start()
    {
        //Init so crafting doesn't break when encountering null
        SpellData data_guide = SpellData.CreateInstance<SpellData>();
        for (int j = 0; j < data_guide.spell_components; j++)
        {
            crafting_menu.Add(ItemData.CreateInstance<ItemData>());
        }
        //Probs needs init so spellcasting without a spell doesn't break due to null values
        
        for(int i = 0; i < spell_slots; i++)
        {
            dd_spell_inventory.Add(new List<ItemData>());
            for(int j = 0; j<data_guide.spell_components;j++)
            {
                dd_spell_inventory[i].Add(ItemData.CreateInstance<ItemData>());
            }
        }

        for (int i = 0; i < inv_height * inv_width; i++)
        {
            inventory_items.Add(ItemData.CreateInstance<ItemData>());
        }
    }

    public bool hasSpace()
    {
        for(int i = 0; i<inv_height*inv_width;i++)
        {
            if (inventory_items[i].ID == 0)
            {
                return true;
            }
        }
        return false;
    }

    public bool addItem(ItemData new_item)
    {
        for (int i = 0; i < inv_height * inv_width; i++)
        {
            if (inventory_items[i].ID == 0)
            {
                inventory_items[i] = new_item;
                inventory_ui.updateInvDisplay();
                return true;
            }
        }
        return false;
    }

    public bool craftSpell()
    {
        for(int i = 0; i<c_new_spell.spell_components;i++)
        {
            if (crafting_menu[i].ID != 0)
            {
                c_new_spell.components[i] = crafting_menu[i];
            }
            else 
            {
                return false;
            }
        }
        return true;
    }

    public void swapItems(bool first_inv, int first_id, bool second_inv, int second_id)
    {
        ItemData first_item;
        if (first_inv)
        {
            first_item = inventory_items[first_id];
            if(second_inv) 
            {
                inventory_items[first_id] = inventory_items[second_id];
                inventory_items[second_id] = first_item;
            }
            else
            {
                inventory_items[first_id] = dd_spell_inventory[second_id / spell_slots][second_id % spell_slots];
                dd_spell_inventory[second_id / spell_slots][second_id % spell_slots] = first_item;
            }
        }
        else
        {
            first_item= dd_spell_inventory[first_id/spell_slots][first_id%spell_slots];
            if (second_inv)
            {
                dd_spell_inventory[first_id / spell_slots][first_id % spell_slots] = inventory_items[second_id];
                inventory_items[second_id] = first_item;
            }
            else
            {
                dd_spell_inventory[first_id / spell_slots][first_id % spell_slots] = dd_spell_inventory[second_id / spell_slots][second_id % spell_slots];
                dd_spell_inventory[second_id / spell_slots][second_id % spell_slots] = first_item;
            }
        }
    }
}
