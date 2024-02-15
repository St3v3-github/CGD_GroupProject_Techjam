using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelSelectController : MonoBehaviour
{
    public int mapNumber = 0;
    public int numberofmaps;
    public GameObject[] levels;
    public int startingMap;
    public GameObject charSelectObj;
    // Start is called before the first frame update
    void Start()
    {
        levels[startingMap].SetActive(true);
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void NextMap()
    {
        levels[mapNumber].SetActive(false);
        mapNumber++;
        if(mapNumber==numberofmaps)
        {
            mapNumber = 0;
        }
        levels[mapNumber].SetActive(true);
    }

    public void PrevMap()
    {
        levels[mapNumber].SetActive(false);
        mapNumber--;
        if(mapNumber<0)
        {
            mapNumber = numberofmaps-1;
        }
        levels[mapNumber].SetActive(true);
    }

    public void CharSelect()
    {
        levels[mapNumber].SetActive(false);
        charSelectObj.SetActive(true);
        //Transition to character selection
    }
}
