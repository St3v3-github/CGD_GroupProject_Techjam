using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    // Start is called before the first frame update

   public ItemData item_data;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetWeight()
    {
        return item_data.spawn_weight;
        
    }
    
    public ItemData GetItemData() { 
    
        return item_data;
    }

}
