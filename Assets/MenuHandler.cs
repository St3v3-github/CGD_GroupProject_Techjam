using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuHandler : MonoBehaviour
{
public bool firstMenu = true;
public bool secondMenu = false;
public bool mapMenu = false;

public static MenuHandler instance;

private PlayerInput playerInput;

private InputAction confirmAction;
public GameObject menuFirstButton;
public GameObject gateFirstButton;
public GameObject mapsFirstButton;

public bool confirmActionPressed { get; private set; }



// Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        playerInput = GetComponent<PlayerInput>();
        confirmAction = playerInput.actions["Confirm"];
        EventSystem.current.SetSelectedGameObject(gateFirstButton);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        playerInput = GetComponent<PlayerInput>();
        confirmAction = playerInput.actions["Confirm"];
        EventSystem.current.SetSelectedGameObject(gateFirstButton);
    }

    // Update is called once per frame
    void Update()
    {
      confirmActionPressed = confirmAction.WasPressedThisFrame();

     
    }
    

    public void EndFirstMenu()
    {
        Debug.Log("ENDFIRSTMENU");
        firstMenu = false;
        secondMenu = true;
        EventSystem.current.SetSelectedGameObject(menuFirstButton);
        
        
    }
    public void EndSecondMenu()
    {
        secondMenu = false;
        mapMenu = true;
        EventSystem.current.SetSelectedGameObject(mapsFirstButton);

    }
    public void EndMapMenu()
    {
        this.gameObject.SetActive(false);
        
    }


  
    
    
    
}
