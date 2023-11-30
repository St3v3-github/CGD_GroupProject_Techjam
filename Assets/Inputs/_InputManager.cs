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

    [Header("Movement/Camera")]
    public Vector2 cameraInput;
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
        movementInput = ctx.ReadValue<Vector2>();

    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.action.triggered)
        {
            playerController.HandleJump();
            Debug.Log("Jumping");
        }
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.action.triggered)
        {
            //Add Script call Here
        }
    }

    public void OnSpellCast(InputAction.CallbackContext ctx)
    {
        if (ctx.action.triggered)
        {
            projectileController.MyInput();
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
    {
        if (ctx.action.triggered)
        {
            playerController.HandleMelee();
            Debug.Log("Punch");
        }
    }*/

}


