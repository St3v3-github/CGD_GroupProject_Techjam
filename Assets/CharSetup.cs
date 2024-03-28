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

    /// <summary>
    /// Currently left and right movement is hard coded to change class. In the future a switch handler can be implemented.
    /// </summary>
    /// <param name="ctx"></param>
    
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
                            menuSelections[i] = 0;
                        }
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
                                        break;
                                    case 1:
                                        break;
                                    case 2:
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
                                        break;
                                    case 1:
                                        break;
                                    case 2:
                                        break;
                                }
                                break;
                            case CharMenuLevels.LEGS_CUSTOM:
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

    public void NextHead(int index)
    {
        CharacterComponentLister.PreBuiltHead new_head = new CharacterComponentLister.PreBuiltHead();
        customisationIDs[index][0]++;
        switch(componentRegistries[index].meshComponentList.meshList)
        {
            case 0:
                if (customisationIDs[index][0] >= charCompLister.lightComponents.head.Count)
                {
                    customisationIDs[index][0] = 0;
                }
                new_head = charCompLister.lightComponents.head[customisationIDs[index][0]];
                break;
            case 1:
                if (customisationIDs[index][0] >= charCompLister.mediumComponents.head.Count)
                {
                    customisationIDs[index][0] = 0;
                }
                new_head = charCompLister.mediumComponents.head[customisationIDs[index][0]];
                break;
            case 2:
                if (customisationIDs[index][0] >= charCompLister.heavyComponents.head.Count)
                {
                    customisationIDs[index][0] = 0;
                }
                new_head = charCompLister.heavyComponents.head[customisationIDs[index][0]];
                break;
        }
        onHeadChange(index, new_head);
    }

    public void PrevHead(int index)
    {
        CharacterComponentLister.PreBuiltHead new_head = new CharacterComponentLister.PreBuiltHead();
        customisationIDs[index][0]--;
        switch (componentRegistries[index].meshComponentList.meshList)
        {
            case 0:
                if (customisationIDs[index][0] < 0)
                {
                    customisationIDs[index][0] = charCompLister.lightComponents.head.Count - 1;
                }
                new_head = charCompLister.lightComponents.head[customisationIDs[index][0]];
                break;
            case 1:
                if (customisationIDs[index][0] < 0)
                {
                    customisationIDs[index][0] = charCompLister.mediumComponents.head.Count - 1;
                }
                new_head = charCompLister.mediumComponents.head[customisationIDs[index][0]];
                break;
            case 2:
                if (customisationIDs[index][0] < 0)
                {
                    customisationIDs[index][0] = charCompLister.heavyComponents.head.Count - 1;
                }
                new_head = charCompLister.heavyComponents.head[customisationIDs[index][0]];
                break;
        }
        UnityEngine.Debug.Log(customisationIDs[index][0]);
        onHeadChange(index, new_head);
    }

    public void onHeadChange(int index, CharacterComponentLister.PreBuiltHead new_head)
    {
        if(new_head.head != null)
        {
            componentRegistries[index].meshComponentList.head.sharedMesh = new_head.head.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            componentRegistries[index].meshComponentList.head.bounds = new_head.head.GetComponent<SkinnedMeshRenderer>().bounds;
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
            componentRegistries[index].meshComponentList.headAccessory.bounds = new_head.accessory.GetComponent<SkinnedMeshRenderer>().bounds;
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
            componentRegistries[index].meshComponentList.facialHair.bounds = new_head.facialHair.GetComponent<SkinnedMeshRenderer>().bounds;
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
            componentRegistries[index].meshComponentList.eyebrow.bounds = new_head.eyebrow.GetComponent<SkinnedMeshRenderer>().bounds;
            componentRegistries[index].meshComponentList.eyebrow.materials = new_head.eyebrow.GetComponent<SkinnedMeshRenderer>().materials;
            componentRegistries[index].meshComponentList.eyebrow.enabled = true;
        }
        else
        {
            componentRegistries[index].meshComponentList.eyebrow.enabled = false;
        }
    }

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
