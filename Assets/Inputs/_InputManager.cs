using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using static UnityEngine.EventSystems.EventTrigger;
using static UpdatedPlayerController;

public class InputManager : MonoBehaviour
{
    private PlayerControlsAsset playercontrols;
    public PlayerController playerController;
    public UpdatedPlayerController updatedPlayerController;
    public Sliding sliding;
    public Dashing dashing;
    public JetPack jetPack;
    public CameraController cameraController;
    public AdvancedProjectileSystem projectileController;
    public AnimationManager animationController;
    public Grappling grappling;
    public Raycast ray;
    private InventoryEdit inventory;
    private bool spell_is_held;

    [Header("Movement/Camera")] 
    public Vector2 cameraInput;
    public Vector2 movementInput;

    

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
        if (ctx.action.triggered && updatedPlayerController.isGrounded && updatedPlayerController.readyToJump)
        {
            //animationController.disableEmote();
            //animationController.toggleEmotingBool(false);
            //playerController.HandleJump();



            updatedPlayerController.HandleJump();
        }
        /*else if(ctx.action.triggered && !updatedPlayerController.hasDoubleJumped)
        {
            updatedPlayerController.HandleJump();
        }*/
        if(ctx.performed)
        {
            jetPack.usingJetpack = true;
        }
        else if(ctx.canceled)
        {
            jetPack.usingJetpack = false;
        }
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.action.triggered)
        {
            updatedPlayerController.sprintPressed = !updatedPlayerController.sprintPressed;
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
        if(ctx.performed)
        {
            grappling.StartGrapple();
        }
        else if(ctx.canceled)
        {

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
            }
            

        }
    }

    //Event Action added for emoting - Harry
    public void OnDance(InputAction.CallbackContext ctx)
    {
        if (ctx.action.triggered)
        {
            animationController.toggleEmotingBool(true);
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


