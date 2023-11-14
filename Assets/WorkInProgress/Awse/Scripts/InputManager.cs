using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.EventTrigger;

public class InputManager : MonoBehaviour
{
    PlayerController playerController;
    CameraController cameraController;

    [Header("Movement/Camera")]
    public Vector2 cameraInput;
    public Vector2 movementInput;

    [Header("Abilities")]
    public bool abilityCastInput = false;
    public bool abilityInput1 = false;
    public bool abilityInput2 = false;
    public bool abilityInput3 = false;
    public bool abilityInput4 = false;
    public Raycast ray;
    public InventoryEdit inventory;

    [Header("Other")]
    public bool interactInput = false;
    public bool meleeInput = false;
    public GameObject spell_holder;

    [Header("DEMO_DELETE_LATER")]
    public GameObject uiHandler;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        //cameraController = FindObjectOfType<CameraController>();

        //Add subsequent scripts here
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
        }
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.action.triggered)
        {
            playerController.HandleSprint();
        }
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

    public void OnMelee(InputAction.CallbackContext ctx)
    {
        meleeInput = ctx.action.triggered;
    }

    /*public void OnElement(InputAction.CallbackContext ctx)
    {
        if (ctx.action.WasReleasedThisFrame())
        {
            InputControl actionInput = ctx.control;
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

            if (ray.target != null && ray.target.GetComponent<ItemInfo>().GetItemData().type == 0)
            {
                ItemData unequipped = inventory.equipFromWorld(ray.target.GetComponent<ItemInfo>().GetItemData(), slotTarget);
                ray.target.GetComponent<ItemScript>().Interact();


                switch (ray.target.GetComponent<ItemInfo>().GetItemData().ID)
                {
                    case 1:
                        uiHandler.GetComponent<DemoUI>().EnableFire();
                        break;
                    case 2:
                        uiHandler.GetComponent<DemoUI>().EnableIce();
                        break;
                    case 3:
                        uiHandler.GetComponent<DemoUI>().EnableElectric();
                        break;


                }
                if (unequipped.ID != 0)
                {

                    //create a dropped item here
                }

            }

            else
            {

                //switch element on UI and maybe ability manager?
                if (inventory.setElementSelection(slotTarget))
                {
                    //update UI
                }
                else
                {
                    //You tried to select an empty slot you dimwit
                }
            }


        }

    }*/

    public void OnSpellRune(InputAction.CallbackContext ctx)
    {
        if (ctx.action.WasReleasedThisFrame())
        {
            InputControl actionInput = ctx.control;
            string actionButton = actionInput.name;
            int slotTarget = 0;
            //Gets spell slot from button press.
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
            //CHECK FOR LOOKING AT A VALID ITEM
            if (ray.target != null && ray.target.GetComponent<ItemInfo>().GetItemData().type == 1)
            {
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
                //inventory.getSelectedElement(); //-> Gives Element ID
                //inventory.getSpellType(slotTarget); //-> Gives Spell Type ID


                switch (inventory.getSpellData(slotTarget))
                {
                    case 1:

                        break;
                    case 2:  // ICE WALL
                        if (spell_holder.GetComponent<Wall>().isPlacingWall)
                        {
                            spell_holder.GetComponent<Wall>().PlaceWall();
                        }
                        else
                        {
                            spell_holder.GetComponent<Wall>().StartPlacingWall();
                        }
                        break;
                    case 3:

                        break;
                    case 4:
                        break;
                    case 5: // FIREBALL
                        spell_holder.GetComponent<FireProjectile>().Fire();
                        break;
                    case 6:
                        break;
                    case 7:
                        break;
                    case 8:
                        break;
                    case 9:
                        break;
                    case 10:
                        break;
                    case 11:
                        break;
                    case 12: // LIGHTNING STRIKE
                        Vector3 centre = spell_holder.GetComponent<CastableAOEStrike>().GetMouseWorldPosition();
                        spell_holder.GetComponent<CastableAOEStrike>().Strike(centre);
                        //spell_holder.GetComponent<CastableAOEStrike>().DetectCharacters(centre);
                        break;
                    case 13: // FIRE SUMMON
                        spell_holder.GetComponent<Summon>().Spawn();
                        break;
                    case 14:
                        break;
                    case 15:
                        break;
                    case 16:
                        break;

                }


            }
        }
    }
}


