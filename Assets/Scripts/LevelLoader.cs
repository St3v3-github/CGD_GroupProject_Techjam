using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    //Public variables to access scene load from any other script - might need in gamne manager?
    //public Animator transition;
    //public float transitionTime = 1f;
    [SerializeField] public GameObject a;
    public int levelSelect;


    //Test on Mouseclick
    /*    void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                LoadNextScene();
            }
        }*/

    //Function to load the scene
    public void NextLevel()
    {
        levelSelect = a.GetComponent<LevelSelectController>().MapNumber;
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene(levelSelect);
    }

    //Courotine To delay the Scene Load - Animation has to play first
    //IEnumerator LoadLevel(int levelIndex)
    //{
        //transition.SetTrigger("Start");

        //yield return new WaitForSeconds(transitionTime);

        //SceneManager.LoadScene(levelIndex);
        
        //SceneManager.LoadScene("Scene B");   -Can also do this :)
    //}
    
}
