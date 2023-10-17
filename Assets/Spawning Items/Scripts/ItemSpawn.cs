using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    // Start is called before the first frame update



    [SerializeField] List<GameObject> SpawnPoints = new List<GameObject>();
    [SerializeField] int maxSpawnPoints = 0;
    void Start()
    {
        GenerateSpawnpoints();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateSpawnpoints()
    {
        int listsize = SpawnPoints.Count;

        int [] chosenNumbers = new int[maxSpawnPoints];

        for(int i = 0; i < chosenNumbers.Length; i++) 
        {
            chosenNumbers[i] = 0;
        }

        for(int i = 0; i < maxSpawnPoints; i++) 
        {
            int randomNumber = 0;
           randomNumber = UnityEngine.Random.Range(1, listsize);
            Debug.Log("New random number is: " +  randomNumber);


            for(int j = 0; j < chosenNumbers.Length; j++)
            {

                if (chosenNumbers[j] == randomNumber)
                {
                    i--;
                    j = chosenNumbers.Length;
                }
                else if (chosenNumbers[j] == 0 ) 
                {
                    chosenNumbers[j] = randomNumber;
                    j = chosenNumbers.Length;
                }
            }



        }
        Debug.Log("The full array list is:");

        for(int i = 0;i < chosenNumbers.Length;i++) 
        {
             Debug.Log(chosenNumbers[i] + ", ");

        }



       
        for (int i = 0; i<chosenNumbers.Length ;i++) 
        {
            if (i == 0 || i == 1)
            {
                SpawnPoints[chosenNumbers[i] - 1].GetComponent<SpawnItem>().SpawnObject(SpawnItem.Type.Element);
            }
            else if(i == 2 || i == 3)
            {
                SpawnPoints[chosenNumbers[i] - 1].GetComponent<SpawnItem>().SpawnObject(SpawnItem.Type.Spell);
            }


           
        }
        

    }
}
