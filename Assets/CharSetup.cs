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
    public GameObject[] characterPositions;
    public GameObject[] toJoinDisplays;
    public GameObject[] playerSetupMenus;
    public GameObject[] playerClassRotation;
    public ComponentRegistry[] componentRegistries;
    //List<bool> spaceTaken = new List<bool>();
    List<int> playerClassID = new List<int>();
    public GameObject[] players;
    public PlayerInputManager inputManager;

    public bool updateAfterDestroy = false;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0;i<maxPlayers;i++)
        {
            //spaceTaken.Add(false);
            playerClassID.Add(0);
        }
    }

    private void LateUpdate()
    {
        if (updateAfterDestroy)
        {
            updateAfterDestroy = false;
            foreach (var player in GameObject.FindObjectsOfType<ComponentRegistry>(true))
            {
                if(player.gameObject.activeSelf == false)
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
            UnityEngine.Debug.Log("Player Devices: " + componentRegistry.playerInput.devices.ToString());
            if (componentRegistry.playerInput.playerIndex != -1)
            {
                UnityEngine.Debug.Log("Player Index: " + componentRegistry.playerInput.playerIndex.ToString());
                players[componentRegistry.playerInput.playerIndex] = player;
                componentRegistries[componentRegistry.playerInput.playerIndex] = componentRegistry;

                componentRegistry.inputManager.enabled = false;
                componentRegistry.advancedProjectileSystem.enabled = false;
                componentRegistry.playerCamera.enabled = false;
                componentRegistry.playerController.enabled = false;
                componentRegistry.rigidBody.MovePosition(characterPositions[componentRegistry.playerInput.playerIndex].transform.position);
                toJoinDisplays[componentRegistry.playerInput.playerIndex].SetActive(false);
                playerSetupMenus[componentRegistry.playerInput.playerIndex].SetActive(true);
            }
        }
        UnityEngine.Debug.Log("New player is being added...");
    }

    public void NextClass(int index)
    {
        if (componentRegistries[index] != null && players[index] != null)
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
}
