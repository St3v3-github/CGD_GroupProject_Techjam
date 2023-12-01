using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using static UnityEngine.EventSystems.EventTrigger;

public class InputManager : MonoBehaviour
{
    public PlayerController playerController;
    public CameraController cameraController;
    public AdvancedProjectileSystem projectileController;
    public AnimationManager animationController;

    [Header("Movement/Camera")] public Vector2 cameraInput;
    public Vector2 movementInput;

    private void Awake()
    {
        //playerController = FindObjectOfType<_PlayerController>();
        //cameraController = FindObjectOfType<_CameraController>();

        //Add subsequent finds here
    }

    private void Update()
    {
        playerController.HandleMovement(movementInput);
        playerController.HandleCamera(cameraInput);
    }


    public void OnLook(InputAction.CallbackContext ctx)
    {
        cameraInput = ctx.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        animationController.disableEmote();
        movementInput = ctx.ReadValue<Vector2>();

    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.action.triggered)
        {
            animationController.disableEmote();
            playerController.HandleJump();
            Debug.Log("Jumping");
        }
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.action.triggered)
        {
            animationController.disableEmote();
            //Add Script call Here
        }
    }

    public void OnSpellCast(InputAction.CallbackContext ctx)
    {
        animationController.disableEmote();
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
            //Cast Spell from Abilitymanager in the selected slot.
            GetComponent<AbilityManager2>().castSpell(slotTarget);
        }

    }

    //Event Action added for emoting - Harry
    public void OnDance(InputAction.CallbackContext ctx)
    {
        if (ctx.action.triggered)
        {
            animationController.toggleEmotingBool();
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
            StartCoroutine(playerController.PlayerDash(movementInput));
        }
    }
}


