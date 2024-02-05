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
                        if (spell_controller.GetComponent<AdvancedProjectileSystem>() == null) { return 0; }
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
                        if (spell_controller.GetComponent<AdvancedProjectileSystem>() == null) { return 0; }
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
                        if (spell_controller.GetComponent<AdvancedProjectileSystem>() == null) { return 0; }
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
                        if (spell_controller.GetComponent<AdvancedProjectileSystem>() == null) { return 0; }
                        if (spell_controller.GetComponent<AdvancedProjectileSystem>().readyToShoot)
                        {
                            spell_controller.GetComponent<AdvancedProjectileSystem>().ToggleShooting();
                            spell_controller.GetComponent<AdvancedProjectileSystem>().ProjectileShoot(); 
                        }
                    }
                    break;
                case ItemData.SpellList.FIREWALL:
                    if (spell_controller.GetComponent<Wall>() == null || spell_controller.GetComponent<WallManager>() == null) { return 0; }
                    inventory.setActiveFor(slot, 2.0f, 1.0f); //TODO: Grab active and cooldown times from spells

                    spell_controller.GetComponent<Wall>().setType(spellEnum.fire);
                    spell_controller.GetComponent<WallManager>().setType(spellEnum.fire);
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
                    if (spell_controller.GetComponent<Wall>() == null || spell_controller.GetComponent<WallManager>() == null) { return 0; }
                    inventory.setActiveFor(slot, 2.0f, 1.0f); //TODO: Grab active and cooldown times from spells

                    spell_controller.GetComponent<Wall>().setType(spellEnum.ice);
                    spell_controller.GetComponent<WallManager>().setType(spellEnum.ice);
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
                    if (spell_controller.GetComponent<Wall>() == null || spell_controller.GetComponent<WallManager>() == null) { return 0; }
                    inventory.setActiveFor(slot, 2.0f, 1.0f); //TODO: Grab active and cooldown times from spells

                    spell_controller.GetComponent<Wall>().setType(spellEnum.wind);
                    spell_controller.GetComponent<WallManager>().setType(spellEnum.wind);
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
                    if (spell_controller.GetComponent<Wall>() == null || spell_controller.GetComponent<WallManager>() == null) { return 0; }
                    inventory.setActiveFor(slot, 2.0f, 1.0f); //TODO: Grab active and cooldown times from spells

                    spell_controller.GetComponent<Wall>().setType(spellEnum.lightning);
                    spell_controller.GetComponent<WallManager>().setType(spellEnum.lightning);
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
                    if (spell_controller.GetComponent<CastableAOEStrike>() == null) { return 0; }
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
                        inventory.dd_spell_inventory[slot].DecreaseCharges();

                    }


                    break;
                case ItemData.SpellList.ICESTRIKE:
                    if (spell_controller.GetComponent<CastableAOEStrike>() == null) { return 0; }
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
                        inventory.dd_spell_inventory[slot].DecreaseCharges();

                    }
                    break;
                case ItemData.SpellList.WINDSTRIKE:
                    if (spell_controller.GetComponent<CastableAOEStrike>() == null) { return 0; }
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
                        inventory.dd_spell_inventory[slot].DecreaseCharges();

                    }
                    break;
                case ItemData.SpellList.LIGHTNINGSTRIKE:
                    if (spell_controller.GetComponent<CastableAOEStrike>() == null) { return 0; }
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
                        inventory.dd_spell_inventory[slot].DecreaseCharges();

                    }
                    break;
                case ItemData.SpellList.FIRESUMMON:
                    if (spell_controller.GetComponent<Summon>() == null) { return 0; }
                    spell_controller.GetComponent<Summon>().setType(spellEnum.fire);
                    spell_controller.GetComponent<Summon>().Cast();
                    inventory.dd_spell_inventory[slot].DecreaseCharges();
                    break;
                case ItemData.SpellList.ICESUMMON:
                    if (spell_controller.GetComponent<Summon>() == null) { return 0; }
                    spell_controller.GetComponent<Summon>().setType(spellEnum.ice);
                    spell_controller.GetComponent<Summon>().Cast();
                    inventory.dd_spell_inventory[slot].DecreaseCharges();
                    break;
                case ItemData.SpellList.WINDSUMMON:
                    if (spell_controller.GetComponent<Summon>() == null) { return 0; }
                    spell_controller.GetComponent<Summon>().setType(spellEnum.wind);
                    spell_controller.GetComponent<Summon>().Cast();
                    inventory.dd_spell_inventory[slot].DecreaseCharges();
                    break;
                case ItemData.SpellList.LIGHTNINGSUMMON:
                    if (spell_controller.GetComponent<Summon>() == null) { return 0; }
                    spell_controller.GetComponent<Summon>().setType(spellEnum.lightning);
                    spell_controller.GetComponent<Summon>().Cast();
                    inventory.dd_spell_inventory[slot].DecreaseCharges();
                    break;
                case ItemData.SpellList.BLACKHOLE:
                    if (spell_controller.GetComponent<SpellCastOnStaff>() == null) { return 0; }
                    spell_controller.GetComponent<SpellCastOnStaff>().Cast();
                    inventory.dd_spell_inventory[slot].DecreaseCharges();
                    break;
                case ItemData.SpellList.HEALSPELL:
                    if (spell_controller.GetComponent<Heal>() == null) { return 0; }
                    spell_controller.GetComponent<Heal>().Cast();
                    inventory.dd_spell_inventory[slot].DecreaseCharges();
                    break;
                case ItemData.SpellList.POISONCLOUD:
                    if (spell_controller.GetComponent<SpellCastOnRay>() == null) { return 0; }
                    spell_controller.GetComponent<SpellCastOnRay>().Cast();
                    inventory.dd_spell_inventory[slot].DecreaseCharges();
                    break;
                case ItemData.SpellList.POWERBEAM:
                    if (spell_controller.GetComponent<Beam>() == null) { return 0; }
                    inventory.setActiveFor(slot, 3f, 1.0f);
                    spell_controller.GetComponent<Beam>().cast();
                    inventory.dd_spell_inventory[slot].DecreaseCharges();
                    break;
                case ItemData.SpellList.FIREGRENADE:
                    if (spell_controller.GetComponent<ThrowSpell>() == null) { Debug.Log("Not Found");  return 0; }
                    if (!inventory.checkCooldown(slot)) { return 0; }
                    spell_controller.GetComponent<ThrowSpell>().Cast();
                    Debug.Log("Tryimnh");
                    //inventory.dd_spell_inventory[slot].DecreaseCharges();
                    break;
                case ItemData.SpellList.FLAMETHROWER:
                    if (spell_controller.GetComponent<Beam>() == null) { return 0; }
                    if (!inventory.checkCooldown(slot)) { return 0; }
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
