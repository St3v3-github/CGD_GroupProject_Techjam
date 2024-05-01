using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    //Function to play game once play is selected on the menu
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("DemoScene");
    }
}
