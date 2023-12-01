using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
    

public class LevelSelectController : MonoBehaviour
{
    
    public GameObject Map; 
    public Button next;
    public Button Previous;
    public int MapNumber = 0;
    public int Numberofmaps;
    public GameObject[] Levels;
    public GameObject StartingMap; 
    int i; 
    // Start is called before the first frame update
    void Start()
    {
        Map = Levels[0]; 
        Map.SetActive(true);
        
    }

    // Update is called once per frame
    private void Update()
    {
       
        
    }

   public void NextMap()
    {
        if (MapNumber < Numberofmaps)
        {
         MapNumber++;
        }

        if (MapNumber == Numberofmaps)
        {
            MapNumber++;
        }

        if (MapNumber > Numberofmaps)
        {
            MapNumber = 0; 
        }
        for (i = 0; i < Numberofmaps; i++)
        {
            Levels[i].SetActive(false);
        }
        Map = Levels[MapNumber];
        Map.SetActive(true);  
        
    }
    
}
