using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextUpdater : MonoBehaviour
{
    public TextMeshProUGUI spelltext;
    public TextMeshProUGUI chargetext;
    public int slot;
    public InventoryEdit playerInventory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (playerInventory.dd_spell_inventory[slot].uses_left > 0)
        {
            spelltext.text = playerInventory.dd_spell_inventory[slot].item_name;
            chargetext.text = playerInventory.dd_spell_inventory[slot].uses_left.ToString();
        }
        else
        {
            spelltext.text = "EMPTY";
            chargetext.text = 0.ToString();
        }*/
     
        
    }
}
