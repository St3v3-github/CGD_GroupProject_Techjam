using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class AbilityManager2 : MonoBehaviour
{
    public InventoryEdit inventory;
    public GameObject spell_controller;
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
            Debug.Log("WE ARE IN THE CAST SPELL FUNCTION. CURRENT SLOT IS: " + slot);
            switch(inventory.dd_spell_inventory[slot].ID) 
            {
             /*case ItemData.SpellList.FIREBALL:
                   inventory.setActiveFor(slot, 0.0f, 1.0f); //TODO: Grab active and cooldown times from spells
                   caster.GetComponent<FireProjectile>().setType(ItemData.SpellType.FIRE);
                   caster.GetComponent<FireProjectile>().Fire();
                   break;
               case ItemData.SpellList.ICEBALL:

                   break;
               case ItemData.SpellList.WINDBALL:

                   break;
               case ItemData.SpellList.LIGHTNINGBALL:

                   break;*/
                case ItemData.SpellList.FIREWALL:
                    inventory.setActiveFor(slot, 2.0f, 1.0f); //TODO: Grab active and cooldown times from spells

                    spell_controller.GetComponent<Wall>().setType(spellEnum.fire);
                    spell_controller.GetComponent<WallManager>().setType(spellEnum.fire);
                    if (spell_controller.GetComponent<Wall>().isPlacingWall)
                    {
                        spell_controller.GetComponent<Wall>().PlaceWall();
                        Debug.Log("PLACED FIRE WALL");
                    }
                    else
                    {
                        spell_controller.GetComponent<Wall>().StartPlacingWall();
                        Debug.Log("PLACING FIRE WALL");
                        
                    }
                    break;
                case ItemData.SpellList.ICEWALL:
                    inventory.setActiveFor(slot, 2.0f, 1.0f); //TODO: Grab active and cooldown times from spells

                    spell_controller.GetComponent<Wall>().setType(spellEnum.ice);
                    spell_controller.GetComponent<WallManager>().setType(spellEnum.ice);
                    if (spell_controller.GetComponent<Wall>().isPlacingWall)
                    {
                        spell_controller.GetComponent<Wall>().PlaceWall();
                    }
                    else
                    {
                        spell_controller.GetComponent<Wall>().StartPlacingWall();
                    }
                    break;
                case ItemData.SpellList.WINDGWALL:
                    inventory.setActiveFor(slot, 2.0f, 1.0f); //TODO: Grab active and cooldown times from spells

                    spell_controller.GetComponent<Wall>().setType(spellEnum.wind);
                    spell_controller.GetComponent<WallManager>().setType(spellEnum.wind);
                    if (spell_controller.GetComponent<Wall>().isPlacingWall)
                    {
                        spell_controller.GetComponent<Wall>().PlaceWall();
                    }
                    else
                    {
                        spell_controller.GetComponent<Wall>().StartPlacingWall();
                    }
                    break;
                case ItemData.SpellList.LIGHTNINGWALL:
                    inventory.setActiveFor(slot, 2.0f, 1.0f); //TODO: Grab active and cooldown times from spells

                    spell_controller.GetComponent<Wall>().setType(spellEnum.lightning);
                    spell_controller.GetComponent<WallManager>().setType(spellEnum.lightning);
                    if (spell_controller.GetComponent<Wall>().isPlacingWall)
                    {
                        spell_controller.GetComponent<Wall>().PlaceWall();
                    }
                    else
                    {
                        spell_controller.GetComponent<Wall>().StartPlacingWall();
                    }
                    break;

                case ItemData.SpellList.FIRESTRIKE:
                    spell_controller.GetComponent<CastableAOEStrike>().setType(spellEnum.fire);
                  //  spell_controller.GetComponent<CastableAOEStrike>().Cast();
                    if (!spell_controller.GetComponent<CastableAOEStrike>().projectionOn)
                    {
                        spell_controller.GetComponent<CastableAOEStrike>().switchProjectionOn();
                    }
                    else
                    {
                        spell_controller.GetComponent<CastableAOEStrike>().Strike();
                        spell_controller.GetComponent<CastableAOEStrike>().switchProjectionOff();

                    }


                    break;
                case ItemData.SpellList.ICESTRIKE:
                    spell_controller.GetComponent<CastableAOEStrike>().setType(spellEnum.ice);
                    // spell_controller.GetComponent<CastableAOEStrike>().Cast();
                    if (!spell_controller.GetComponent<CastableAOEStrike>().projectionOn)
                    {
                        spell_controller.GetComponent<CastableAOEStrike>().switchProjectionOn();
                    }
                    else
                    {
                        spell_controller.GetComponent<CastableAOEStrike>().Strike();
                        spell_controller.GetComponent<CastableAOEStrike>().switchProjectionOff();

                    }
                    break;
                case ItemData.SpellList.WINDSTRIKE:
                    spell_controller.GetComponent<CastableAOEStrike>().setType(spellEnum.wind);
                    //  spell_controller.GetComponent<CastableAOEStrike>().Cast();
                    if (!spell_controller.GetComponent<CastableAOEStrike>().projectionOn)
                    {
                        spell_controller.GetComponent<CastableAOEStrike>().switchProjectionOn();
                    }
                    else
                    {
                        spell_controller.GetComponent<CastableAOEStrike>().Strike();
                        spell_controller.GetComponent<CastableAOEStrike>().switchProjectionOff();

                    }
                    break;
                case ItemData.SpellList.LIGHTNINGSTRIKE:
                    spell_controller.GetComponent<CastableAOEStrike>().setType(spellEnum.lightning);
                    //  spell_controller.GetComponent<CastableAOEStrike>().Cast();
                    if (!spell_controller.GetComponent<CastableAOEStrike>().projectionOn)
                    {
                        spell_controller.GetComponent<CastableAOEStrike>().switchProjectionOn();
                    }
                    else
                    {
                        spell_controller.GetComponent<CastableAOEStrike>().Strike();
                        spell_controller.GetComponent<CastableAOEStrike>().switchProjectionOff();

                    }
                    break;
                case ItemData.SpellList.FIRESUMMON:
                    spell_controller.GetComponent<Summon>().setType(spellEnum.fire);
                    spell_controller.GetComponent<Summon>().Cast();
                    break;
                case ItemData.SpellList.ICESUMMON:
                    spell_controller.GetComponent<Summon>().setType(spellEnum.ice);
                    spell_controller.GetComponent<Summon>().Cast();
                    break;
                case ItemData.SpellList.WINDSUMMON:
                    spell_controller.GetComponent<Summon>().setType(spellEnum.wind);
                    spell_controller.GetComponent<Summon>().Cast();
                    break;
                case ItemData.SpellList.LIGHTNINGSUMMON:
                    spell_controller.GetComponent<Summon>().setType(spellEnum.lightning);
                    spell_controller.GetComponent<Summon>().Cast();
                    break;
                case ItemData.SpellList.BLACKHOLE:
                    spell_controller.GetComponent<SpellCastOnStaff>().Cast();
                    break;
                case ItemData.SpellList.HEALSPELL:
                    spell_controller.GetComponent<Heal>().Cast();
                    break;
                case ItemData.SpellList.POISONCLOUD:
                    spell_controller.GetComponent<SpellCastOnRay>().Cast();
                    break;
                case ItemData.SpellList.POWERBEAM:
                    inventory.setActiveFor(slot, 3f, 1.0f);
                    spell_controller.GetComponent<Beam>().cast();
                    break;

            }
        }
        else
        {

        }
        
        return answer;
    }
}
