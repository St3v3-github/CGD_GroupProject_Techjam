using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelSelectController : MonoBehaviour
{
    public int mapNumber = 0;
    public GameObject[] levels;
    public string[] sceneNames;
    public int startingMap;
    public GameObject charSelectObj;
    public GameObject mapMenu;
    public GameObject characterMenu;
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
        if(mapNumber==levels.Length)
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
            mapNumber = levels.Length-1;
        }
        levels[mapNumber].SetActive(true);
    }

    public void CharSelect()
    {
        levels[mapNumber].SetActive(false);
        charSelectObj.SetActive(true);
        mapMenu.SetActive(false);
        characterMenu.SetActive(true);
    }


    public IEnumerator LoadScene()
    {
        AsyncOperation sceneLoader = SceneManager.LoadSceneAsync(sceneNames[mapNumber], LoadSceneMode.Additive);
        sceneLoader.allowSceneActivation = false;
        while (sceneLoader.progress < 0.9f)
        {
            Debug.Log("Loading scene " + " [][] Progress: " + sceneLoader.progress);
            yield return null;
        }
        FinishLoading(sceneLoader);
    }

    public void FinishLoading(AsyncOperation sceneLoader)
    {
        sceneLoader.allowSceneActivation = true;
        var scene = SceneManager.GetSceneByName(sceneNames[mapNumber]);
        if(scene.IsValid())
        {
            foreach(var player in GameObject.FindGameObjectsWithTag("Player"))
            {
                SceneManager.MoveGameObjectToScene(player, scene);
            }
        }
        SceneManager.SetActiveScene(scene);
    }
}
