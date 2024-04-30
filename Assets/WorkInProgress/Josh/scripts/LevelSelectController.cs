using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelSelectController : MonoBehaviour
{
    public int mapNumber = 0;
    public GameObject[] levels;
    public GameObject[] resetPosition;
    public string[] sceneNames;
    public int startingMap;
    public GameObject charSelectObj;
    public GameObject mapMenu;
    public GameObject characterMenu;
    public bool onlyLoadOnce = true;

    public bool loading = false;
    public bool unloading = false;

    public List<GameObject> rememberActive = new List<GameObject>();
    public bool timeToReset = false;
    public string nameOfPlayedMap;
    public GameObject charSetupObj;

    [SerializeField] private GameObject canvas;
    [SerializeField] private CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
       // levels[startingMap].SetActive(true);
       
    }

    // Update is called once per frame
    private void Update()
    {
        if(canvasGroup.alpha > 0 && !loading) {
            canvasGroup.alpha -= Time.deltaTime;
                }
       
        else if(canvasGroup.alpha < 1 && loading )
        {
            canvasGroup.alpha += Time.deltaTime;
        }
        if(timeToReset)
        {
            Debug.Log("Resetting to menu");
            timeToReset = false;
            //ResetThings
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(nameOfPlayedMap));
            foreach (var rootObject in rememberActive)
            {
                rootObject.SetActive(true);
            }
            int playercount = 0;
            foreach(var player in GameObject.FindGameObjectsWithTag("Player"))
            {
                var compReg = player.GetComponent<ComponentRegistry>();
                compReg.animationManager.ToggleRestartTrigger();
                compReg.rigidBody.MovePosition(resetPosition[playercount].transform.position);
                compReg.attributeManager.currentHealth = compReg.attributeManager.maxHealth;
                compReg.playerCamera.enabled = true;
                compReg.inputManager.enabled = true;
                compReg.playerController.enabled = true;
                compReg.playerController.readyToJump = true;
                playercount++;
            }
            charSetupObj.GetComponent<CharSetup>().RefreshCameras();
        }
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
            loading = true;
            StartCoroutine(Loadingscreen());
            StartCoroutine(LoadScene(sceneToLoad));
        }
    }

    public IEnumerator Loadingscreen()
    {
      
        yield return new WaitForSeconds(5f);
        loading = false;
    }

    public IEnumerator LoadScene(string sceneToLoad)
    {
        yield return new WaitForSeconds(2f);

        AsyncOperation sceneLoader = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        sceneLoader.allowSceneActivation = false;
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

    public IEnumerator waitforX(float x)
    {
        yield return new WaitForSeconds(x);
    }

    public void FinishLoading(string sceneToLoad)
    {
        nameOfPlayedMap = sceneToLoad;
        var scene = SceneManager.GetSceneByName(sceneToLoad);
        if (scene.IsValid())
        {
            //SceneManager.MoveGameObjectToScene(canvas, scene);
           
            foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
            {
                SceneManager.MoveGameObjectToScene(player, scene);
                player.GetComponent<ComponentRegistry>().attributeManager.currentHealth = player.GetComponent<ComponentRegistry>().attributeManager.maxHealth;
            }

            rememberActive.Clear();
            foreach (var rootGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if(rootGameObject.activeSelf && this.gameObject != rootGameObject)
                {
                    rememberActive.Add(rootGameObject);
                    rootGameObject.SetActive(false);
                }
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
            AudioManager.instance.InitializeMusic(FMODEvents.instance.music);
            onlyLoadOnce = true;

            StartCoroutine(waitforX(5f));
        }
    }
}
