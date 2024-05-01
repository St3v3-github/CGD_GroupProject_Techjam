using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemData;

public class ItemInfo : MonoBehaviour
{
    // Start is called before the first frame update

    public ItemData item_data;
    [SerializeField] ItemData private_item_data;

    //Lists should be on the spawner, unless dropping items
    public List<Mesh> meshes = new List<Mesh>();
    public List<Material> materials = new List<Material>();

    void Start()
    {
        //fill up the lists for method 1
        /*var clone = Instantiate(private_item_data);
        item_data = clone;*/
     

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetWeight()
    {
        return 1;

    }

    public ItemData GetItemData()
    {
        return item_data;
    }

    //This is better considering dropping items, but more memory and time intensive
    public void setItemData(ItemData new_data)
    {
        item_data = new_data;
        this.gameObject.GetComponent<MeshFilter>().mesh = meshes[(int)new_data.ID];
        this.gameObject.GetComponent<Renderer>().material = materials[(int)new_data.ID];
    }

    //with this dropping items needs to go through an item spawner otherwise the mesh and material are unknown
    public void setItemData(Mesh new_mesh, Material new_material, ItemData new_data)
    {
        item_data = new_data;
        this.gameObject.GetComponent<MeshFilter>().mesh = new_mesh;
        this.gameObject.GetComponent<Renderer>().material = new_material;
    }
}
