using System.Collections;
using System.Collections.Generic;
using System.Timers;

using UnityEngine;
using UnityEngine.InputSystem;

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
 

    public int castSpell(int slot, InputAction.CallbackContext ctx)
    {
        int answer = 0;
        if (inventory.dd_spell_inventory[slot].current_state==ItemData.SpellState.READY)
        {
        //    Debug.Log("WE ARE IN THE CAST SPELL FUNCTION. CURRENT SLOT IS: " + slot);
            switch(inventory.dd_spell_inventory[slot].ID) 
            {
                case ItemData.SpellList.FIREBALL:
                   if(ctx.performed)
                    {
                        if (spell_controller.GetComponent<AdvancedProjectileSystem>().readyToShoot)
                        {
                            spell_controller.GetComponent<AdvancedProjectileSystem>().ToggleShooting();
                            spell_controller.GetComponent<AdvancedProjectileSystem>().ProjectileShoot(); 
                        }
                        
                    }
                                     break;
                case ItemData.SpellList.ICEBALL:
                    if (ctx.performed)
                    {
                        if (spell_controller.GetComponent<AdvancedProjectileSystem>().readyToShoot)
                        {
                            spell_controller.GetComponent<AdvancedProjectileSystem>().ToggleShooting();
                            spell_controller.GetComponent<AdvancedProjectileSystem>().ProjectileShoot(); 
                        }
                    }
                    break;
                case ItemData.SpellList.WINDBALL:
                    if (ctx.performed)
                    {
                        if (spell_controller.GetComponent<AdvancedProjectileSystem>().readyToShoot)
                        {
                            spell_controller.GetComponent<AdvancedProjectileSystem>().ToggleShooting();
                            spell_controller.GetComponent<AdvancedProjectileSystem>().ProjectileShoot(); 
                        }
                    }
                    break;
                case ItemData.SpellList.LIGHTNINGBALL:
                    if (ctx.performed)
                    {
                        if (spell_controller.GetComponent<AdvancedProjectileSystem>().readyToShoot)
                        {
                            spell_controller.GetComponent<AdvancedProjectileSystem>().ToggleShooting();
                            spell_controller.GetComponent<AdvancedProjectileSystem>().ProjectileShoot(); 
                        }
                    }
                    break;
                case ItemData.SpellList.FIREWALL:
                    inventory.setActiveFor(slot, 2.0f, 1.0f); //TODO: Grab active and cooldown times from spells

                    spell_controller.GetComponent<Wall>().SetType(spellEnum.fire);
                    spell_controller.GetComponent<WallManager>().SetType(spellEnum.fire);
                    if (spell_controller.GetComponent<Wall>().isPlacingWall)
                    {
                        spell_controller.GetComponent<Wall>().PlaceWall();
                        inventory.dd_spell_inventory[slot].DecreaseCharges();
                    }
                    else
                    {
                        spell_controller.GetComponent<Wall>().StartPlacingWall();
                       
                        
                    }
                    break;
                case ItemData.SpellList.ICEWALL:
                    inventory.setActiveFor(slot, 2.0f, 1.0f); //TODO: Grab active and cooldown times from spells

                    spell_controller.GetComponent<Wall>().SetType(spellEnum.ice);
                    spell_controller.GetComponent<WallManager>().SetType(spellEnum.ice);
                    if (spell_controller.GetComponent<Wall>().isPlacingWall)
                    {
                        spell_controller.GetComponent<Wall>().PlaceWall();
                        inventory.dd_spell_inventory[slot].DecreaseCharges();
                    }
                    else
                    {
                        spell_controller.GetComponent<Wall>().StartPlacingWall();
                    }
                    break;
                case ItemData.SpellList.WINDGWALL:
                    inventory.setActiveFor(slot, 2.0f, 1.0f); //TODO: Grab active and cooldown times from spells

                    spell_controller.GetComponent<Wall>().SetType(spellEnum.wind);
                    spell_controller.GetComponent<WallManager>().SetType(spellEnum.wind);
                    if (spell_controller.GetComponent<Wall>().isPlacingWall)
                    {
                        spell_controller.GetComponent<Wall>().PlaceWall();
                        inventory.dd_spell_inventory[slot].DecreaseCharges();
                    }
                    else
                    {
                        spell_controller.GetComponent<Wall>().StartPlacingWall();
                    }
                    break;
                case ItemData.SpellList.LIGHTNINGWALL:
                    inventory.setActiveFor(slot, 2.0f, 1.0f); //TODO: Grab active and cooldown times from spells

                    spell_controller.GetComponent<Wall>().SetType(spellEnum.lightning);
                    spell_controller.GetComponent<WallManager>().SetType(spellEnum.lightning);
                    if (spell_controller.GetComponent<Wall>().isPlacingWall)
                    {
                        spell_controller.GetComponent<Wall>().PlaceWall();
                        inventory.dd_spell_inventory[slot].DecreaseCharges();
                    }
                    else
                    {
                        spell_controller.GetComponent<Wall>().StartPlacingWall();
                    }
                    break;

                case ItemData.SpellList.FIRESTRIKE:
                    spell_controller.GetComponent<CastableAOEStrike>().SetType(spellEnum.fire);
                  //  spell_controller.GetComponent<CastableAOEStrike>().Cast();
                    if (!spell_controller.GetComponent<CastableAOEStrike>().projectionOn)
                    {
                        spell_controller.GetComponent<CastableAOEStrike>().switchProjectionOn();
                    }
                    else
                    {
                        spell_controller.GetComponent<CastableAOEStrike>().Strike();
                        spell_controller.GetComponent<CastableAOEStrike>().switchProjectionOff();
                        inventory.dd_spell_inventory[slot].DecreaseCharges();

                    }


                    break;
                case ItemData.SpellList.ICESTRIKE:
                    spell_controller.GetComponent<CastableAOEStrike>().SetType(spellEnum.ice);
                    // spell_controller.GetComponent<CastableAOEStrike>().Cast();
                    if (!spell_controller.GetComponent<CastableAOEStrike>().projectionOn)
                    {
                        spell_controller.GetComponent<CastableAOEStrike>().switchProjectionOn();
                    }
                    else
                    {
                        spell_controller.GetComponent<CastableAOEStrike>().Strike();
                        spell_controller.GetComponent<CastableAOEStrike>().switchProjectionOff();
                        inventory.dd_spell_inventory[slot].DecreaseCharges();

                    }
                    break;
                case ItemData.SpellList.WINDSTRIKE:
                    spell_controller.GetComponent<CastableAOEStrike>().SetType(spellEnum.wind);
                    //  spell_controller.GetComponent<CastableAOEStrike>().Cast();
                    if (!spell_controller.GetComponent<CastableAOEStrike>().projectionOn)
                    {
                        spell_controller.GetComponent<CastableAOEStrike>().switchProjectionOn();
                    }
                    else
                    {
                        spell_controller.GetComponent<CastableAOEStrike>().Strike();
                        spell_controller.GetComponent<CastableAOEStrike>().switchProjectionOff();
                        inventory.dd_spell_inventory[slot].DecreaseCharges();

                    }
                    break;
                case ItemData.SpellList.LIGHTNINGSTRIKE:
                    spell_controller.GetComponent<CastableAOEStrike>().SetType(spellEnum.lightning);
                    //  spell_controller.GetComponent<CastableAOEStrike>().Cast();
                    if (!spell_controller.GetComponent<CastableAOEStrike>().projectionOn)
                    {
                        spell_controller.GetComponent<CastableAOEStrike>().switchProjectionOn();
                    }
                    else
                    {
                        spell_controller.GetComponent<CastableAOEStrike>().Strike();
                        spell_controller.GetComponent<CastableAOEStrike>().switchProjectionOff();
                        inventory.dd_spell_inventory[slot].DecreaseCharges();

                    }
                    break;
                case ItemData.SpellList.FIRESUMMON:
                    spell_controller.GetComponent<Summon>().SetType(spellEnum.fire);
                    spell_controller.GetComponent<Summon>().Cast();
                    inventory.dd_spell_inventory[slot].DecreaseCharges();
                    break;
                case ItemData.SpellList.ICESUMMON:
                    spell_controller.GetComponent<Summon>().SetType(spellEnum.ice);
                    spell_controller.GetComponent<Summon>().Cast();
                    inventory.dd_spell_inventory[slot].DecreaseCharges();
                    break;
                case ItemData.SpellList.WINDSUMMON:
                    spell_controller.GetComponent<Summon>().SetType(spellEnum.wind);
                    spell_controller.GetComponent<Summon>().Cast();
                    inventory.dd_spell_inventory[slot].DecreaseCharges();
                    break;
                case ItemData.SpellList.LIGHTNINGSUMMON:
                    spell_controller.GetComponent<Summon>().SetType(spellEnum.lightning);
                    spell_controller.GetComponent<Summon>().Cast();
                    inventory.dd_spell_inventory[slot].DecreaseCharges();
                    break;
                case ItemData.SpellList.BLACKHOLE:
                    spell_controller.GetComponent<SpellCastOnStaff>().Cast();
                    inventory.dd_spell_inventory[slot].DecreaseCharges();
                    break;
                case ItemData.SpellList.HEALSPELL:
                    spell_controller.GetComponent<Heal>().Cast();
                    inventory.dd_spell_inventory[slot].DecreaseCharges();
                    break;
                case ItemData.SpellList.POISONCLOUD:
                    spell_controller.GetComponent<SpellCastOnRay>().Cast();
                    inventory.dd_spell_inventory[slot].DecreaseCharges();
                    break;
                case ItemData.SpellList.POWERBEAM:
                    inventory.setActiveFor(slot, 3f, 1.0f);
                    spell_controller.GetComponent<Beam>().cast();
                    inventory.dd_spell_inventory[slot].DecreaseCharges();
                    break;

            }
            
        }
        else
        {

        }
        
        return answer;
    }
}
