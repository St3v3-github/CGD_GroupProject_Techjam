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
    public SpellManagerTemplate spellManagerTemplate;
    private bool spell_is_held;

    [Header("Movement/Camera")]
    public Vector2 cameraInput;
    public Vector2 movementInput = new Vector2(0, 0);

    [Header("Shooting")]
    public bool shootInput;
    private bool firingSlot1;
    private bool firingSlot2;
    private bool firingSlot3;
    private bool firingSlot4;


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

    private void FixedUpdate()
    {
        if (firingSlot1)
        {
            spellManagerTemplate.Cast(0);
        }

        if (firingSlot2)
        {
            spellManagerTemplate.Cast(1);
        }


        if (firingSlot3)
        {
            spellManagerTemplate.Cast(2);
        }

        if (firingSlot4)
        {
            spellManagerTemplate.Cast(3);
        }

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
        if (Element == WizardType.fire || Element == WizardType.ice || Element == WizardType.plant || Element == WizardType.electric)
        {
            if (ctx.action.triggered && componentRegistry.playerController.isGrounded && componentRegistry.playerController.readyToJump)
            {
                //animationController.disableEmote();
                //animationController.toggleEmotingBool(false);
                //playerController.HandleJump();

                componentRegistry.playerController.HandleJump();
            }
        }


        if (Element == WizardType.wind)
        {
            if (ctx.action.triggered && componentRegistry.playerController.isGrounded && componentRegistry.playerController.readyToJump)
            {
                //animationController.disableEmote();
                //animationController.toggleEmotingBool(false);
                //playerController.HandleJump();



                componentRegistry.playerController.HandleJump();
            }
            else if (ctx.action.triggered && !componentRegistry.playerController.hasDoubleJumped)
            {
                componentRegistry.playerController.HandleJump();
            }
        }



        if (Element == WizardType.earth)
        {
            if (ctx.performed && componentRegistry.playerController.isGrounded && componentRegistry.playerController.readyToJump)
            {
                componentRegistry.playerController.chargingJump = true;
                Debug.Log(componentRegistry.playerController.chargingJump);
            }

            if (ctx.canceled && componentRegistry.playerController.isGrounded && componentRegistry.playerController.readyToJump)
            {
                componentRegistry.playerController.chargingJump = false;
                componentRegistry.playerController.HandleJump();
            }
        }

        componentRegistry.gamepadRumbleController.StartRumble(componentRegistry.playerInput.playerIndex, 0.1f, 5.0f, 1.0f);
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.action.triggered)
        {
            componentRegistry.playerController.sprintPressed = !componentRegistry.playerController.sprintPressed;
        }
    }

    public void OnCrouch(InputAction.CallbackContext ctx)
    {
        if (ctx.action.triggered && componentRegistry.playerController.isGrounded)
        {
            componentRegistry.playerController.crouchPressed = !componentRegistry.playerController.crouchPressed;
        }
    }

    public void OnAction(InputAction.CallbackContext ctx)
    {
        if (Element == WizardType.earth)
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

        if (Element == WizardType.plant)
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

        if (Element == WizardType.electric)
        {
            if (ctx.action.triggered)
            {
                componentRegistry.dashing.Dash();
            }
        }

    }

    public void OnSpellSlot1(InputAction.CallbackContext ctx)
    {
        componentRegistry.animationManager.toggleEmotingBool(false);

        if (ctx.action.triggered)
        {
            firingSlot1 = true;
        }

        else if (ctx.action.WasReleasedThisFrame())
        {
            firingSlot1 = false;
        }

    }

    public void OnSpellSlot2(InputAction.CallbackContext ctx)
    {
        componentRegistry.animationManager.toggleEmotingBool(false);

        if (ctx.action.triggered)
        {
            firingSlot2 = true;
        }

        else if (ctx.action.WasReleasedThisFrame())
        {
            firingSlot2 = false;
        }

    }

    public void OnSpellSlot3(InputAction.CallbackContext ctx)
    {
        componentRegistry.animationManager.toggleEmotingBool(false);

        if (ctx.action.triggered)
        {
            firingSlot3 = true;
        }

        else if (ctx.action.WasReleasedThisFrame())
        {
            firingSlot3 = false;
        }

    }

    public void OnSpellSlot4(InputAction.CallbackContext ctx)
    {
        componentRegistry.animationManager.toggleEmotingBool(false);

        if (ctx.action.triggered)
        {
            firingSlot4 = true;
        }

        else if (ctx.action.WasReleasedThisFrame())
        {
            firingSlot4 = false;
        }

    }

    public void OnSelect(InputAction.CallbackContext ctx)
    {
        componentRegistry.uiController.SwitchTabInstructions();

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

}


