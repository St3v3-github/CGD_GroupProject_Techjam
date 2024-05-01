using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemData;


public class InventoryEdit : MonoBehaviour
{
    public int spell_slots = 4;
    public ItemData fireballforalpha;
    //public int element_selection = 0;

    //2D array, spell slots x spell components
    //public List<List<ItemData>> dd_spell_inventory = new();
    public List<ItemData> dd_spell_inventory = new List<ItemData>();

    public ItemData equipFromWorld(ItemData item_to_equip, int slot)
    {
        //Temporary copy
        //Debug.Log(slot.ToString());
        ItemData swapped_item = dd_spell_inventory[slot];
        if (dd_spell_inventory[slot].ID == item_to_equip.ID)
        {
            //Upgrade system ->
           // dd_spell_inventory[slot].uses_left += item_to_equip.uses_left;
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

    
    /// <summary>
    /// Call this function on spell use
    /// </summary>
    /// <param name="slot">Target spell slot</param>
    /// <param name="duration">Time spent active before cooldown, useful for UI</param>
    /// <param name="cooldown">Time required before next use</param>
    /// 





    /// REVISIT THIS CODE AS IT'S REQUIRED FOR SPELL STATES
    /// 


///


    /*public void setActiveFor(int slot, float duration, float cooldown)
    {
        dd_spell_inventory[slot].current_state = SpellState.ACTIVE;;
        StartCoroutine(setOnCooldownFor(slot,duration,cooldown));
    }

    private IEnumerator setOnCooldownFor(int slot, float duration, float cooldown)
    {
        yield return new WaitForSeconds(duration);
        dd_spell_inventory[slot].current_state = SpellState.COOLDOWN;
        yield return new WaitForSeconds(cooldown);
        dd_spell_inventory[slot].current_state = SpellState.READY;
    }

    public ItemData.SpellList getSpellData(int slot)
    {
        return dd_spell_inventory[slot].ID;
    }

    public bool checkCooldown(int slot)
    {
        if (dd_spell_inventory[slot].current_state == SpellState.READY)
        {
            return true;
        }
        return false;
    }
    public void SetUsed(int slot)
    {
        dd_spell_inventory[slot].current_state = SpellState.COOLDOWN;

    }*/
}
