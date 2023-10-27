using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    // Start is called before the first frame update

    public ItemData item_data;
    public List<Mesh> meshes = new List<Mesh>();
    public List<Material> element_materials = new List<Material>();
    public List<Material> type_materials = new List<Material>();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {

    }

    public int GetWeight()
    {
        return item_data.spawn_weight;
        
    }
    
    public ItemData GetItemData() { 
    
        return item_data;
    }

    public void setItemData(ItemData set_to)
    {
        item_data = set_to;
        this.gameObject.GetComponent<MeshFilter>().mesh = meshes[set_to.type];
        switch(set_to.type)
        {
            case (int)ItemData.RuneTypes.ELEMENTAL:
                this.gameObject.GetComponent<Renderer>().material = element_materials[set_to.ID];
                break;
            case (int)ItemData.RuneTypes.SPELLTYPE:
                this.gameObject.GetComponent<Renderer>().material = type_materials[set_to.ID];
                break;
            default:
                this.gameObject.GetComponent<Renderer>().material = element_materials[0];
                break;
        }
    }
}
