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
                    inventory.setActiveFor(slot, 2.0f, 1.0f); //TODO: Grab active and cooldown times from spells

                    caster.GetComponent<Wall>().setType(ItemData.SpellType.ICE);
                    if (caster.GetComponent<Wall>().isPlacingWall)
                    {
                        //caster.GetComponent<Wall>().PlaceWall();
                    }
                    else
                    {
                        caster.GetComponent<Wall>().StartPlacingWall();
                    }
                    break;
                case ItemData.SpellList.WINDGWALL:
                    inventory.setActiveFor(slot, 2.0f, 1.0f); //TODO: Grab active and cooldown times from spells

                    caster.GetComponent<Wall>().setType(ItemData.SpellType.WIND);
                    if (caster.GetComponent<Wall>().isPlacingWall)
                    {
                        //caster.GetComponent<Wall>().PlaceWall();
                    }
                    else
                    {
                        caster.GetComponent<Wall>().StartPlacingWall();
                    }
                    break;
                case ItemData.SpellList.LIGHTNINGWALL:
                    inventory.setActiveFor(slot, 2.0f, 1.0f); //TODO: Grab active and cooldown times from spells

                    caster.GetComponent<Wall>().setType(ItemData.SpellType.LIGHTNING);
                    if (caster.GetComponent<Wall>().isPlacingWall)
                    {
                        //caster.GetComponent<Wall>().PlaceWall();
                    }
                    else
                    {
                        caster.GetComponent<Wall>().StartPlacingWall();
                    }
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
                    caster.GetComponent<CastableAOEStrike>().Cast();
                    break;
                case ItemData.SpellList.ICESTRIKE:
                    caster.GetComponent<CastableAOEStrike>().setType(ItemData.SpellType.ICE);
                    caster.GetComponent<CastableAOEStrike>().Cast();
                    break;
                case ItemData.SpellList.WINDSTRIKE:
                    caster.GetComponent<CastableAOEStrike>().setType(ItemData.SpellType.WIND);
                    caster.GetComponent<CastableAOEStrike>().Cast();
                    break;
                case ItemData.SpellList.LIGHTNINGSTRIKE:
                    caster.GetComponent<CastableAOEStrike>().setType(ItemData.SpellType.LIGHTNING);
                    caster.GetComponent<CastableAOEStrike>().Cast();
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
                    caster.GetComponent<SpellCastOnStaff>().Cast();
                    break;
                case ItemData.SpellList.HEALSPELL:
                    caster.GetComponent<Heal>().Cast();
                    break;
                case ItemData.SpellList.POISONCLOUD:
                    caster.GetComponent<SpellCastOnRay>().Cast();
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
