using FMOD;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    List<int> playerClassID = new List<int>();
    public GameObject[] players;
    public int[] menuSelections;
    public CharMenuLevels[] menuLevels;
    public float[] inputClocks;
    public int maxSelection = 3;
    public PlayerInputManager inputManager;
    public bool kwikfix = true;
    public bool updateAfterDestroy = false;
    public List<int>[] customisationIDs;
    public const int CUSTOMISATION_PARTS = 4;
    public CharacterComponentLister charCompLister;
    public GameObject[] customisationMenus;

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
        customisationIDs = new List<int>[maxPlayers];
        for (int i = 0; i < maxPlayers; i++)
        {
            playerClassID.Add(0);
            customisationIDs[i] = new List<int>();
            for(int j = 0; j < CUSTOMISATION_PARTS; j++)
            {
                customisationIDs[i].Add(0);
            }
        }
        menuSelections = new int[maxPlayers];
        menuLevels = new CharMenuLevels[maxPlayers];
        inputClocks = new float[maxPlayers];
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
    }

    public void HandleNewPlayer()
    {
        int playernumber = 0;
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            var componentRegistry = player.GetComponent<ComponentRegistry>();
            if (componentRegistry.playerInput.playerIndex != -1)
            {
                playernumber++;
                switch (playernumber)
                {
                    case 1:
                        componentRegistry.mainMesh.GetComponent<CameraCulling>().SetGameLayerRecursive(componentRegistry.mainMesh,12);
                        break;
                    case 2:
                        componentRegistry.mainMesh.GetComponent<CameraCulling>().SetGameLayerRecursive(componentRegistry.mainMesh,13);
                        break;
                    case 3:
                        componentRegistry.mainMesh.GetComponent<CameraCulling>().SetGameLayerRecursive(componentRegistry.mainMesh,14);
                        break;
                    case 4:
                        componentRegistry.mainMesh.GetComponent<CameraCulling>().SetGameLayerRecursive(componentRegistry.mainMesh,15);
                        break;

                }
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
            }
        }
        UnityEngine.Debug.Log("New player is being added...");
    }

    public void LeaveCharSetup()
    {
         int playernumber = 0;
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            var componentRegistry = player.GetComponent<ComponentRegistry>();
            if (componentRegistry.playerInput.playerIndex != -1)
            {
                playernumber++;
               componentRegistry.inputManager.enabled = true;
                componentRegistry.playerCamera.enabled = true;
                componentRegistry.playerController.enabled = true;
                componentRegistry.rigidBody.MovePosition(tutorialPositions[componentRegistry.playerInput.playerIndex].transform.position);
                toJoinDisplays[componentRegistry.playerInput.playerIndex].SetActive(false);
                playerSetupMenus[componentRegistry.playerInput.playerIndex].SetActive(true);
            }
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
            }
        }

    }

    /// <summary>
    /// Currently left and right movement is hard coded to change class. In the future a switch handler can be implemented.
    /// </summary>
    /// <param name="ctx"></param>
    /// 
    public void onMenuChange(int index)
    {
        customisationMenus[index].GetComponent<CustomisationUIHandler>().UpdateUISeclections(menuLevels[index], menuSelections[index]);
        //Create an array to feed in first, toArray does not work
        customisationMenus[index].GetComponent<CustomisationUIHandler>().UpdateSelectionNames(customisationIDs[index].ToArray(), playerClassID[index]);
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
                                        //TODO: Colour picker
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
                                        break;
                                    case 1:
                                        break;
                                    case 2:
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
        }
    }

    #region CustomizationFunctions

    const int HEAD_ID = 0;
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

    const int BODY_ID = 1;
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

    const int LEGS_ID = 2;
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

    const int HAIR_ID = 3;
    public void NextHair(int index)
    {
        var new_hair = new GameObject();
        customisationIDs[index][HAIR_ID]++;
        switch (componentRegistries[index].meshComponentList.meshList)
        {
            case 0:
                if (customisationIDs[index][HAIR_ID] >= charCompLister.lightComponents.hair.Count)
                {
                    customisationIDs[index][HAIR_ID] = 0;
                }
                new_hair = charCompLister.lightComponents.hair[customisationIDs[index][HAIR_ID]];
                break;
            case 1:
                if (customisationIDs[index][HAIR_ID] >= charCompLister.mediumComponents.hair.Count)
                {
                    customisationIDs[index][HAIR_ID] = 0;
                }
                new_hair = charCompLister.mediumComponents.hair[customisationIDs[index][HAIR_ID]];
                break;
            case 2:
                if (customisationIDs[index][HAIR_ID] >= charCompLister.heavyComponents.hair.Count)
                {
                    customisationIDs[index][HAIR_ID] = 0;
                }
                new_hair = charCompLister.heavyComponents.hair[customisationIDs[index][HAIR_ID]];
                break;
        }
        onHairChange(index, new_hair);
        Destroy(new_hair);
    }

    public void PrevHair(int index)
    {
        var new_hair = new GameObject();
        customisationIDs[index][HAIR_ID]--;
        switch (componentRegistries[index].meshComponentList.meshList)
        {
            case 0:
                if (customisationIDs[index][HAIR_ID] < 0)
                {
                    customisationIDs[index][HAIR_ID] = charCompLister.lightComponents.hair.Count - 1;
                }
                new_hair = charCompLister.lightComponents.hair[customisationIDs[index][HAIR_ID]];
                break;
            case 1:
                if (customisationIDs[index][HAIR_ID] < 0)
                {
                    customisationIDs[index][HAIR_ID] = charCompLister.mediumComponents.hair.Count - 1;
                }
                new_hair = charCompLister.mediumComponents.hair[customisationIDs[index][HAIR_ID]];
                break;
            case 2:
                if (customisationIDs[index][HAIR_ID] < 0)
                {
                    customisationIDs[index][HAIR_ID] = charCompLister.heavyComponents.hair.Count - 1;
                }
                new_hair = charCompLister.heavyComponents.hair[customisationIDs[index][HAIR_ID]];
                break;
        }
        UnityEngine.Debug.Log(customisationIDs[index][HAIR_ID]);
        onHairChange(index, new_hair);
        Destroy(new_hair);
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

    public void copyMesh(SkinnedMeshRenderer original, SkinnedMeshRenderer new_mesh)
    {
        System.Type type = original.GetType();
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(original, field.GetValue(new_mesh));
        }
    }

    public void NextClass(int index)
    {
        if (kwikfix && componentRegistries[index] != null && players[index] != null)
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
        if (kwikfix && componentRegistries[index] != null && players[index] != null)
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
