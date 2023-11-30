using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    // REMOVED OLD LISTS - NOW USING SINGLE SPAWN LIST
  public List<GameObject> spawnList = new List<GameObject>();
    

    public enum Type {Spell, Other};
    public enum SpawnerType {UltimateOnly, RareOnly, UncommonOnly, CommonOnly, AnySpell};

    public SpawnerType spawnerType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SpawnObject(Type objectType) 
    {
        switch(objectType)
        {
            case Type.Other:
                // SPAWN OTHER THINGS HERE MAYBE (HP PICKUPS ETC)
                break;

            case Type.Spell:
                SpawnSpells();
                break;

            default: 
                break;
        }
        
    }

    public void SetSpells(List<GameObject> spellslist)
    {
        spawnList.Clear();
        spawnList = spellslist;
    }
    
    private void SpawnSpells() 
    {
        int totalWeight = 0;
        int randomNumber = 0;

        for (int i = 0; i < spawnList.Count; i++)
        {
            int itemWeight = 0;
            itemWeight = spawnList[i].GetComponent<ItemInfo>().GetWeight();
            totalWeight += itemWeight;
        }
        //UnityEngine.Debug.Log("Total Weight is: " + totalWeight);

        randomNumber = UnityEngine.Random.Range(1, totalWeight + 1);
     //   UnityEngine.Debug.Log("Spells Random number is: " + randomNumber);

        for (int i = 0; i < spawnList.Count; i++)
        {
            int itemWeight = 0;
            itemWeight = spawnList[i].GetComponent<ItemInfo>().GetWeight();

            randomNumber -= itemWeight;
            if (randomNumber < 1)
            {
                Instantiate(spawnList[i], this.transform);
                i = spawnList.Count + 1;
            }
        }
    }
}
