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
    public Raycast ray;
    public Inventory inventory;

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
        jumpInput = ctx.action.triggered;
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

    public void OnElement(InputAction.CallbackContext ctx)
    {
        if (ctx.action.WasReleasedThisFrame())
        {
            InputControl actionInput = ctx.control;

            if (ray.target != null && ray.target.GetComponent<ItemInfo>().GetItemData().type == 0)
            {
                int slotTarget = 0;
                switch (actionInput.name)
                {
                    case "1":
                        slotTarget = 0;
                        Debug.Log("Button 1 was pressed, setting slot 0.");
                        break;
                    case "2":
                        slotTarget = 1;
                        Debug.Log("Button 2 was pressed, setting slot 1.");
                        break;
                    case "3":
                        slotTarget = 2;
                        Debug.Log("Button 3 was pressed, setting slot 2.");
                        break;
                    case "4":
                        slotTarget = 3;
                        Debug.Log("Button 4 was pressed, setting slot 3.");
                        break;
                    case "left":
                        slotTarget = 0;
                        Debug.Log("Button left was pressed, setting slot 0.");
                        break;
                    case "up":
                        slotTarget = 1;
                        Debug.Log("Button up was pressed, setting slot 1.");
                        break;
                    case "right":
                        slotTarget = 2;
                        Debug.Log("Button right was pressed, setting slot 2.");
                        break;
                    case "down":
                        slotTarget = 3;
                        Debug.Log("Button down was pressed, setting slot 3.");
                        break;
                }

                ItemData unequipped = inventory.equipFromWorld(ray.target.GetComponent<ItemInfo>().GetItemData(), slotTarget);
                ray.target.GetComponent<ItemScript>().Interact();
                if (unequipped.ID != 0)
                {
                    //create a dropped item here
                }

            }

            else
            {
                //switch element on UI and maybe ability manager?
            }


        }

    }

    public void OnSpellRune(InputAction.CallbackContext ctx)
    {
        if (ctx.action.WasReleasedThisFrame())
        {
            InputControl actionInput = ctx.control;
            string actionButton = actionInput.name;

            if (ray.target != null && ray.target.GetComponent<ItemInfo>().GetItemData().type == 1)
            {
                int slotTarget = 0;
                switch (actionButton)
                {
                    case "5":
                        slotTarget = 0;
                        break;
                    case "6":
                        slotTarget = 1;
                        break;
                    case "7":
                        slotTarget = 2;
                        break;
                    case "8":
                        slotTarget = 3;
                        break;
                    case "leftTrigger":
                        slotTarget = 0;
                        break;
                    case "leftBumper":
                        slotTarget = 1;
                        break;
                    case "rightTrigger":
                        slotTarget = 2;
                        break;
                    case "rightBumper":
                        slotTarget = 3;
                        break;
                }


                ItemData unequipped = inventory.equipFromWorld(ray.target.GetComponent<ItemInfo>().GetItemData(), slotTarget);
                ray.target.GetComponent<ItemScript>().Interact();
                if (unequipped.ID != 0)
                {
                    //create a dropped item here
                }
            }
            else
            {
                //call for casting spell
            }
        }

    }

}
