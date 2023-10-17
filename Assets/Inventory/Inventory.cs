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
    }

    public bool hasSpace()
    {
        for(int i = 0; i<inv_height*inv_width;i++)
        {
            if (inventory_items[i] == null)
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
            if (inventory_items[i] == null)
            {
                inventory_items[i] = new_item;
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
}
