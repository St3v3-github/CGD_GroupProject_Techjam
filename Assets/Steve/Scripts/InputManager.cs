using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Movement/Camera")]
    public Vector2 cameraInput;
    public Vector2 movementInput;
    public bool jumpInput = false;
    public bool sprintInput = false;

    [Header("Abilities")]
    public bool abilityCastInput = false;
    public bool abilityInput1 = false;
    public bool abilityInput2 = false;
    public bool abilityInput3 = false;
    public bool abilityInput4 = false;

    [Header("Other")]
    public bool interactInput = false;


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
        jumpInput = ctx.action.triggered;
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        sprintInput = ctx.action.triggered;
    }

    public void OnAbilityCast(InputAction.CallbackContext ctx)
    {
        abilityCastInput = ctx.action.triggered;
    }

    public void OnChooseAbility1(InputAction.CallbackContext ctx)
    {
        abilityInput1 = ctx.action.triggered;
    }

    public void OnChooseAbility2(InputAction.CallbackContext ctx)
    {
        abilityInput2 = ctx.action.triggered;
    }

    public void OnChooseAbility3(InputAction.CallbackContext ctx)
    {
        abilityInput3 = ctx.action.triggered;
    }

    public void OnChooseAbility4(InputAction.CallbackContext ctx)
    {
        abilityInput4 = ctx.action.triggered;
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        interactInput = ctx.action.triggered;
    }

}
