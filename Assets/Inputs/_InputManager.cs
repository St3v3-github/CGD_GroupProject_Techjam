using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;
using static UpdatedPlayerController;

public class InputManager : MonoBehaviour
{
    private PlayerControlsAsset playercontrols;
    public PlayerController playerController;
    public AdvancedProjectileSystem advancedProjectileSystem;
    public UpdatedPlayerController updatedPlayerController;
    public Sliding sliding;
    public Dashing dashing;
    public JetPack jetPack;
    public CameraController cameraController;
    public AdvancedProjectileSystem projectileController;
    public AnimationManager animationController;
    public Grappling grappling;
    public GrappleSwing grappleSwing;
    public Raycast ray;
    private InventoryEdit inventory;
    private bool spell_is_held;

    [Header("Movement/Camera")] 
    public Vector2 cameraInput;
    public Vector2 movementInput;

    [Header("Shooting")]
    public bool shootInput;

    

    private void Awake()
    {
       
    
        //playerController = FindObjectOfType<_PlayerController>();
        //cameraController = FindObjectOfType<_CameraController>();

        //Add subsequent finds here
        inventory = GetComponent<InventoryEdit>();
    }

    private void Update()
    {
       
        updatedPlayerController.HandleMovement(movementInput);
        updatedPlayerController.HandleCamera(cameraInput);
        
        sliding.AssignValues(movementInput);
    }

    

    public void OnLook(InputAction.CallbackContext ctx)
    {
        cameraInput = ctx.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        //animationController.disableEmote();
        animationController.toggleEmotingBool(false);
        movementInput = ctx.ReadValue<Vector2>();

    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        /*if (ctx.action.triggered && updatedPlayerController.isGrounded && updatedPlayerController.readyToJump)
        {
            //animationController.disableEmote();
            //animationController.toggleEmotingBool(false);
            //playerController.HandleJump();



            updatedPlayerController.HandleJump();
        }
        *//*else if(ctx.action.triggered && !updatedPlayerController.hasDoubleJumped)
        {
            updatedPlayerController.HandleJump();
        }*//*
        if(ctx.performed)
        {
            jetPack.usingJetpack = true;
        }
        else if(ctx.canceled)
        {
            jetPack.usingJetpack = false;
        }*/

        if(ctx.performed && updatedPlayerController.isGrounded && updatedPlayerController.readyToJump)
        {
            updatedPlayerController.chargingJump = true;
            Debug.Log(updatedPlayerController.chargingJump);
        }

        if(ctx.canceled && updatedPlayerController.isGrounded && updatedPlayerController.readyToJump)
        {
            updatedPlayerController.chargingJump = false;
            updatedPlayerController.HandleJump();
        }
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.action.triggered)
        {
            updatedPlayerController.sprintPressed = !updatedPlayerController.sprintPressed;
        }
    }

    public void OnPrimaryCast(InputAction.CallbackContext ctx)
    {
        if (advancedProjectileSystem != null)
        {
            if (advancedProjectileSystem.equippedProjectile.allowButtonHold)
            {
                if (ctx.action.triggered)
                {
                    advancedProjectileSystem.shooting = true;
                }
                if (ctx.action.WasReleasedThisFrame())
                {
                    advancedProjectileSystem.shooting = false;
                }
            }
            else
            {
                if (ctx.action.triggered)
                {
                    advancedProjectileSystem.shooting = true;
                }
            }
        }


    }

    public void OnCrouch(InputAction.CallbackContext ctx)
    {
        if (ctx.action.triggered)
        {
            //updatedPlayerController.crouchPressed = !updatedPlayerController.crouchPressed;
        }

        if(ctx.performed && (movementInput.x != 0 || movementInput.y != 0))
        {
            sliding.StartSlide();
        }
        else if(ctx.canceled && updatedPlayerController.sliding)
        {
            sliding.EndSlide();
        }
    }

    public void OnAction(InputAction.CallbackContext ctx)
    {
        if (ctx.action.triggered)
        {
            updatedPlayerController.HandlePound();
        }

        if (ctx.performed)
        {
            //grappling.StartGrapple();
            //grappleSwing.StartSwing();
            //updatedPlayerController.HandlePound();
        }
        else if(ctx.canceled)
        {
            //grappleSwing.StopSwing();
        }
    }

    public void OnSpellCast(InputAction.CallbackContext ctx)
    {
        //animationController.disableEmote();
        animationController.toggleEmotingBool(false);
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
            if (ray.target != null && ray.target.GetComponent<ItemInfo>().GetItemData().type != ItemData.SpellType.EMPTY && SlotCheck(ray.target.GetComponent<ItemInfo>().GetItemData(), slotTarget))
            {
                ItemData unequipped = inventory.equipFromWorld(ray.target.GetComponent<ItemInfo>().GetItemData(), slotTarget);
                ray.target.GetComponent<ItemScript>().Interact();
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
            updatedPlayerController.animControl.toggleEmotingBool(true);
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
        if (ctx.action.triggered)
        {
            dashing.Dash();
        }
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


