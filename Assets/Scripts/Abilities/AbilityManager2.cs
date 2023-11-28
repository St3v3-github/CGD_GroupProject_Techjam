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
                case ItemData.SpellList.FIREWALL:
                    inventory.setActiveFor(slot, 2.0f, 1.0f); //TODO: Grab active and cooldown times from spells

                    caster.GetComponent<Wall>().setType(ItemData.SpellType.FIRE);
                    if (caster.GetComponent<Wall>().isPlacingWall)
                    {
                        //caster.GetComponent<Wall>().PlaceWall();
                    }
                    else
                    {
                        caster.GetComponent<Wall>().StartPlacingWall();
                    }
                    break;
                case ItemData.SpellList.ICEWALL:

                    break;
                case ItemData.SpellList.WINDGWALL:

                    break;
                case ItemData.SpellList.LIGHTNINGWALL:

                    break;
                case ItemData.SpellList.FIREBALL:
                    inventory.setActiveFor(slot, 0.0f, 1.0f); //TODO: Grab active and cooldown times from spells
                    caster.GetComponent<FireProjectile>().setType(ItemData.SpellType.FIRE);
                    caster.GetComponent<FireProjectile>().Fire();
                    break;
                case ItemData.SpellList.ICEBALL:

                    break;
                case ItemData.SpellList.WINDBALL:

                    break;
                case ItemData.SpellList.LIGHTNINGBALL:

                    break;
                case ItemData.SpellList.FIRESTRIKE:
                    caster.GetComponent<CastableAOEStrike>().setType(ItemData.SpellType.FIRE);
                    break;
                case ItemData.SpellList.ICESTRIKE:

                    break;
                case ItemData.SpellList.WINDSTRIKE:

                    break;
                case ItemData.SpellList.LIGHTNINGSTRIKE:

                    break;
                case ItemData.SpellList.FIRESUMMON:
                    caster.GetComponent<Summon>().setType(ItemData.SpellType.FIRE);
                    caster.GetComponent<Summon>().Cast();
                    break;
                case ItemData.SpellList.ICESUMMON:
                    caster.GetComponent<Summon>().setType(ItemData.SpellType.ICE);
                    caster.GetComponent<Summon>().Cast();
                    break;
                case ItemData.SpellList.WINDSUMMON:
                    caster.GetComponent<Summon>().setType(ItemData.SpellType.WIND);
                    caster.GetComponent<Summon>().Cast();
                    break;
                case ItemData.SpellList.LIGHTNINGSUMMON:
                    caster.GetComponent<Summon>().setType(ItemData.SpellType.LIGHTNING);
                    caster.GetComponent<Summon>().Cast();
                    break;
                case ItemData.SpellList.BLACKHOLE:
                    break;
                case ItemData.SpellList.HEALSPELL:

                    break;
                case ItemData.SpellList.POISONCLOUD:
                    break;
                case ItemData.SpellList.POWERBEAM:
                    inventory.setActiveFor(slot, 3f, 1.0f);
                    caster.GetComponent<Beam>().Cast();
                    break;

            }
        }
        else
        {

        }
        
        return answer;
    }
}
