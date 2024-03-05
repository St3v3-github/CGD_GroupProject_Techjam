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
    List<bool> spaceTaken = new List<bool>();
    List<int> playerClassID = new List<int>();
    public List<GameObject> players;
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
        int newPlayerSpace = findSpace();
        toJoinDisplays[newPlayerSpace].SetActive(false);
        playerSetupMenus[newPlayerSpace].SetActive(true);

        UnityEngine.Debug.Log("Handling Player, about to run the foreach loop.");
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player);
        }
        UnityEngine.Debug.Log("New player is being added...");
        players[players.Count - 1].GetComponent<UpdatedPlayerController>().enabled = false;
        players[players.Count - 1].GetComponentInChildren<Camera>().enabled = false;
        players[players.Count - 1].GetComponent<Rigidbody>().MovePosition(characterPositions[newPlayerSpace].transform.position);
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
        var playerInputSave = players[index].GetComponent<PlayerInput>();
        var playerObjectSave = players[index];
        playerClassID[index]++;
        if (playerClassID[index] == playerClassRotation.Count())
        {
            playerClassID[index] = 0;
        }
        var newPlayer = Instantiate(playerClassRotation[playerClassID[index]], players[index].transform.position, players[index].transform.rotation);
        players[index] = newPlayer;
        players[index].GetComponent<UpdatedPlayerController>().enabled = false;
        players[index].GetComponentInChildren<Camera>().enabled = false;
        //Change inputs? TODO: Destroy player
        //players[index].GetComponent<PlayerInput>().actions = playerInputSave.actions;
        GameObject.Destroy(playerObjectSave);
        UnityEngine.Debug.Log("Player " + index.ToString() + " is cycling class...");

    }
}
