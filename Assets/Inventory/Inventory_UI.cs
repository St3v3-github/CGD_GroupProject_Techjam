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

    
    public Vector2 offset = new Vector2(0, 0);
    public Vector2 rune_slot_size = new Vector2(40, 40);
    public Vector2 spell_slot_size = new Vector2(60, 60);
    public Vector2 inv_spacing = new Vector2(50, 50);
    public Vector2 spell_spacing = new Vector2(80, 80);
    public Vector2 inv_offset = new Vector2(500, 50);
    public Vector2 spell_offset = new Vector2(50, 50);
    public Vector2 crafting_offset = new Vector2(500, 50);
    
    public List<GameObject> inventory_displays = new();
    public List<GameObject> craft_displays = new();
    public List<GameObject> spell_displays = new();

    GameObject canvas_object;

    // Start is called before the first frame update
    void Start()
    {
        // Create Canvas
        canvas_object = new GameObject();
        canvas_object.name = "Canvas " + player_nr.ToString();
        Canvas canvas_component = canvas_object.AddComponent<Canvas>();
        canvas_component.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas_object.AddComponent<CanvasScaler>();
        canvas_object.AddComponent<GraphicRaycaster>();

        // Create Inventory
        for(int i = 0; i<inventory.inv_height; i++)
        {
            for(int j = 0; j<inventory.inv_width;j++)
            {
                inventory_displays.Add(new GameObject());
                int object_number = i * inventory.inv_width + j;
                inventory_displays[object_number].name = "InventoryDisplay_"+player_nr.ToString()+"_"+ object_number.ToString();
                inventory_displays[object_number].transform.parent = canvas_object.transform;
                RectTransform display_transform = inventory_displays[object_number].AddComponent<RectTransform>();
                display_transform.localPosition = new Vector3(inv_offset.x+inv_spacing.x*j, -inv_offset.y-inv_spacing.y*i, 0);
                display_transform.sizeDelta = rune_slot_size;
                display_transform.anchorMin = new Vector2(0, 1);
                display_transform.anchorMax = new Vector2(0, 1);
                Image display_image = inventory_displays[object_number].AddComponent<Image>();
                display_image.sprite = display_box_image;
            }
        }

        SpellData data_guide = SpellData.CreateInstance<SpellData>();
        switch(inventory.inventory_mode)
        {
            case Inventory.InventoryModes.DRAGDROP:
                //Create modular spell slots
                for (int i = 0; i < inventory.spell_slots; i++)
                {
                    for (int j = 0; j < data_guide.spell_components; j++)
                    {
                        spell_displays.Add(new GameObject());
                        int object_number = i * data_guide.spell_components + j;
                        spell_displays[object_number].name = "SpellDisplay_" + player_nr.ToString() + "_" + object_number.ToString();
                        spell_displays[object_number].transform.parent = canvas_object.transform;
                        RectTransform display_transform = spell_displays[object_number].AddComponent<RectTransform>();
                        display_transform.localPosition = new Vector3(spell_offset.x + inv_spacing.x * j, -spell_offset.y - inv_spacing.y * i, 0);
                        display_transform.sizeDelta = rune_slot_size;
                        display_transform.anchorMin = new Vector2(0, 1);
                        display_transform.anchorMax = new Vector2(0, 1);
                        Image display_image = spell_displays[object_number].AddComponent<Image>();
                        display_image.sprite = display_box_image;
                    }
                }
                //TODO: Create spell displays
                break;
            case Inventory.InventoryModes.CRAFT:
                //Create spell slots
                for (int i = 0; i < inventory.spell_slots; i++)
                {
                    spell_displays.Add(new GameObject());
                    int object_number = i;
                    spell_displays[object_number].name = "SpellDisplay_" + player_nr.ToString() + "_" + object_number.ToString();
                    spell_displays[object_number].transform.parent = canvas_object.transform;
                    RectTransform display_transform = spell_displays[object_number].AddComponent<RectTransform>();
                    display_transform.localPosition = new Vector3(spell_offset.x, -spell_offset.y - spell_spacing.y * i, 0);
                    display_transform.sizeDelta = spell_slot_size;
                    display_transform.anchorMin = new Vector2(0, 1);
                    display_transform.anchorMax = new Vector2(0, 1);
                    Image display_image = spell_displays[object_number].AddComponent<Image>();
                    display_image.sprite = display_box_image;
                }
                //Create crafting slots
                for (int j = 0; j < data_guide.spell_components; j++)
                {
                    craft_displays.Add(new GameObject());
                    int object_number = j;
                    craft_displays[object_number].name = "CraftDisplay_" + player_nr.ToString() + "_" + object_number.ToString();
                    craft_displays[object_number].transform.parent = canvas_object.transform;
                    RectTransform display_transform = craft_displays[object_number].AddComponent<RectTransform>();
                    display_transform.localPosition = new Vector3(crafting_offset.x + inv_spacing.x * j, crafting_offset.y, 0);
                    display_transform.sizeDelta = rune_slot_size;
                    display_transform.anchorMin = new Vector2(0, 0);
                    display_transform.anchorMax = new Vector2(0, 0);
                    Image display_image = craft_displays[object_number].AddComponent<Image>();
                    display_image.sprite = display_box_image;
                }
                //TODO: Create Crafting Button
                //TODO: Create Crafted Spell Display
                break;
            default:
                break;
        }
    }

    public void toggleInventory()
    {
        in_view = !in_view;
        canvas_object.SetActive(in_view);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            toggleInventory();
        }
    }
}
