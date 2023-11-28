using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    [SerializeField] List<GameObject> elements = new List<GameObject>();
    [SerializeField] List<GameObject> spells = new List<GameObject>();

    public enum Type {Element, Spell};

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Object type:
    // 0 = Elements
    // 1 = Spells

    public void SpawnObject(Type objectType) 
    {
        switch(objectType)
        {
            //case Type.Element:
                //SpawnElement();
                //break;

            case Type.Spell:
                SpawnSpells();
                break;

            default: 
                break;
        }
        
    }

    /*private void SpawnElement() 
    {
        int totalWeight = 0;
        int randomNumber = 0;

        for (int i  = 0; i < elements.Count; i++)
        {
            int itemWeight = 0;
            itemWeight = elements[i].GetComponent<ItemInfo>().GetWeight();
            totalWeight += itemWeight;
        }
        //UnityEngine.Debug.Log("Total Weight is: " +  totalWeight);

        randomNumber = UnityEngine.Random.Range(1, totalWeight+1);
//        UnityEngine.Debug.Log("Elements Random number is: " + randomNumber);

        for (int i = 0; i < elements.Count; i++)
        {
            int itemWeight = 0;
            itemWeight = elements[i].GetComponent<ItemInfo>().GetWeight();

            randomNumber -= itemWeight;
            if (randomNumber < 1)
            {
                Instantiate(elements[i], this.transform);
                i = elements.Count + 1;
            }
        }




    }*/

    private void SpawnSpells() {

        int totalWeight = 0;
        int randomNumber = 0;

        for (int i = 0; i < spells.Count; i++)
        {
            int itemWeight = 0;
            itemWeight = spells[i].GetComponent<ItemInfo>().GetWeight();
            totalWeight += itemWeight;
        }
        //UnityEngine.Debug.Log("Total Weight is: " + totalWeight);

        randomNumber = UnityEngine.Random.Range(1, totalWeight + 1);
     //   UnityEngine.Debug.Log("Spells Random number is: " + randomNumber);

        for (int i = 0; i < elements.Count; i++)
        {
            int itemWeight = 0;
            itemWeight = elements[i].GetComponent<ItemInfo>().GetWeight();

            randomNumber -= itemWeight;
            if (randomNumber < 1)
            {
                Instantiate(spells[i], this.transform);
                i = spells.Count + 1;
            }
        }


    


    }
}
