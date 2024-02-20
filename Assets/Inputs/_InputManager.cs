using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;
using static UpdatedPlayerController;

public class InputManager : MonoBehaviour
{

    [Header("References")]
    public ComponentRegistry componentRegistry;
    private bool spell_is_held;

    [Header("Movement/Camera")] 
    public Vector2 cameraInput;
    public Vector2 movementInput = new Vector2(0,0);

    [Header("Shooting")]
    public bool shootInput;

    [Header("Wizard Element")]
    public WizardType Element;

    public enum WizardType
    {
        wind,
        fire,
        earth,
        ice,
        electric,
        plant
    }

    private void Update()
    {

        componentRegistry.playerController.HandleMovement(movementInput);
        componentRegistry.playerController.HandleCamera(cameraInput);

        componentRegistry.sliding.AssignValues(movementInput);
    }

    

    public void OnLook(InputAction.CallbackContext ctx)
    {
        cameraInput = ctx.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        //animationController.disableEmote();
        componentRegistry.animationManager.toggleEmotingBool(false);
        movementInput = ctx.ReadValue<Vector2>();

    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if(Element == WizardType.fire || Element == WizardType.ice || Element == WizardType.plant || Element == WizardType.electric) 
        {
            if (ctx.action.triggered && componentRegistry.playerController.isGrounded && componentRegistry.playerController.readyToJump)
            {
                //animationController.disableEmote();
                //animationController.toggleEmotingBool(false);
                //playerController.HandleJump();



                componentRegistry.playerController.HandleJump();
            }
        }
        

        if(Element == WizardType.wind)
        {
            if (ctx.action.triggered && componentRegistry.playerController.isGrounded && componentRegistry.playerController.readyToJump)
            {
                //animationController.disableEmote();
                //animationController.toggleEmotingBool(false);
                //playerController.HandleJump();



                componentRegistry.playerController.HandleJump();
            }
            else if(ctx.action.triggered && !componentRegistry.playerController.hasDoubleJumped)
            {
                componentRegistry.playerController.HandleJump();
            }
        }

        

        if(Element == WizardType.earth)
        {
            if(ctx.performed && componentRegistry.playerController.isGrounded && componentRegistry.playerController.readyToJump)
            {
                componentRegistry.playerController.chargingJump = true;
                Debug.Log(componentRegistry.playerController.chargingJump);
            }

            if(ctx.canceled && componentRegistry.playerController.isGrounded && componentRegistry.playerController.readyToJump)
            {
                componentRegistry.playerController.chargingJump = false;
                componentRegistry.playerController.HandleJump();
            }
        }
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.action.triggered)
        {
            componentRegistry.playerController.sprintPressed = !componentRegistry.playerController.sprintPressed;
        }
    }

    public void OnPrimaryCast(InputAction.CallbackContext ctx)
    {
        if (componentRegistry.advancedProjectileSystem != null)
        {
            if (componentRegistry.advancedProjectileSystem.equippedProjectile.allowButtonHold)
            {
                if (ctx.action.triggered)
                {
                    componentRegistry.advancedProjectileSystem.shooting = true;
                }
                if (ctx.action.WasReleasedThisFrame())
                {
                    componentRegistry.advancedProjectileSystem.shooting = false;
                }
            }
            else
            {
                if (ctx.action.triggered)
                {
                    componentRegistry.advancedProjectileSystem.shooting = true;
                }
            }
        }


    }

    public void OnCrouch(InputAction.CallbackContext ctx)
    {
        if (ctx.action.triggered)
        {
            componentRegistry.playerController.crouchPressed = !componentRegistry.playerController.crouchPressed;
        }

        
    }

    public void OnAction(InputAction.CallbackContext ctx)
    {
        if(Element == WizardType.earth)
        {
            if (ctx.action.triggered)
            {
                componentRegistry.playerController.HandlePound();
            }
        }

        if (Element == WizardType.fire)
        {
            if (ctx.performed)
            {
                componentRegistry.jetPack.usingJetpack = true;
            }
            else if (ctx.canceled)
            {
                componentRegistry.jetPack.usingJetpack = false;
            }
        }

        if (Element == WizardType.ice)
        {
            if (ctx.performed && (movementInput.x != 0 || movementInput.y != 0))
            {
                componentRegistry.sliding.StartSlide();
            }
            else if (ctx.canceled && componentRegistry.playerController.sliding)
            {
                componentRegistry.sliding.EndSlide();
            }
        }
        
        if(Element == WizardType.plant)
        {
            if (ctx.performed)
            {
                componentRegistry.grappling.StartGrapple();
                //grappleSwing.StartSwing();
            }
            else if (ctx.canceled)
            {
                //grappleSwing.StopSwing();
            }
        }

        if(Element == WizardType.electric)
        {
            if (ctx.action.triggered)
            {
                componentRegistry.dashing.Dash();
            }
        }
       

    }

    public void OnSpellCast(InputAction.CallbackContext ctx)
    {
        //animationController.disableEmote();
        componentRegistry.animationManager.toggleEmotingBool(false);
        if (ctx.action.triggered)
        {
            //Determining Spell Slot
            InputControl actionInput = ctx.control;
            string actionButton = actionInput.name;
            int slotTarget = 0;
            switch (actionButton)
            {
                case "1":
                    slotTarget = 0;
                    break;
                case "2":
                    slotTarget = 1;
                    break;
                case "3":
                    slotTarget = 2;
                    break;
                case "4":
                    slotTarget = 3;
                    break;
                case "leftTrigger":
                    slotTarget = 0;
                    break;
                case "leftBumper":

                    slotTarget = 1;
                    break;
                case "leftShoulder":

                    slotTarget = 1;
                    break;
                case "rightTrigger":
                    return;
                    slotTarget = 2;
                    break;
                case "rightBumper":

                    slotTarget = 3;
                    break;
                case "rightShoulder":

                    slotTarget = 3;
                    break;
            }
            // Set item to inventory if looking at a pickup.
            if (componentRegistry.ray.target != null && componentRegistry.ray.target.GetComponent<ItemInfo>().GetItemData().type != ItemData.SpellType.EMPTY && SlotCheck(componentRegistry.ray.target.GetComponent<ItemInfo>().GetItemData(), slotTarget))
            {
                ItemData unequipped = componentRegistry.inventory.equipFromWorld(componentRegistry.ray.target.GetComponent<ItemInfo>().GetItemData(), slotTarget);
                componentRegistry.ray.target.GetComponent<ItemScript>().Interact();
            }
            //Cast Spell from Abilitymanager in the selected slot.
            else
            {
                GetComponent<AbilityManager2>().castSpell(slotTarget,ctx);
                //updatedPlayerController.animControl.toggleCastingBool(true);
            }
            

        }
    }

    //Event Action added for emoting - Harry
    public void OnDance(InputAction.CallbackContext ctx)
    {
        if (ctx.action.triggered)
        {
            componentRegistry.animationManager.toggleEmotingBool(true);
        }
    }


    /*
    public void OnMelee(InputAction.CallbackContext ctx)
        /*
        public void OnMelee(InputAction.CallbackContext ctx)
        {
            if (ctx.action.triggered)
            {
                playerController.HandleMelee();
                Debug.Log("Punch");
            }
        }*/

    public void OnDash(InputAction.CallbackContext ctx)
    {
        
    }

    private bool SlotCheck(ItemData itemData, int slot)
    {
        if(slot == 2 && itemData.slot == ItemData.SlotType.BASIC)
        {
            return true;
        }
        else if(slot != 2 && itemData.slot != ItemData.SlotType.BASIC)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    
}


