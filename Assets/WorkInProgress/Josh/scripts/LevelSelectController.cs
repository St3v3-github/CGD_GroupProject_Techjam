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

    public void StartGameplayScene()
    {
        StartCoroutine(LoadScene());
    }

    public IEnumerator LoadScene()
    {
        AsyncOperation sceneLoader = SceneManager.LoadSceneAsync(sceneNames[mapNumber], LoadSceneMode.Additive);
        sceneLoader.allowSceneActivation = false;
        while (sceneLoader.progress < 0.9f)
        {
            Debug.Log("Loading scene " + sceneNames[mapNumber] + " <<||>> Progress: " + sceneLoader.progress);
            yield return null;
        }
        FinishLoading(sceneLoader);
    }

    public void FinishLoading(AsyncOperation sceneLoader)
    {
        sceneLoader.allowSceneActivation = true;
        var scene = SceneManager.GetSceneByName(sceneNames[mapNumber]);
        Debug.Log(sceneNames[mapNumber] + " loaded?");
        if (scene.IsValid())
        {
            foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
            {
                SceneManager.MoveGameObjectToScene(player, scene);
            }

            foreach (var rootGameObject in scene.GetRootGameObjects())
            {
                if (rootGameObject.CompareTag("GameController"))
                {
                    var rules = rootGameObject.GetComponent<GameModeHandler>().ruleSetting;
                    rules.gameTime = 300;
                    //Can edit the rules here.
                    rootGameObject.GetComponent<GameModeHandler>().LoadGameSettings();
                }
            }
            SceneManager.SetActiveScene(scene);
        }
    }
}
