    using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SpawnItem;

public class ItemSpawn : MonoBehaviour
{
    // Start is called before the first frame update


    private float timerMax = 10;
    private float timerCurrent = 10;
    [SerializeField] List<GameObject> SpawnPoints = new List<GameObject>();
   [SerializeField] int minSpawnPoints = 0;
    [SerializeField] int maxSpawnPoints = 0;
    //[SerializeField] int minElementSpawns = 0;
    [SerializeField] int minSpellSpawns = 0;
    [SerializeField]  List<GameObject> ultimateList = new List<GameObject>();
    [SerializeField]  List<GameObject> rareList = new List<GameObject>();
    [SerializeField]  List<GameObject> uncommonList = new List<GameObject>();
    [SerializeField]  List<GameObject> commonList = new List<GameObject>();

    
    void Start()
    {
        GenerateSpawnpoints();
        
    }

    // Update is called once per frame
    void Update()
    {
        /*timerCurrent -= Time.deltaTime;
        if(timerCurrent <= 0)
        {
            GenerateSpawnpoints();
            timerCurrent = timerMax;
        }*/
        
    }

    private List<GameObject> GetSpellList(SpawnerType spawnerType) 
    {
        switch (spawnerType)
        {
            case SpawnerType.AnySpell:
                List<GameObject> newSpelllist = new List<GameObject>();
                newSpelllist.AddRange(commonList);
                newSpelllist.AddRange(uncommonList);
                newSpelllist.AddRange(rareList);
                newSpelllist.AddRange(ultimateList);
               
                return newSpelllist;
             
            case SpawnerType.CommonOnly:
                return commonList;
                
            case SpawnerType.UncommonOnly:
                return uncommonList;
                
            case SpawnerType.RareOnly:
                return rareList;
                
            case SpawnerType.UltimateOnly:
                return ultimateList;
                
                           
        }
        return null;
    }

    public void GenerateSpawnpoints()
    {
        int listsize = SpawnPoints.Count;
        int randomSpawnCount = 0;

        for(int i = 0;i < SpawnPoints.Count; i++)
        {
            SpawnerType currentSpawnerType = SpawnPoints[i].GetComponent<SpawnItem>().spawnerType;
       List<GameObject> currentSpellList = GetSpellList(currentSpawnerType);
                SpawnPoints[i].GetComponent<SpawnItem>().SetSpells(ultimateList);
        }

        randomSpawnCount = UnityEngine.Random.Range(minSpawnPoints, maxSpawnPoints+1);
        




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



// THIS NEEDS TO BE SORTED - FIX LATER
       // int spawnedElements = 0;
        //int spawnedSpells = 0;
        for (int i = 0; i<chosenNumbers.Length ;i++) 
        {
            
           /* if(spawnedElements < minElementSpawns)
            {
                SpawnPoints[chosenNumbers[i] - 1].GetComponent<SpawnItem>().SpawnObject(SpawnItem.Type.Spell);
                availableSpawns--;
                spawnedElements++;
            }
            else if (spawnedSpells < minSpellSpawns)
            {
                SpawnPoints[chosenNumbers[i] - 1].GetComponent<SpawnItem>().SpawnObject(SpawnItem.Type.Spell);
                availableSpawns--;
                spawnedSpells++;
            }*/
            if( availableSpawns > 0 )
            {

                int randomNumber = 0;
                randomNumber = UnityEngine.Random.Range(0, 2);
            //    Debug.Log("The randomly generated value is: " + randomNumber);
              
                if(randomNumber == 0 )
                {
                    SpawnPoints[chosenNumbers[i] - 1].GetComponent<SpawnItem>().SpawnObject(SpawnItem.Type.Spell);
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
