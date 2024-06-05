using FMOD;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static Unity.Burst.Intrinsics.X86.Avx;
using static UnityEngine.UI.Image;

public class CharSetup : MonoBehaviour
{
    public int maxPlayers;
    public float inputTime;
    public GameObject[] characterPositions;
    public GameObject[] tutorialPositions;
    public GameObject[] toJoinDisplays;
    public GameObject[] playerSetupMenus;
    public GameObject[] playerClassRotation;
    public ComponentRegistry[] componentRegistries;
    public List<int> playerClassID = new List<int>();
    public GameObject[] players;
    public int[] menuSelections;
    public CharMenuLevels[] menuLevels;
    public float[] inputClocks;
    public int maxSelection = 3;
    public PlayerInputManager inputManager;
    public bool updateAfterDestroy = false;
    public float[] recordedHealth;
    public float[] lastHeal;
    public float[] recordTimers;
    public float healDelay = 2.0f;
    public float healPerSec = 20.0f;
    public List<int>[] customisationIDs;
    public List<int>[] colourIDs;
    public const int CUSTOMISATION_PARTS = 4;
    public CharacterComponentLister charCompLister;
    public GameObject[] customisationMenus;
    public List<GameObject> customisationCameras;
    public MapSelector mapSelector;
    public Camera[] targetCameras;
    public GameObject blankCamera;
    public TextMeshProUGUI joinText;

    public enum CharMenuLevels
    {
        CHAR_CLASS = 0,
        HAIR_CUSTOM,
        HEAD_CUSTOM,
        BODY_CUSTOM,
        LEGS_CUSTOM,
        MAIN_CONTROLS,
        LEVEL_MAX
    }

    void Start()
    {
        recordedHealth = new float[maxPlayers];
        lastHeal = new float[maxPlayers];
        recordTimers = new float[maxPlayers];
        customisationIDs = new List<int>[maxPlayers];
        colourIDs = new List<int>[maxPlayers];
        for (int i = 0; i < maxPlayers; i++)
        {
            playerClassID.Add(0);
            customisationIDs[i] = new List<int>();
            colourIDs[i] = new List<int>();
            for(int j = 0; j < CUSTOMISATION_PARTS; j++)
            {
                customisationIDs[i].Add(0);
                colourIDs[i].Add(0);
            }
            colourIDs[i].Add(0);
        }
        menuSelections = new int[maxPlayers];
        menuLevels = new CharMenuLevels[maxPlayers];
        inputClocks = new float[maxPlayers];
        mapSelector = GameObject.FindGameObjectWithTag("VoteHandler").GetComponent<MapSelector>();
    }

    private void Update()
    {
        for (int i = 0; i < maxPlayers; i++)
        {
            if (inputClocks[i] > 0)
            {
                inputClocks[i] -= Time.deltaTime;
            }
        }
    }

    const float TOLERANCE = 0.001f;
    private void LateUpdate()
    {
        if (updateAfterDestroy)
        {
            updateAfterDestroy = false;
            foreach (var player in GameObject.FindObjectsOfType<ComponentRegistry>(true))
            {
                if (player.gameObject.activeSelf == false)
                {
                    Destroy(player.gameObject);
                }
            }
        }
        for (int i = 0; i < maxPlayers; i++)
        {
            if (players[i] == null) { continue; }
            if (componentRegistries[i].attributeManager.currentHealth - lastHeal[i] >= recordedHealth[i] - TOLERANCE)
            {
                recordTimers[i] += Time.deltaTime;
            }
            else
            {
                recordTimers[i] = 0f;
                lastHeal[i] = 0f;
            }
            recordedHealth[i] = componentRegistries[i].attributeManager.currentHealth;
            if (recordTimers[i] < healDelay) { continue; }
            if (componentRegistries[i].attributeManager.currentHealth >= componentRegistries[i].attributeManager.maxHealth)
            {
                componentRegistries[i].attributeManager.currentHealth = componentRegistries[i].attributeManager.maxHealth;
                continue;
            }
            lastHeal[i] = healPerSec * Time.deltaTime;
            componentRegistries[i].attributeManager.currentHealth += lastHeal[i];
        }
    }

