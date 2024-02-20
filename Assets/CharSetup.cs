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
    public List<ComponentRegistry> componentRegistries;
    List<bool> spaceTaken = new List<bool>();
    List<int> playerClassID = new List<int>();
    public List<GameObject> players;
    public PlayerInputManager inputManager;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0;i<maxPlayers;i++)
        {
            spaceTaken.Add(false);
            playerClassID.Add(0);
        }
    }

    public void handleNewPlayer()
    {
        
        players = new List<GameObject>();
        componentRegistries = new List<ComponentRegistry>();
        int newPlayerSpace = findSpace();
        toJoinDisplays[newPlayerSpace].SetActive(false);
        playerSetupMenus[newPlayerSpace].SetActive(true);

        UnityEngine.Debug.Log("Handling Player, about to run the foreach loop.");
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player);
            componentRegistries.Add(player.GetComponent<ComponentRegistry>());
        }
        UnityEngine.Debug.Log("New player is being added...");
        componentRegistries[players.Count - 1].inputManager.enabled = false;
        componentRegistries[players.Count - 1].advancedProjectileSystem.enabled = false;
        componentRegistries[players.Count - 1].playerCamera.enabled = false;
        componentRegistries[players.Count - 1].rigidBody.MovePosition(characterPositions[newPlayerSpace].transform.position);
    }

    public int findSpace()
    {
        for (int i = 0; i < maxPlayers; i++)
        {
            if (!spaceTaken[i])
            {
                spaceTaken[i] = true;
                return i;
            }
        }
        return 0;
    }

    public void nextClass(int index)
    {
        var playerDevice = componentRegistries[index].playerInput.devices;
        var playerControlScheme = componentRegistries[index].playerInput.currentControlScheme;
        var playerObjectSave = players[index];
        Destroy(players[index]);
        playerClassID[index]++;
        if (playerClassID[index] == playerClassRotation.Count())
        {
            playerClassID[index] = 0;
        }
        PlayerInput.Instantiate(playerClassRotation[playerClassID[index]], index, playerControlScheme, index, playerDevice[0]);

        players = new List<GameObject>();
        componentRegistries = new List<ComponentRegistry>();
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player);
            componentRegistries.Add(player.GetComponent<ComponentRegistry>());
        }

        componentRegistries[players.Count - 1].inputManager.enabled = false;
        componentRegistries[players.Count - 1].advancedProjectileSystem.enabled = false;
        componentRegistries[players.Count - 1].playerCamera.enabled = false;
        //Change inputs? TODO: Destroy player
        //players[index].GetComponent<PlayerInput>().actions = playerInputSave.actions;
        UnityEngine.Debug.Log("Player " + index.ToString() + " is cycling class...");

    }
}
