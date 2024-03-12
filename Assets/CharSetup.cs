using FMOD;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public float[] inputClocks;
    public int maxSelection = 1;
    public PlayerInputManager inputManager;
    public bool kwikfix = true;

    public bool updateAfterDestroy = false;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < maxPlayers; i++)
        {
            //spaceTaken.Add(false);
            playerClassID.Add(0);
        }
        menuSelections = new int[maxPlayers];
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
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            var componentRegistry = player.GetComponent<ComponentRegistry>();
            if (componentRegistry.playerInput.playerIndex != -1)
            {
                players[componentRegistry.playerInput.playerIndex] = player;
                menuSelections[componentRegistry.playerInput.playerIndex] = 0;
                componentRegistries[componentRegistry.playerInput.playerIndex] = componentRegistry;
                componentRegistry.playerScoreInfo.team = componentRegistry.playerInput.playerIndex;
                componentRegistry.inputManager.enabled = false;
                componentRegistry.playerInput.actions.FindAction("MenuRight").performed += NextClass;
                componentRegistry.playerInput.actions.FindAction("MenuLeft").performed += previousClass;
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
    public void NextClass(InputAction.CallbackContext ctx)
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
                        NextClass(i);
                        break;
                    }
                }
            }
        }
    }
    public void previousClass(InputAction.CallbackContext ctx)
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
                        previousClass(i);
                        break;
                    }
                }
            }
        }
    }

    public void NextClass(int index)
    {
        if (kwikfix && componentRegistries[index] != null && players[index] != null)
        {
            var playerDevice = componentRegistries[index].playerInput.devices.FirstOrDefault<InputDevice>();
            var playerControlScheme = componentRegistries[index].playerInput.currentControlScheme;
            playerClassID[index]++;
            if (playerClassID[index] == playerClassRotation.Count())
            {
                playerClassID[index] = 0;
            }
            players[index].SetActive(false);
            UnityEngine.Debug.Log("Player " + index.ToString() + " is cycling class...");
            PlayerInput.Instantiate(playerClassRotation[playerClassID[index]], index, playerControlScheme, index, playerDevice);
            updateAfterDestroy = true;
        }
    }

    public void previousClass(int index)
    {
        if (kwikfix && componentRegistries[index] != null && players[index] != null)
        {
            var playerDevice = componentRegistries[index].playerInput.devices.FirstOrDefault<InputDevice>();
            var playerControlScheme = componentRegistries[index].playerInput.currentControlScheme;
            playerClassID[index]--;
            if (playerClassID[index] == -1)
            {
                playerClassID[index] = playerClassRotation.Count()-1;
            }
            players[index].SetActive(false);
            UnityEngine.Debug.Log("Player " + index.ToString() + " is cycling class...");
            PlayerInput.Instantiate(playerClassRotation[playerClassID[index]], index, playerControlScheme, index, playerDevice);
            updateAfterDestroy = true;
        }
    }
}