    public void HandleNewPlayer()
    {
        int nrOfPlayers = 0;
        ComponentRegistry componentRegistry = null;
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            componentRegistry = player.GetComponent<ComponentRegistry>();
            if (componentRegistry.playerInput.playerIndex != -1 && componentRegistry.attributeManager.initialSpawnSetup)
            {
                componentRegistry.attributeManager.initialSpawnSetup = false;
                componentRegistry.mainMesh.GetComponent<CameraCulling>().SetGameLayerRecursive(componentRegistry.mainMesh, 12 + componentRegistry.playerInput.playerIndex);
                componentRegistry.projectionLayer = 16 + componentRegistry.playerInput.playerIndex;
                players[componentRegistry.playerInput.playerIndex] = player;
                menuSelections[componentRegistry.playerInput.playerIndex] = 0;
                componentRegistries[componentRegistry.playerInput.playerIndex] = componentRegistry;
                componentRegistry.playerScoreInfo.team = componentRegistry.playerInput.playerIndex;
                componentRegistry.inputManager.enabled = false;
                componentRegistry.playerInput.actions.FindAction("MenuRight").performed += MenuRight;
                componentRegistry.playerInput.actions.FindAction("MenuLeft").performed += MenuLeft;
                componentRegistry.playerInput.actions.FindAction("MenuUp").performed += MenuUp;
                componentRegistry.playerInput.actions.FindAction("MenuDown").performed += MenuDown;
                componentRegistry.playerInput.actions.FindAction("MenuExecute").performed += MenuExecute;
                //REMEMBER TO DISABLE SPELL SYSTEM
                //componentRegistry.advancedProjectileSystem.enabled = false;
                componentRegistry.playerCamera.enabled = false;
                componentRegistry.playerController.enabled = false;
                componentRegistry.rigidBody.MovePosition(characterPositions[componentRegistry.playerInput.playerIndex].transform.position);
                toJoinDisplays[componentRegistry.playerInput.playerIndex].SetActive(false);
                playerSetupMenus[componentRegistry.playerInput.playerIndex].SetActive(true);
                customisationIDs[componentRegistry.playerInput.playerIndex][HAIR_ID] = 1;
                customisationIDs[componentRegistry.playerInput.playerIndex][HEAD_ID] = 1;
                customisationIDs[componentRegistry.playerInput.playerIndex][BODY_ID] = 1;
                colourIDs[componentRegistry.playerInput.playerIndex][BODY_ID] = 0;
                customisationIDs[componentRegistry.playerInput.playerIndex][LEGS_ID] = 1;
                colourIDs[componentRegistry.playerInput.playerIndex][LEGS_ID] = 0;
                PrevHead(componentRegistry.playerInput.playerIndex);
                PrevHair(componentRegistry.playerInput.playerIndex);
                PrevBody(componentRegistry.playerInput.playerIndex);
                PrevLegs(componentRegistry.playerInput.playerIndex);
                updatePlayerColours(componentRegistry.playerInput.playerIndex);
                customisationCameras[componentRegistry.playerInput.playerIndex].SetActive(true);
                targetCameras[componentRegistry.playerInput.playerIndex] = customisationCameras[componentRegistry.playerInput.playerIndex].GetComponent<Camera>();
            }
            nrOfPlayers++;
        }
        RefreshCameras();
        mapSelector.playerCount = nrOfPlayers;
        joinText.text = nrOfPlayers + " / 4";
        UnityEngine.Debug.Log("New player is being added...");
    }
    private int getNrOfPlayers()
    {
        int answer = 0;
        foreach(var player in players)
        {
            if(player != null)
            {
                answer++;
            }
        }
        return answer;
    }

    public void RefreshCameras()
    {
        int nrOfPlayers = getNrOfPlayers();
        int i = 1;
        for (i = 1; (nrOfPlayers - 1) / i >= i; i++)
        {
            UnityEngine.Debug.Log(i.ToString());
        }
        int camColumns = i;
        int camRows = i;
        //Try reducing rows
        if (i * (i - 1) >= nrOfPlayers)
        {
            camRows = i - 1;
        }
        //Determine camera dimensions
        float camXSize = 1.0f / camColumns;
        float camYSize = 1.0f / camRows;
        UnityEngine.Debug.Log(camColumns.ToString());
        UnityEngine.Debug.Log(camRows.ToString());
        UnityEngine.Debug.Log(camXSize.ToString());
        UnityEngine.Debug.Log(camYSize.ToString());
        //Update camera render targets
        for (int j = 0; j < nrOfPlayers; j++)
        {
            targetCameras[j].rect = new Rect((float)(j % camColumns) * camXSize, 1.0f - (float)((j / camColumns) + 1) * camYSize, camXSize, camYSize);
            targetCameras[j].depth = 3;
        }
        blankCamera.SetActive(nrOfPlayers==3);
    }

    public void LeaveCharSetup(int playerID)
    {
        var componentRegistry = componentRegistries[playerID];
        if (componentRegistry.playerInput.playerIndex != -1)
        {
            componentRegistry.inputManager.enabled = true;
            componentRegistry.playerCamera.enabled = true;
            componentRegistry.playerController.enabled = true;
            componentRegistry.playerInput.actions.FindAction("MenuRight").Disable();
            componentRegistry.playerInput.actions.FindAction("MenuLeft").Disable();
            componentRegistry.playerInput.actions.FindAction("MenuUp").Disable();
            componentRegistry.playerInput.actions.FindAction("MenuDown").Disable();
            componentRegistry.playerInput.actions.FindAction("MenuExecute").Disable();
            componentRegistry.rigidBody.MovePosition(tutorialPositions[componentRegistry.playerInput.playerIndex].transform.position);
            toJoinDisplays[componentRegistry.playerInput.playerIndex].SetActive(false);
            playerSetupMenus[componentRegistry.playerInput.playerIndex].SetActive(true);
            customisationCameras[componentRegistry.playerInput.playerIndex].SetActive(false);
            componentRegistry.playerController.readyToJump = true;
            targetCameras[playerID] = componentRegistry.playerCamera;
            RefreshCameras();
        }

    }

    public void EnterCharSetup(int playerID)
    {
        var componentRegistry = componentRegistries[playerID];
        if (componentRegistry.playerInput.playerIndex != -1)
        {
            componentRegistry.inputManager.enabled = false;
            componentRegistry.playerCamera.enabled = false;
            componentRegistry.playerController.enabled = false;
            componentRegistry.playerInput.actions.FindAction("MenuRight").Enable();
            componentRegistry.playerInput.actions.FindAction("MenuLeft").Enable();
            componentRegistry.playerInput.actions.FindAction("MenuUp").Enable();
            componentRegistry.playerInput.actions.FindAction("MenuDown").Enable();
            componentRegistry.playerInput.actions.FindAction("MenuExecute").Enable();
            componentRegistry.rigidBody.MovePosition(characterPositions[componentRegistry.playerInput.playerIndex].transform.position);
            componentRegistry.rigidBody.rotation = characterPositions[componentRegistry.playerInput.playerIndex].transform.rotation;
            componentRegistry.rigidBody.velocity = new Vector3(0, 0, 0);
            componentRegistry.playerController.readyToJump = false;
            componentRegistry.animationManager.updateMovementFloats(new Vector3(0, 0, 0));
            toJoinDisplays[componentRegistry.playerInput.playerIndex].SetActive(false);
            playerSetupMenus[componentRegistry.playerInput.playerIndex].SetActive(true);
            customisationCameras[componentRegistry.playerInput.playerIndex].SetActive(true);
            targetCameras[playerID] = customisationCameras[componentRegistry.playerInput.playerIndex].GetComponent<Camera>();
            RefreshCameras();
        }
    }

    public void EnterCharSetup()
    {
         int playernumber = 0;
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            var componentRegistry = player.GetComponent<ComponentRegistry>();
            if (componentRegistry.playerInput.playerIndex != -1)
            {
                playernumber++;
               componentRegistry.inputManager.enabled = false;
                componentRegistry.playerCamera.enabled = false;
                componentRegistry.playerController.enabled = false;
                componentRegistry.rigidBody.MovePosition(characterPositions[componentRegistry.playerInput.playerIndex].transform.position);
                toJoinDisplays[componentRegistry.playerInput.playerIndex].SetActive(false);
                playerSetupMenus[componentRegistry.playerInput.playerIndex].SetActive(true);
                customisationCameras[componentRegistry.playerInput.playerIndex].SetActive(true);
            }
        }

    }

    public void onMenuChange(int index)
    {
        customisationMenus[index].GetComponent<CustomisationUIHandler>().UpdateUISeclections(menuLevels[index], menuSelections[index]);
        //Create an array to feed in first, toArray does not work
        int[] index_array = new int[CUSTOMISATION_PARTS];
        for(int i = 0; i<CUSTOMISATION_PARTS;i++)
        {
            index_array[i] = customisationIDs[index][i];
        }
        customisationMenus[index].GetComponent<CustomisationUIHandler>().UpdateSelectionNames(index_array, playerClassID[index]);
    }
    
    public void MenuLeft(InputAction.CallbackContext ctx)
    {
        for (int i = 0; i < maxPlayers; i++)
        {
            if (players[i] != null)
            {
                foreach (var playerInputDevice in componentRegistries[i].playerInput.devices)
                {
                    if (playerInputDevice == ctx.control.device && inputClocks[i] <= 0.0f)
                    {
                        inputClocks[i] = inputTime;
                        //NextClass(i);
                        /*switch(menuLevels[i])
                        {
                            case CharMenuLevels.CHAR_CLASS:
                                break;
                        }*/
                        menuSelections[i]--;
                        if (menuSelections[i] < 0)
                        {
                            menuSelections[i] = maxSelection - 1;
                        }
                        onMenuChange(i);
                        break;
                    }
                }
            }
        }
    }

    public void MenuRight(InputAction.CallbackContext ctx)
    {
        for (int i = 0; i < maxPlayers; i++)
        {
            if (players[i] != null)
            {
                foreach (var playerInputDevice in componentRegistries[i].playerInput.devices)
                {
                    if (playerInputDevice == ctx.control.device && inputClocks[i] <= 0.0f)
                    {
                        inputClocks[i] = inputTime;
                        menuSelections[i]++;
                        if (menuSelections[i] == maxSelection)
                        {
                            UnityEngine.Debug.Log("Menu Selection RESET");
                            menuSelections[i] = 0;
                        }
                        UnityEngine.Debug.Log("Menu Selection is: " + menuSelections[i]);
                        onMenuChange(i);
                        break;
                    }
                }
            }
        }
    }

    public void MenuUp(InputAction.CallbackContext ctx)
    {
        for (int i = 0; i < maxPlayers; i++)
        {
            if (players[i] != null)
            {
                foreach (var playerInputDevice in componentRegistries[i].playerInput.devices)
                {
                    if (playerInputDevice == ctx.control.device && inputClocks[i] <= 0.0f)
                    {
                        inputClocks[i] = inputTime;
                        menuLevels[i]--;
                        if (menuLevels[i] < 0)
                        {
                            menuLevels[i] = CharMenuLevels.MAIN_CONTROLS;
                        }
                        onMenuChange(i);
                        break;
                    }
                }
            }
        }
    }

    public void MenuDown(InputAction.CallbackContext ctx)
    {
        for (int i = 0; i < maxPlayers; i++)
        {
            if (players[i] != null)
            {
                foreach (var playerInputDevice in componentRegistries[i].playerInput.devices)
                {
                    if (playerInputDevice == ctx.control.device && inputClocks[i] <= 0.0f)
                    {
                        inputClocks[i] = inputTime;
                        menuLevels[i]++;
                        if (menuLevels[i] >= CharMenuLevels.LEVEL_MAX)
                        {
                            menuLevels[i] = CharMenuLevels.CHAR_CLASS;
                        }
                        onMenuChange(i);
                        break;
                    }
                }
            }
        }
    }

    public void MenuExecute(InputAction.CallbackContext ctx)
    {
        for (int i = 0; i < maxPlayers; i++)
        {
            if (players[i] != null)
            {
                foreach (var playerInputDevice in componentRegistries[i].playerInput.devices)
                {
                    if (playerInputDevice == ctx.control.device && inputClocks[i] <= 0.0f)
                    {
                        inputClocks[i] = inputTime;
                        UnityEngine.Debug.Log(i.ToString() + " executed " + menuLevels[i] + " button " + menuSelections[i]);
                        switch(menuLevels[i]) 
                        {
                            case CharMenuLevels.CHAR_CLASS:
                                switch(menuSelections[i])
                                {
                                    case 0:
                                        previousClass(i);
                                        break;
                                    case 1:
                                        //TODO: Show Class Info
                                        break;
                                    case 2:
                                        NextClass(i);
                                        break;
                                }
                                break;
                            case CharMenuLevels.HAIR_CUSTOM:
                                switch (menuSelections[i])
                                {
                                    case 0:
                                        PrevHair(i);
                                        break;
                                    case 1:
                                        ChangeColour(i, HAIR_ID);
                                        break;
                                    case 2:
                                        NextHair(i);
                                        break;
                                }
                                break;
                            case CharMenuLevels.HEAD_CUSTOM:
                                switch (menuSelections[i])
                                {
                                    case 0:
                                        PrevHead(i);
                                        break;
                                    case 1:
                                        ChangeColour(i, HEAD_ID);
                                        break;
                                    case 2:
                                        NextHead(i);
                                        break;
                                }
                                break;
                            case CharMenuLevels.BODY_CUSTOM:
                                switch (menuSelections[i])
                                {
                                    case 0:
                                        PrevBody(i);
                                        break;
                                    case 1:
                                        ChangeColour(i, BODY_ID);
                                        break;
                                    case 2:
                                        NextBody(i);
                                        break;
                                }
                                break;
                            case CharMenuLevels.LEGS_CUSTOM:
                                switch (menuSelections[i])
                                {
                                    case 0:
                                        PrevLegs(i);
                                        break;
                                    case 1:
                                        ChangeColour(i, LEGS_ID);
                                        break;
                                    case 2:
                                        NextLegs(i);
                                        break;
                                }
                                break;
                            case CharMenuLevels.MAIN_CONTROLS:
                                switch (menuSelections[i])
                                {
                                    case 0:
                                        LeaveCharSetup(i);
                                        break;
                                    case 1:
                                        LeaveCharSetup(i);
                                        break;
                                    case 2:
                                        LeaveCharSetup(i);
                                        break;
                                }
                                break;
                        }
                    }
                    updatePlayerColours(i);
                    onMenuChange(i);
                }
            }
        }
    }

    #region CustomizationFunctions

    const int HEAD_ID = 1;
    public void NextHead(int index)
    {
        CharacterComponentLister.PreBuiltHead new_head = new CharacterComponentLister.PreBuiltHead();
        customisationIDs[index][HEAD_ID]++;
        switch(componentRegistries[index].meshComponentList.meshList)
        {
            case 0:
                if (customisationIDs[index][HEAD_ID] >= charCompLister.lightComponents.head.Count)
                {
                    customisationIDs[index][HEAD_ID] = 0;
                }
                new_head = charCompLister.lightComponents.head[customisationIDs[index][HEAD_ID]];
                break;
            case 1:
                if (customisationIDs[index][HEAD_ID] >= charCompLister.mediumComponents.head.Count)
                {
                    customisationIDs[index][HEAD_ID] = 0;
                }
                new_head = charCompLister.mediumComponents.head[customisationIDs[index][HEAD_ID]];
                break;
            case 2:
                if (customisationIDs[index][HEAD_ID] >= charCompLister.heavyComponents.head.Count)
                {
                    customisationIDs[index][HEAD_ID] = 0;
                }
                new_head = charCompLister.heavyComponents.head[customisationIDs[index][HEAD_ID]];
                break;
        }
        onHeadChange(index, new_head);
    }

    public void PrevHead(int index)
    {
        CharacterComponentLister.PreBuiltHead new_head = new CharacterComponentLister.PreBuiltHead();
        customisationIDs[index][HEAD_ID]--;
        switch (componentRegistries[index].meshComponentList.meshList)
        {
            case 0:
                if (customisationIDs[index][HEAD_ID] < 0)
                {
                    customisationIDs[index][HEAD_ID] = charCompLister.lightComponents.head.Count - 1;
                }
                new_head = charCompLister.lightComponents.head[customisationIDs[index][HEAD_ID]];
                break;
            case 1:
                if (customisationIDs[index][HEAD_ID] < 0)
                {
                    customisationIDs[index][HEAD_ID] = charCompLister.mediumComponents.head.Count - 1;
                }
                new_head = charCompLister.mediumComponents.head[customisationIDs[index][HEAD_ID]];
                break;
            case 2:
                if (customisationIDs[index][HEAD_ID] < 0)
                {
                    customisationIDs[index][HEAD_ID] = charCompLister.heavyComponents.head.Count - 1;
                }
                new_head = charCompLister.heavyComponents.head[customisationIDs[index][HEAD_ID]];
                break;
        }
        UnityEngine.Debug.Log(customisationIDs[index][HEAD_ID]);
        onHeadChange(index, new_head);
    }

    public void onHeadChange(int index, CharacterComponentLister.PreBuiltHead new_head)
    {
        if(new_head.head != null)
        {
            componentRegistries[index].meshComponentList.head.sharedMesh = new_head.head.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            //componentRegistries[index].meshComponentList.head.bounds = new_head.head.GetComponent<SkinnedMeshRenderer>().bounds;
            componentRegistries[index].meshComponentList.head.materials = new_head.head.GetComponent<SkinnedMeshRenderer>().materials;
            componentRegistries[index].meshComponentList.head.enabled = true;
        }
        else
        {
            componentRegistries[index].meshComponentList.head.enabled = false;
        }

        if (new_head.accessory != null)
        {
            componentRegistries[index].meshComponentList.headAccessory.sharedMesh = new_head.accessory.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            //componentRegistries[index].meshComponentList.headAccessory.bounds = new_head.accessory.GetComponent<SkinnedMeshRenderer>().bounds;
            componentRegistries[index].meshComponentList.headAccessory.materials = new_head.accessory.GetComponent<SkinnedMeshRenderer>().materials;
            componentRegistries[index].meshComponentList.headAccessory.enabled = true;
        }
        else
        {
            componentRegistries[index].meshComponentList.headAccessory.enabled = false;
        }

        if (new_head.facialHair != null)
        {
            componentRegistries[index].meshComponentList.facialHair.sharedMesh = new_head.facialHair.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            //componentRegistries[index].meshComponentList.facialHair.bounds = new_head.facialHair.GetComponent<SkinnedMeshRenderer>().bounds;
            componentRegistries[index].meshComponentList.facialHair.materials = new_head.facialHair.GetComponent<SkinnedMeshRenderer>().materials;
            componentRegistries[index].meshComponentList.facialHair.enabled = true;
        }
        else
        {
            componentRegistries[index].meshComponentList.facialHair.enabled = false;
        }

        if (new_head.eyebrow != null)
        {
            componentRegistries[index].meshComponentList.eyebrow.sharedMesh = new_head.eyebrow.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            //componentRegistries[index].meshComponentList.eyebrow.bounds = new_head.eyebrow.GetComponent<SkinnedMeshRenderer>().bounds;
            componentRegistries[index].meshComponentList.eyebrow.materials = new_head.eyebrow.GetComponent<SkinnedMeshRenderer>().materials;
            componentRegistries[index].meshComponentList.eyebrow.enabled = true;
        }
        else
        {
            componentRegistries[index].meshComponentList.eyebrow.enabled = false;
        }
    }

    const int BODY_ID = 2;
    public void NextBody(int index)
    {
        CharacterComponentLister.PreBuiltChest new_body = new CharacterComponentLister.PreBuiltChest();
        customisationIDs[index][BODY_ID]++;
        switch(componentRegistries[index].meshComponentList.meshList)
        {
            case 0:
                if (customisationIDs[index][BODY_ID] >= charCompLister.lightComponents.chest.Count)
                {
                    customisationIDs[index][BODY_ID] = 0;
                }
                new_body = charCompLister.lightComponents.chest[customisationIDs[index][BODY_ID]];
                break;
            case 1:
                if (customisationIDs[index][BODY_ID] >= charCompLister.mediumComponents.chest.Count)
                {
                    customisationIDs[index][BODY_ID] = 0;
                }
                new_body = charCompLister.mediumComponents.chest[customisationIDs[index][BODY_ID]];
                break;
            case 2:
                if (customisationIDs[index][BODY_ID] >= charCompLister.heavyComponents.chest.Count)
                {
                    customisationIDs[index][BODY_ID] = 0;
                }
                new_body = charCompLister.heavyComponents.chest[customisationIDs[index][BODY_ID]];
                break;
        }
        onBodyChange(index, new_body);
    }

    public void PrevBody(int index)
    {
        CharacterComponentLister.PreBuiltChest new_body = new CharacterComponentLister.PreBuiltChest();
        customisationIDs[index][BODY_ID]--;
        switch (componentRegistries[index].meshComponentList.meshList)
        {
            case 0:
                if (customisationIDs[index][BODY_ID] < 0)
                {
                    customisationIDs[index][BODY_ID] = charCompLister.lightComponents.chest.Count - 1;
                }
                new_body = charCompLister.lightComponents.chest[customisationIDs[index][BODY_ID]];
                break;
            case 1:
                if (customisationIDs[index][BODY_ID] < 0)
                {
                    customisationIDs[index][BODY_ID] = charCompLister.mediumComponents.chest.Count - 1;
                }
                new_body = charCompLister.mediumComponents.chest[customisationIDs[index][BODY_ID]];
                break;
            case 2:
                if (customisationIDs[index][BODY_ID] < 0)
                {
                    customisationIDs[index][BODY_ID] = charCompLister.heavyComponents.chest.Count - 1;
                }
                new_body = charCompLister.heavyComponents.chest[customisationIDs[index][BODY_ID]];
                break;
        }
        UnityEngine.Debug.Log(customisationIDs[index][BODY_ID]);
        onBodyChange(index, new_body);
    }

    public void onBodyChange(int index, CharacterComponentLister.PreBuiltChest new_body)
    {
        if(new_body.torso != null)
        {
            componentRegistries[index].meshComponentList.torso.sharedMesh = new_body.torso.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            //componentRegistries[index].meshComponentList.torso.bounds = new_body.torso.GetComponent<SkinnedMeshRenderer>().bounds;
            componentRegistries[index].meshComponentList.torso.materials = new_body.torso.GetComponent<SkinnedMeshRenderer>().materials;
            componentRegistries[index].meshComponentList.torso.enabled = true;
        }
        else
        {
            componentRegistries[index].meshComponentList.torso.enabled = false;
        }

        if (new_body.armUpperRight != null)
        {
            componentRegistries[index].meshComponentList.armUpperRight.sharedMesh = new_body.armUpperRight.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            //componentRegistries[index].meshComponentList.armUpperRight.bounds = new_body.armUpperRight.GetComponent<SkinnedMeshRenderer>().bounds;
            componentRegistries[index].meshComponentList.armUpperRight.materials = new_body.armUpperRight.GetComponent<SkinnedMeshRenderer>().materials;
            componentRegistries[index].meshComponentList.armUpperRight.enabled = true;
        }
        else
        {
            componentRegistries[index].meshComponentList.armUpperRight.enabled = false;
        }

        if (new_body.armUpperLeft != null)
        {
            componentRegistries[index].meshComponentList.armUpperLeft.sharedMesh = new_body.armUpperLeft.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            //componentRegistries[index].meshComponentList.armUpperLeft.bounds = new_body.armUpperLeft.GetComponent<SkinnedMeshRenderer>().bounds;
            componentRegistries[index].meshComponentList.armUpperLeft.materials = new_body.armUpperLeft.GetComponent<SkinnedMeshRenderer>().materials;
            componentRegistries[index].meshComponentList.armUpperLeft.enabled = true;
        }
        else
        {
            componentRegistries[index].meshComponentList.armUpperLeft.enabled = false;
        }

        if (new_body.armLowerRight != null)
        {
            componentRegistries[index].meshComponentList.armLowerRight.sharedMesh = new_body.armLowerRight.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            //componentRegistries[index].meshComponentList.armLowerRight.bounds = new_body.armLowerRight.GetComponent<SkinnedMeshRenderer>().bounds;
            componentRegistries[index].meshComponentList.armLowerRight.materials = new_body.armLowerRight.GetComponent<SkinnedMeshRenderer>().materials;
            componentRegistries[index].meshComponentList.armLowerRight.enabled = true;
        }
        else
        {
            componentRegistries[index].meshComponentList.armLowerRight.enabled = false;
        }

        if (new_body.armLowerLeft != null)
        {
            componentRegistries[index].meshComponentList.armLowerLeft.sharedMesh = new_body.armLowerLeft.GetComponent<SkinnedMeshRenderer>().sharedMesh;
           // componentRegistries[index].meshComponentList.armLowerLeft.bounds = new_body.armLowerLeft.GetComponent<SkinnedMeshRenderer>().bounds;
            componentRegistries[index].meshComponentList.armLowerLeft.materials = new_body.armLowerLeft.GetComponent<SkinnedMeshRenderer>().materials;
            componentRegistries[index].meshComponentList.armLowerLeft.enabled = true;
        }
        else
        {
            componentRegistries[index].meshComponentList.armLowerLeft.enabled = false;
        }

        if (new_body.handRight != null)
        {
            componentRegistries[index].meshComponentList.handRight.sharedMesh = new_body.handRight.GetComponent<SkinnedMeshRenderer>().sharedMesh;
           // componentRegistries[index].meshComponentList.handRight.bounds = new_body.handRight.GetComponent<SkinnedMeshRenderer>().bounds;
            componentRegistries[index].meshComponentList.handRight.materials = new_body.handRight.GetComponent<SkinnedMeshRenderer>().materials;
            componentRegistries[index].meshComponentList.handRight.enabled = true;
        }
        else
        {
            componentRegistries[index].meshComponentList.handRight.enabled = false;
        }

        if (new_body.handLeft != null)
        {
            componentRegistries[index].meshComponentList.handLeft.sharedMesh = new_body.handLeft.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            //componentRegistries[index].meshComponentList.handLeft.bounds = new_body.handLeft.GetComponent<SkinnedMeshRenderer>().bounds;
            componentRegistries[index].meshComponentList.handLeft.materials = new_body.handLeft.GetComponent<SkinnedMeshRenderer>().materials;
            componentRegistries[index].meshComponentList.handLeft.enabled = true;
        }
        else
        {
            componentRegistries[index].meshComponentList.handLeft.enabled = false;
        }

        if (new_body.shoulderRight != null)
        {
            componentRegistries[index].meshComponentList.shoulderRight.sharedMesh = new_body.shoulderRight.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            //componentRegistries[index].meshComponentList.shoulderRight.bounds = new_body.shoulderRight.GetComponent<SkinnedMeshRenderer>().bounds;
            componentRegistries[index].meshComponentList.shoulderRight.materials = new_body.shoulderRight.GetComponent<SkinnedMeshRenderer>().materials;
            componentRegistries[index].meshComponentList.shoulderRight.enabled = true;
        }
        else
        {
            componentRegistries[index].meshComponentList.shoulderRight.enabled = false;
        }

        if (new_body.shoulderLeft != null)
        {
            componentRegistries[index].meshComponentList.shoulderLeft.sharedMesh = new_body.shoulderLeft.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            //componentRegistries[index].meshComponentList.shoulderLeft.bounds = new_body.shoulderLeft.GetComponent<SkinnedMeshRenderer>().bounds;
            componentRegistries[index].meshComponentList.shoulderLeft.materials = new_body.shoulderLeft.GetComponent<SkinnedMeshRenderer>().materials;
            componentRegistries[index].meshComponentList.shoulderLeft.enabled = true;
        }
        else
        {
            componentRegistries[index].meshComponentList.shoulderLeft.enabled = false;
        }

        if (new_body.elbowRight != null)
        {
            componentRegistries[index].meshComponentList.elbowRight.sharedMesh = new_body.elbowRight.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            //componentRegistries[index].meshComponentList.elbowRight.bounds = new_body.elbowRight.GetComponent<SkinnedMeshRenderer>().bounds;
            componentRegistries[index].meshComponentList.elbowRight.materials = new_body.elbowRight.GetComponent<SkinnedMeshRenderer>().materials;
            componentRegistries[index].meshComponentList.elbowRight.enabled = true;
        }
        else
        {
            componentRegistries[index].meshComponentList.elbowRight.enabled = false;
        }

        if (new_body.elbowLeft != null)
        {
            componentRegistries[index].meshComponentList.elbowLeft.sharedMesh = new_body.elbowLeft.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            //componentRegistries[index].meshComponentList.elbowLeft.bounds = new_body.elbowLeft.GetComponent<SkinnedMeshRenderer>().bounds;
            componentRegistries[index].meshComponentList.elbowLeft.materials = new_body.elbowLeft.GetComponent<SkinnedMeshRenderer>().materials;
            componentRegistries[index].meshComponentList.elbowLeft.enabled = true;
        }
        else
        {
            componentRegistries[index].meshComponentList.elbowLeft.enabled = false;
        }

        if (new_body.backAttachment != null)
        {
            componentRegistries[index].meshComponentList.backAttachment.sharedMesh = new_body.backAttachment.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            //componentRegistries[index].meshComponentList.backAttachment.bounds = new_body.backAttachment.GetComponent<SkinnedMeshRenderer>().bounds;
            componentRegistries[index].meshComponentList.backAttachment.materials = new_body.backAttachment.GetComponent<SkinnedMeshRenderer>().materials;
            componentRegistries[index].meshComponentList.backAttachment.enabled = true;
        }
        else
        {
            componentRegistries[index].meshComponentList.shoulderRight.enabled = false;
        }
}

    const int LEGS_ID = 3;
    public void NextLegs(int index)
    {
        var new_legs = new CharacterComponentLister.PreBuiltLegs();
        customisationIDs[index][LEGS_ID]++;
        switch (componentRegistries[index].meshComponentList.meshList)
        {
            case 0:
                if (customisationIDs[index][LEGS_ID] >= charCompLister.lightComponents.legs.Count)
                {
                    customisationIDs[index][LEGS_ID] = 0;
                }
                new_legs = charCompLister.lightComponents.legs[customisationIDs[index][LEGS_ID]];
                break;
            case 1:
                if (customisationIDs[index][LEGS_ID] >= charCompLister.mediumComponents.legs.Count)
                {
                    customisationIDs[index][LEGS_ID] = 0;
                }
                new_legs = charCompLister.mediumComponents.legs[customisationIDs[index][LEGS_ID]];
                break;
            case 2:
                if (customisationIDs[index][LEGS_ID] >= charCompLister.heavyComponents.legs.Count)
                {
                    customisationIDs[index][LEGS_ID] = 0;
                }
                new_legs = charCompLister.heavyComponents.legs[customisationIDs[index][LEGS_ID]];
                break;
        }
        onLegsChange(index, new_legs);
    }

    public void PrevLegs(int index)
    {
        var new_legs = new CharacterComponentLister.PreBuiltLegs();
        customisationIDs[index][LEGS_ID]--;
        switch (componentRegistries[index].meshComponentList.meshList)
        {
            case 0:
                if (customisationIDs[index][LEGS_ID] < 0)
                {
                    customisationIDs[index][LEGS_ID] = charCompLister.lightComponents.legs.Count - 1;
                }
                new_legs = charCompLister.lightComponents.legs[customisationIDs[index][LEGS_ID]];
                break;
            case 1:
                if (customisationIDs[index][LEGS_ID] < 0)
                {
                    customisationIDs[index][LEGS_ID] = charCompLister.mediumComponents.legs.Count - 1;
                }
                new_legs = charCompLister.mediumComponents.legs[customisationIDs[index][LEGS_ID]];
                break;
            case 2:
                if (customisationIDs[index][LEGS_ID] < 0)
                {
                    customisationIDs[index][LEGS_ID] = charCompLister.heavyComponents.legs.Count - 1;
                }
                new_legs = charCompLister.heavyComponents.legs[customisationIDs[index][LEGS_ID]];
                break;
        }
        UnityEngine.Debug.Log(customisationIDs[index][LEGS_ID]);
        onLegsChange(index, new_legs);
    }

    public void onLegsChange(int index, CharacterComponentLister.PreBuiltLegs new_head)
    {
        if (new_head.hips != null)
        {
            componentRegistries[index].meshComponentList.hips.sharedMesh = new_head.hips.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            //componentRegistries[index].meshComponentList.hips.bounds = new_head.hips.GetComponent<SkinnedMeshRenderer>().bounds;
            componentRegistries[index].meshComponentList.hips.materials = new_head.hips.GetComponent<SkinnedMeshRenderer>().materials;
            componentRegistries[index].meshComponentList.hips.enabled = true;
        }
        else
        {
            componentRegistries[index].meshComponentList.hips.enabled = false;
        }

        if (new_head.legRight != null)
        {
            componentRegistries[index].meshComponentList.legRight.sharedMesh = new_head.legRight.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            //componentRegistries[index].meshComponentList.legRight.bounds = new_head.legRight.GetComponent<SkinnedMeshRenderer>().bounds;
            componentRegistries[index].meshComponentList.legRight.materials = new_head.legRight.GetComponent<SkinnedMeshRenderer>().materials;
            componentRegistries[index].meshComponentList.legRight.enabled = true;
        }
        else
        {
            componentRegistries[index].meshComponentList.legRight.enabled = false;
        }

        if (new_head.legLeft != null)
        {
            componentRegistries[index].meshComponentList.legLeft.sharedMesh = new_head.legLeft.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            //componentRegistries[index].meshComponentList.legLeft.bounds = new_head.legLeft.GetComponent<SkinnedMeshRenderer>().bounds;
            componentRegistries[index].meshComponentList.legLeft.materials = new_head.legLeft.GetComponent<SkinnedMeshRenderer>().materials;
            componentRegistries[index].meshComponentList.legLeft.enabled = true;
        }
        else
        {
            componentRegistries[index].meshComponentList.legLeft.enabled = false;
        }

        if (new_head.kneeAttachmentRight != null)
        {
            componentRegistries[index].meshComponentList.KneeAttachmentRight.sharedMesh = new_head.kneeAttachmentRight.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            //componentRegistries[index].meshComponentList.KneeAttachmentRight.bounds = new_head.kneeAttachmentRight.GetComponent<SkinnedMeshRenderer>().bounds;
            componentRegistries[index].meshComponentList.KneeAttachmentRight.materials = new_head.kneeAttachmentRight.GetComponent<SkinnedMeshRenderer>().materials;
            componentRegistries[index].meshComponentList.KneeAttachmentRight.enabled = true;
        }
        else
        {
            componentRegistries[index].meshComponentList.KneeAttachmentRight.enabled = false;
        }

        if (new_head.kneeAttachmentLeft != null)
        {
            componentRegistries[index].meshComponentList.kneeAttachmentLeft.sharedMesh = new_head.kneeAttachmentLeft.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            //componentRegistries[index].meshComponentList.kneeAttachmentLeft.bounds = new_head.kneeAttachmentLeft.GetComponent<SkinnedMeshRenderer>().bounds;
            componentRegistries[index].meshComponentList.kneeAttachmentLeft.materials = new_head.kneeAttachmentLeft.GetComponent<SkinnedMeshRenderer>().materials;
            componentRegistries[index].meshComponentList.kneeAttachmentLeft.enabled = true;
        }
        else
        {
            componentRegistries[index].meshComponentList.kneeAttachmentLeft.enabled = false;
        }

        if (new_head.hipsAttachment != null)
        {
            componentRegistries[index].meshComponentList.hipsAttachment.sharedMesh = new_head.hipsAttachment.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            //componentRegistries[index].meshComponentList.hipsAttachment.bounds = new_head.hipsAttachment.GetComponent<SkinnedMeshRenderer>().bounds;
            componentRegistries[index].meshComponentList.hipsAttachment.materials = new_head.hipsAttachment.GetComponent<SkinnedMeshRenderer>().materials;
            componentRegistries[index].meshComponentList.hipsAttachment.enabled = true;
        }
        else
        {
            componentRegistries[index].meshComponentList.hipsAttachment.enabled = false;
        }
    }

    const int HAIR_ID = 0;
    public void NextHair(int index)
    {
        customisationIDs[index][HAIR_ID]++;
        switch (componentRegistries[index].meshComponentList.meshList)
        {
            case 0:
                if (customisationIDs[index][HAIR_ID] >= charCompLister.lightComponents.hair.Count)
                {
                    customisationIDs[index][HAIR_ID] = 0;
                }
                onHairChange(index, charCompLister.lightComponents.hair[customisationIDs[index][HAIR_ID]]);
                break;
            case 1:
                if (customisationIDs[index][HAIR_ID] >= charCompLister.mediumComponents.hair.Count)
                {
                    customisationIDs[index][HAIR_ID] = 0;
                }
                onHairChange(index, charCompLister.mediumComponents.hair[customisationIDs[index][HAIR_ID]]);
                break;
            case 2:
                if (customisationIDs[index][HAIR_ID] >= charCompLister.heavyComponents.hair.Count)
                {
                    customisationIDs[index][HAIR_ID] = 0;
                }
                onHairChange(index, charCompLister.heavyComponents.hair[customisationIDs[index][HAIR_ID]]);
                break;
            default:
                if (customisationIDs[index][HAIR_ID] >= charCompLister.lightComponents.hair.Count)
                {
                    customisationIDs[index][HAIR_ID] = 0;
                }
                onHairChange(index, charCompLister.lightComponents.hair[customisationIDs[index][HAIR_ID]]);
                break;
        }
    }

    public void PrevHair(int index)
    {
        customisationIDs[index][HAIR_ID]--;
        switch (componentRegistries[index].meshComponentList.meshList)
        {
            case 0:
                if (customisationIDs[index][HAIR_ID] < 0)
                {
                    customisationIDs[index][HAIR_ID] = charCompLister.lightComponents.hair.Count - 1;
                }
                onHairChange(index, charCompLister.lightComponents.hair[customisationIDs[index][HAIR_ID]]);
                break;
            case 1:
                if (customisationIDs[index][HAIR_ID] < 0)
                {
                    customisationIDs[index][HAIR_ID] = charCompLister.mediumComponents.hair.Count - 1;
                }
                onHairChange(index, charCompLister.mediumComponents.hair[customisationIDs[index][HAIR_ID]]);
                break;
            case 2:
                if (customisationIDs[index][HAIR_ID] < 0)
                {
                    customisationIDs[index][HAIR_ID] = charCompLister.heavyComponents.hair.Count - 1;
                }
                onHairChange(index, charCompLister.heavyComponents.hair[customisationIDs[index][HAIR_ID]]);
                break;
            default:
                if (customisationIDs[index][HAIR_ID] >= charCompLister.lightComponents.hair.Count)
                {
                    customisationIDs[index][HAIR_ID] = 0;
                }
                onHairChange(index, charCompLister.lightComponents.hair[customisationIDs[index][HAIR_ID]]);
                break;
        }
        UnityEngine.Debug.Log(customisationIDs[index][HAIR_ID]);
    }

    public void onHairChange(int index, GameObject new_hair)
    {
        if (new_hair != null)
        {
            componentRegistries[index].meshComponentList.hair.sharedMesh = new_hair.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            //componentRegistries[index].meshComponentList.hair.bounds = new_hair.GetComponent<SkinnedMeshRenderer>().bounds;
            componentRegistries[index].meshComponentList.hair.materials = new_hair.GetComponent<SkinnedMeshRenderer>().materials;
            componentRegistries[index].meshComponentList.hair.enabled = true;
        }
        else
        {
            componentRegistries[index].meshComponentList.hair.enabled = false;
        }
    }

    #endregion

    const int BODY_ART_COLOUR = 4;
    public void ChangeColour(int index, int partID)
    {
        colourIDs[index][partID]++;
        int target_max = 0;
        switch (partID)
        {
            case HAIR_ID:
                target_max = charCompLister.hairColours.Length;
                break;
            case HEAD_ID:
                target_max = charCompLister.skinColours.Length;
                break;
            case BODY_ID:
                target_max = charCompLister.primaryColours.Length / 4;
                break;
            case LEGS_ID:
                target_max = charCompLister.metalDarkColours.Length;
                break;
        }

        if (colourIDs[index][partID] >= target_max)
        {
            colourIDs[index][partID] = 0;
            if (partID == HEAD_ID)
            {
                colourIDs[index][BODY_ART_COLOUR]++;
                if (colourIDs[index][BODY_ART_COLOUR] >= charCompLister.bodyArtColours.Length)
                {
                    colourIDs[index][BODY_ART_COLOUR] = 0;
                }
            }
        }
    }

    private void updatePlayerColours(int index)
    {
        foreach (var meshPart in componentRegistries[index].meshComponentList.listOfMeshes)
        {
            meshPart.material.SetColor("_Color_BodyArt", charCompLister.bodyArtColours[colourIDs[index][BODY_ART_COLOUR]]);
            meshPart.material.SetColor("_Color_Hair", charCompLister.hairColours[colourIDs[index][HAIR_ID]]);
            meshPart.material.SetColor("_Color_Stubble", charCompLister.hairColours[colourIDs[index][HAIR_ID]]);
            meshPart.material.SetColor("_Color_Skin", charCompLister.skinColours[colourIDs[index][HEAD_ID]]);
            meshPart.material.SetColor("_Color_Primary", charCompLister.primaryColours[colourIDs[index][BODY_ID] * playerClassRotation.Length + playerClassID[index]]);
            meshPart.material.SetColor("_Color_Secondary", charCompLister.secondaryColours[colourIDs[index][BODY_ID] * playerClassRotation.Length + playerClassID[index]]);
            meshPart.material.SetColor("_Color_Metal_Dark", charCompLister.metalDarkColours[colourIDs[index][LEGS_ID]]);
            meshPart.material.SetColor("_Color_Leather_Primary", charCompLister.leatherColours[colourIDs[index][LEGS_ID]]);
            meshPart.material.SetColor("_Color_Leather_Secondary", charCompLister.leatherSecondaryColours[colourIDs[index][LEGS_ID]]);
        }
    }

    public void NextClass(int index)
    {
        if (componentRegistries[index] != null && players[index] != null)
        {
            playerClassID[index]++;
            if (playerClassID[index] == playerClassRotation.Count())
            {
                playerClassID[index] = 0;
            }
            onClassChange(index);
        }
    }

    public void previousClass(int index)
    {
        if (componentRegistries[index] != null && players[index] != null)
        {
            playerClassID[index]--;
            if (playerClassID[index] == -1)
            {
                playerClassID[index] = playerClassRotation.Count()-1;
            }
            onClassChange(index);
        }
    }

    public void onClassChange(int index)
    {
        var playerDevice = componentRegistries[index].playerInput.devices.FirstOrDefault<InputDevice>();
        var playerControlScheme = componentRegistries[index].playerInput.currentControlScheme;
        players[index].SetActive(false);
        UnityEngine.Debug.Log("Player " + index.ToString() + " is cycling class...");
        PlayerInput.Instantiate(playerClassRotation[playerClassID[index]], index, playerControlScheme, index, playerDevice);
        updateAfterDestroy = true;
        for(int i = 0; i < customisationIDs[index].Count; i++) 
        {
            customisationIDs[index][i] = 0;
        }
    }
}
