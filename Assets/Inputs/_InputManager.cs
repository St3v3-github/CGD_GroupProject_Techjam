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
    private PlayerControlsAsset playercontrols;
    public AdvancedProjectileSystem advancedProjectileSystem;
    public UpdatedPlayerController updatedPlayerController;
    public Sliding sliding;
    public Dashing dashing;
    public JetPack jetPack;
    public UpdatedCameraController cameraController;
    public AdvancedProjectileSystem projectileController;
    public AnimationManager animationController;
    public Grappling grappling;
    public GrappleSwing grappleSwing;
    public Raycast ray;

    public SpellManagerTemplate spellManagerTemplate;

    private InventoryEdit inventory;
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



    private void Awake()
    {
       
    
        cameraController = FindObjectOfType<UpdatedCameraController>();

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
        if(Element == WizardType.fire || Element == WizardType.ice || Element == WizardType.plant || Element == WizardType.electric) 
        {
            if (ctx.action.triggered && updatedPlayerController.isGrounded && updatedPlayerController.readyToJump)
            {
                //animationController.disableEmote();
                //animationController.toggleEmotingBool(false);
                //playerController.HandleJump();



                updatedPlayerController.HandleJump();
            }
        }
        

        if(Element == WizardType.wind)
        {
            if (ctx.action.triggered && updatedPlayerController.isGrounded && updatedPlayerController.readyToJump)
            {
                //animationController.disableEmote();
                //animationController.toggleEmotingBool(false);
                //playerController.HandleJump();



                updatedPlayerController.HandleJump();
            }
            else if(ctx.action.triggered && !updatedPlayerController.hasDoubleJumped)
            {
                updatedPlayerController.HandleJump();
            }
        }

        

        if(Element == WizardType.earth)
        {
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
            updatedPlayerController.crouchPressed = !updatedPlayerController.crouchPressed;
        }

        
    }

    public void OnAction(InputAction.CallbackContext ctx)
    {
        if(Element == WizardType.earth)
        {
            if (ctx.action.triggered)
            {
                updatedPlayerController.HandlePound();
            }
        }

        if (Element == WizardType.fire)
        {
            if (ctx.performed)
            {
                jetPack.usingJetpack = true;
            }
            else if (ctx.canceled)
            {
                jetPack.usingJetpack = false;
            }
        }

        if (Element == WizardType.ice)
        {
            if (ctx.performed && (movementInput.x != 0 || movementInput.y != 0))
            {
                sliding.StartSlide();
            }
            else if (ctx.canceled && updatedPlayerController.sliding)
            {
                sliding.EndSlide();
            }
        }
        
        if(Element == WizardType.plant)
        {
            if (ctx.performed)
            {
                grappling.StartGrapple();
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
                dashing.Dash();
            }
        }

    }

    public void OnSpellSlot1(InputAction.CallbackContext ctx)
    {
        animationController.toggleEmotingBool(false);
        if (ctx.action.triggered)
            spellManagerTemplate.Cast(0);
    } 
    
    public void OnSpellSlot2(InputAction.CallbackContext ctx)
    {
        animationController.toggleEmotingBool(false);
        if (ctx.action.triggered)
            spellManagerTemplate.Cast(1);
    }
    
    public void OnSpellSlot3(InputAction.CallbackContext ctx)
    {
        animationController.toggleEmotingBool(false);
        if (ctx.action.triggered)
            spellManagerTemplate.Cast(2);
    } 
    
    public void OnSpellSlot4(InputAction.CallbackContext ctx)
    {
        animationController.toggleEmotingBool(false);
        if (ctx.action.triggered)
            spellManagerTemplate.Cast(3);
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
        
    }

    

    
}


