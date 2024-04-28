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
    public bool onlyLoadOnce = true;

    // Start is called before the first frame update
    void Start()
    {
        levels[startingMap].SetActive(true);
        CharSelect();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void NextMap()
    {
        levels[mapNumber].SetActive(false);
        mapNumber++;
        if(mapNumber == levels.Length)
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
            mapNumber = levels.Length - 1;
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

    public void StartGameplayScene(string sceneToLoad)
    {
        if (onlyLoadOnce)
        {
            onlyLoadOnce = false;
            StartCoroutine(LoadScene(sceneToLoad));
        }
    }

    public IEnumerator LoadScene(string sceneToLoad)
    {
        AsyncOperation sceneLoader = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        sceneLoader.allowSceneActivation = false;
        charSelectObj.GetComponent<CharSetup>().kwikfix = false;
        while (sceneLoader.progress < 0.9f)
        {
            Debug.Log("Loading scene " + sceneToLoad + " <<||>> Progress: " + sceneLoader.progress);
            yield return null;
        }
        sceneLoader.allowSceneActivation = true;
        while (!SceneManager.GetSceneByName(sceneToLoad).isLoaded)
        {
            Debug.Log("Scene not fully loaded yet...");
            yield return null;
        }
        FinishLoading(sceneToLoad);
    }

    public void FinishLoading(string sceneToLoad)
    {
        var scene = SceneManager.GetSceneByName(sceneToLoad);
        if (scene.IsValid())
        {
            foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
            {
                SceneManager.MoveGameObjectToScene(player, scene);
            }

            foreach (var rootGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                rootGameObject.SetActive(false);
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
            onlyLoadOnce = true;
        }
        
    }
}
