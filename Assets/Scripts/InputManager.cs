using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public Vector2 cameraInput;
    public Vector2 movementInput;
    public bool jumpInput = false;
    public Raycast ray;
    public Inventory inventory;

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

    public void OnElement(InputAction.CallbackContext ctx)
    {
        if(ray.target != null) 
        {
            //change based on which slot was pressed
            ItemData unequipped = inventory.equipFromWorld(ray.target.GetComponent<ItemData>(), 0/*change this*/);
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

    public void OnSpellRune(InputAction.CallbackContext ctx)
    {
        if(ray.target != null) 
        {
            //change based on which slot was pressed
            ItemData unequipped = inventory.equipFromWorld(ray.target.GetComponent<ItemData>(), 0/*change this*/);
            if(unequipped.ID!=0)
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
