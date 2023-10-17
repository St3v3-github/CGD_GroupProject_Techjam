using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
            case Type.Element:
                SpawnElement();
                break;

            case Type.Spell:
                SpawnSpells();
                break;

            default: 
                break;
        }
        
    }

    private void SpawnElement() 
    {

        int randomNumber = 0;
        randomNumber = UnityEngine.Random.Range(0, elements.Count);

        Instantiate(elements[randomNumber], this.transform);

        }

    private void SpawnSpells() {
        int randomNumber = 0;
        randomNumber = UnityEngine.Random.Range(0, spells.Count);

        Instantiate(spells[randomNumber], this.transform);

    }
}
