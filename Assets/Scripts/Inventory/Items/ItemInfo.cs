using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    // Start is called before the first frame update

    public ItemData item_data;
    public List<Mesh> meshes = new List<Mesh>();
    public List<Material> materials = new List<Material>();

    void Start()
    {
        //fill up mesh and material lists
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetWeight()
    {
        return item_data.spawn_weight;

    }

    public ItemData GetItemData()
    {

        return item_data;
    }

    public void setItemData(ItemData set_to)
    {
        item_data = set_to;
        this.gameObject.GetComponent<MeshFilter>().mesh = meshes[(int)set_to.ID];
        this.gameObject.GetComponent<Renderer>().material = materials[(int)set_to.ID];
    }

}
