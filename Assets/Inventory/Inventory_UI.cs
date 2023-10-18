using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_UI : MonoBehaviour
{
    bool in_view = true;
    

    public int player_nr = 0;
    public Inventory inventory;
    public Sprite display_box_image;
    public Sprite selection_indicator;

    
    public Vector2 offset = new Vector2(0, 0);
    public Vector2 rune_slot_size = new Vector2(40, 40);
    public Vector2 spell_slot_size = new Vector2(60, 60);
    public Vector2 inv_spacing = new Vector2(50, 50);
    public Vector2 spell_spacing = new Vector2(80, 80);
    public Vector2 inv_offset = new Vector2(500, 50);
    public Vector2 spell_offset = new Vector2(50, 50);
    public Vector2 crafting_offset = new Vector2(500, 50);
    public Vector2 selection_offset = new Vector2(10, 10);
    
    public List<GameObject> inventory_displays = new();
    public List<GameObject> craft_displays = new();
    public List<GameObject> spell_displays = new();

    public List<GameObject> item_icons = new();

    GameObject canvas_object;

    bool interacting = false;
    int interactive_target = 0;
    GameObject selection_display;

    // Start is called before the first frame update
    void Start()
    {
        // Create Canvas
        canvas_object = new GameObject();
        canvas_object.name = "Canvas_" + player_nr.ToString();
        Canvas canvas_component = canvas_object.AddComponent<Canvas>();
        canvas_component.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas_object.AddComponent<CanvasScaler>();
        canvas_object.AddComponent<GraphicRaycaster>();

        selection_display = new GameObject();
        selection_display.name = "Selection_" + player_nr.ToString();
        selection_display.transform.parent = canvas_object.transform;
        RectTransform selection_transform = selection_display.AddComponent<RectTransform>();
        selection_transform.sizeDelta = new Vector2 (rune_slot_size.x+selection_offset.x*2,rune_slot_size.y+selection_offset.y*2);
        Image selector_image = selection_display.AddComponent<Image>();
        selector_image.sprite = display_box_image;
        selection_display.SetActive (false);

        int max_items = inventory.inv_height*inventory.inv_width;
        int object_number = 0;
        // Create Inventory
        for (int i = 0; i<inventory.inv_height; i++)
        {
            for(int j = 0; j<inventory.inv_width;j++)
            {
                //Init display
                inventory_displays.Add(new GameObject());
                inventory_displays[object_number].name = "InventoryDisplay_"+player_nr.ToString()+"_"+ object_number.ToString();
                inventory_displays[object_number].transform.parent = canvas_object.transform;
                RectTransform display_transform = inventory_displays[object_number].AddComponent<RectTransform>();
                display_transform.localPosition = new Vector3(inv_offset.x+inv_spacing.x*j, -inv_offset.y-inv_spacing.y*i, 0);
                display_transform.sizeDelta = rune_slot_size;
                display_transform.anchorMin = new Vector2(0, 1);
                display_transform.anchorMax = new Vector2(0, 1);
                Image display_image = inventory_displays[object_number].AddComponent<Image>();
                display_image.sprite = display_box_image;
                Inv_ItemSlot click_handling = inventory_displays[object_number].AddComponent<Inv_ItemSlot>();
                click_handling.call_back_ID = object_number;
                click_handling.UI_callback = this;
                //Init icon
                item_icons.Add(new GameObject());
                item_icons[object_number].name = "InventoryIcon_" + player_nr.ToString() + "_" + object_number.ToString();
                item_icons[object_number].transform.parent = inventory_displays[object_number].transform;
                RectTransform icon_transform = item_icons[object_number].AddComponent<RectTransform>();
                icon_transform.localPosition = new Vector3(0, 0, 0);
                icon_transform.sizeDelta = new Vector2(0, 0);
                icon_transform.anchorMin = new Vector2(0, 1);
                icon_transform.anchorMax = new Vector2(0, 1);
                icon_transform.pivot = new Vector2(0, 1);
                Image icon_image = item_icons[object_number].AddComponent<Image>();
                icon_image.sprite = null;

                //Ugly, but think this looks clearer, and has less calculations
                object_number++;
            }
        }
        object_number = 0;
        int icon_number = max_items;
        SpellData data_guide = SpellData.CreateInstance<SpellData>();
        switch(inventory.inventory_mode)
        {
            case Inventory.InventoryModes.DRAGDROP:
                //Create modular spell slots
                for (int i = 0; i < inventory.spell_slots; i++)
                {
                    for (int j = 0; j < data_guide.spell_components; j++)
                    {
                        //Init displays
                        spell_displays.Add(new GameObject());
                        spell_displays[object_number].name = "SpellDisplay_" + player_nr.ToString() + "_" + object_number.ToString();
                        spell_displays[object_number].transform.parent = canvas_object.transform;
                        RectTransform display_transform = spell_displays[object_number].AddComponent<RectTransform>();
                        display_transform.localPosition = new Vector3(spell_offset.x + inv_spacing.x * j, -spell_offset.y - inv_spacing.y * i, 0);
                        display_transform.sizeDelta = rune_slot_size;
                        display_transform.anchorMin = new Vector2(0, 1);
                        display_transform.anchorMax = new Vector2(0, 1);
                        Image display_image = spell_displays[object_number].AddComponent<Image>();
                        display_image.sprite = display_box_image;
                        //Init Icons
                        item_icons.Add(new GameObject());
                        item_icons[icon_number].name = "InventoryIcon_" + player_nr.ToString() + "_" + icon_number.ToString();
                        item_icons[icon_number].transform.parent = spell_displays[object_number].transform;
                        RectTransform icon_transform = item_icons[icon_number].AddComponent<RectTransform>();
                        icon_transform.localPosition = new Vector3(0, 0, 0);
                        icon_transform.sizeDelta = new Vector2(0, 0);
                        icon_transform.anchorMin = new Vector2(0, 1);
                        icon_transform.anchorMax = new Vector2(0, 1);
                        icon_transform.pivot = new Vector2(0, 1);
                        Image icon_image = item_icons[icon_number].AddComponent<Image>();
                        icon_image.sprite = null;
                        Inv_ItemSlot click_handling = spell_displays[object_number].AddComponent<Inv_ItemSlot>();
                        click_handling.call_back_ID = icon_number;
                        click_handling.UI_callback = this;

                        object_number++;
                        icon_number++;
                    }
                }
                //TODO: Create spell displays
                break;
                ///Focus on DD
                ///

            case Inventory.InventoryModes.CRAFT:
                //Create spell slots
                for (int i = 0; i < inventory.spell_slots; i++)
                {
                    spell_displays.Add(new GameObject());
                    spell_displays[i].name = "SpellDisplay_" + player_nr.ToString() + "_" + i.ToString();
                    spell_displays[i].transform.parent = canvas_object.transform;
                    RectTransform display_transform = spell_displays[i].AddComponent<RectTransform>();
                    display_transform.localPosition = new Vector3(spell_offset.x, -spell_offset.y - spell_spacing.y * i, 0);
                    display_transform.sizeDelta = spell_slot_size;
                    display_transform.anchorMin = new Vector2(0, 1);
                    display_transform.anchorMax = new Vector2(0, 1);
                    Image display_image = spell_displays[i].AddComponent<Image>();
                    display_image.sprite = display_box_image;
                }
                //Create crafting slots
                
                for (int j = 0; j < data_guide.spell_components; j++)
                {
                    craft_displays.Add(new GameObject());
                    craft_displays[j].name = "CraftDisplay_" + player_nr.ToString() + "_" + j.ToString();
                    craft_displays[j].transform.parent = canvas_object.transform;
                    RectTransform display_transform = craft_displays[j].AddComponent<RectTransform>();
                    display_transform.localPosition = new Vector3(crafting_offset.x + inv_spacing.x * j, crafting_offset.y, 0);
                    display_transform.sizeDelta = rune_slot_size;
                    display_transform.anchorMin = new Vector2(0, 0);
                    display_transform.anchorMax = new Vector2(0, 0);
                    Image display_image = craft_displays[j].AddComponent<Image>();
                    display_image.sprite = display_box_image;
                }
                //TODO: Create Crafting Button
                //TODO: Create Crafted Spell Display
                break;
            default:
                break;
        }
        updateInvDisplay();
    }

    public void toggleInventory()
    {
        in_view = !in_view;
        canvas_object.SetActive(in_view);
        //TODO: Change this to setting the individual 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            toggleInventory();
        }
    }

    public void updateInvDisplay()
    {
        int max_items = inventory.inv_width * inventory.inv_height;

        for (int i = 0; i < max_items; i++)
        {
            if (inventory.inventory_items[i].ID != 0)
            {
                item_icons[i].GetComponent<Image>().sprite = inventory.inventory_items[i].icon;
                item_icons[i].GetComponent<RectTransform>().sizeDelta = rune_slot_size;
            }
            else
            {
                item_icons[i].GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            }
        }

        SpellData data_guide = SpellData.CreateInstance<SpellData>();
        int target_rune_slot = 0;
        int target_icon = max_items;
        for (int i = 0; i < inventory.spell_slots; i++)
        {
            for (int j = 0; j < data_guide.spell_components; j++)
            {
                if (inventory.dd_spell_inventory[i][j].ID != 0)
                {
                    item_icons[target_icon].GetComponent<Image>().sprite = inventory.dd_spell_inventory[i][j].icon;
                    item_icons[target_icon].GetComponent<RectTransform>().sizeDelta = rune_slot_size;
                }
                else
                {
                    item_icons[target_icon].GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                }
            }
            target_rune_slot++;
            target_icon++;
        }
    }

    public void interactionCall(int caller_ID)
    {
        if (interacting)
        {
            interacting = false;
            selection_display.SetActive(false);
            if (caller_ID > inventory_displays.Count)
            {
                //Check if slot corresponds to type
                if(interactive_target > inventory_displays.Count)
                {
                    //Check if this also corresponds to type
                }
                else
                {

                }
            }
            else
            {
                if (interactive_target > inventory_displays.Count)
                {
                    //Check this too
                }
                else 
                {
                    inventory.swapItems(true, interactive_target, true, caller_ID);
                }
            }
            updateInvDisplay();
        }
        else
        {
            interactive_target = caller_ID;
            interacting = true;
            RectTransform rect = selection_display.GetComponent<RectTransform>();
            if (caller_ID > inventory_displays.Count)
            {
                RectTransform target = spell_displays[caller_ID - inventory_displays.Count].GetComponent<RectTransform>();
                rect.localPosition = new Vector3(target.localPosition.x - selection_offset.x, target.localPosition.y - selection_offset.y, target.localPosition.z);
                rect.anchorMin = target.anchorMin;
                rect.anchorMax = target.anchorMax;
            }
            else
            {
                RectTransform target = inventory_displays[caller_ID].GetComponent<RectTransform>();
                rect.localPosition = new Vector3(target.localPosition.x - selection_offset.x, target.localPosition.y - selection_offset.y, target.localPosition.z);
                rect.anchorMin = target.anchorMin;
                rect.anchorMax = target.anchorMax;
            }
            selection_display.SetActive(true);
        }
    }
}
