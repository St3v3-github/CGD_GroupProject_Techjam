using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class AbilityManager2 : MonoBehaviour
{
    public InventoryEdit inventory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool castSpell(int slot)
    {
        bool success = false;
        switch(inventory.dd_spell_inventory[slot].ID) 
        {
            case ItemData.SpellList.FIREBALL:
                success = true;
                break;
                case ItemData.SpellList.ICEBALL: 
                break;
        }
        return success;
    }
}
