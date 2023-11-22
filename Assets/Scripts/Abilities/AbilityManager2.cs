using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class AbilityManager2 : MonoBehaviour
{
    public InventoryEdit inventory;
    public GameObject caster;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int castSpell(int slot)
    {
        int answer = 0;
        if (inventory.dd_spell_inventory[slot].current_state==ItemData.SpellState.READY)
        {
            switch(inventory.dd_spell_inventory[slot].ID) 
            {
                case ItemData.SpellList.FIREBALL:
                    inventory.setActiveFor(slot, 0.0f, 1.0f); //TODO: Grab active and cooldown times from spells
                    caster.GetComponent<FireProjectile>().Fire();
                    break;
                case ItemData.SpellList.ICEBALL: 
                    break;
                case ItemData.SpellList.FIREWALL:
                    inventory.setActiveFor(slot, 2.0f, 1.0f); //TODO: Grab active and cooldown times from spells
                    if (caster.GetComponent<Wall>().isPlacingWall)
                    {
                        //caster.GetComponent<Wall>().PlaceWall();
                    }
                    else
                    {
                        caster.GetComponent<Wall>().StartPlacingWall();
                    }
                    break;
            }
        }
        else
        {

        }
        
        return answer;
    }
}
