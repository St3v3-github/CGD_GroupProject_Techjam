using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelector : MonoBehaviour
    {
    public GameObject Player;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void updateItemDisplay ()
    {
        Inventory playerInventory = Player.GetComponent<Inventory>();
        

        for (int i=0; i < playerInventory.spell_slots; i++)
        {
            
            //playerInventory.dd_spell_inventory[0][i].icon
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
