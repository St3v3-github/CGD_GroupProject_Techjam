using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    // Start is called before the first frame update



    [SerializeField] List<GameObject> SpawnPoints = new List<GameObject>();
    [SerializeField] int minSpawnPoints = 0;
    [SerializeField] int maxSpawnPoints = 0;
    [SerializeField] int minElementSpawns = 0;
    [SerializeField] int minSpellSpawns = 0;
    
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
        int randomSpawnCount = 0;

        randomSpawnCount = UnityEngine.Random.Range(minSpawnPoints, maxSpawnPoints+1);
        Debug.Log("Number of spawn points is: " + randomSpawnCount);



        int [] chosenNumbers = new int[randomSpawnCount];

        for(int i = 0; i < chosenNumbers.Length; i++) 
        {
            chosenNumbers[i] = 0;
        }

        for(int i = 0; i < randomSpawnCount; i++) 
        {
            int randomNumber = 0;
           randomNumber = UnityEngine.Random.Range(1, listsize+1);
//            Debug.Log("New random number is: " +  randomNumber);


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
      //  Debug.Log("The full array list is:");

        for(int i = 0;i < chosenNumbers.Length;i++) 
        {
//             Debug.Log(chosenNumbers[i] + ", ");

        }




        int availableSpawns = 0;
        availableSpawns = randomSpawnCount;




        int spawnedElements = 0;
        int spawnedSpells = 0;
        for (int i = 0; i<chosenNumbers.Length ;i++) 
        {
            
            if(spawnedElements < minElementSpawns)
            {
                SpawnPoints[chosenNumbers[i] - 1].GetComponent<SpawnItem>().SpawnObject(SpawnItem.Type.Element);
                availableSpawns--;
                spawnedElements++;
            }
            else if (spawnedSpells < minSpellSpawns)
            {
                SpawnPoints[chosenNumbers[i] - 1].GetComponent<SpawnItem>().SpawnObject(SpawnItem.Type.Spell);
                availableSpawns--;
                spawnedSpells++;
            }
            else if( availableSpawns > 0 )
            {

                int randomNumber = 0;
                randomNumber = UnityEngine.Random.Range(0, 2);
            //    Debug.Log("The randomly generated value is: " + randomNumber);
              
                if(randomNumber == 0 )
                {
                    SpawnPoints[chosenNumbers[i] - 1].GetComponent<SpawnItem>().SpawnObject(SpawnItem.Type.Element);
                    availableSpawns--;
//                    Debug.Log("Randomly chosen an element.");
                }
                else if( randomNumber == 1 )
                {
                    SpawnPoints[chosenNumbers[i] - 1].GetComponent<SpawnItem>().SpawnObject(SpawnItem.Type.Spell);
                    availableSpawns--;
                //    Debug.Log("Randomly chosen a spell.");

                }
            }







           
       

           
        }
        

    }
}
