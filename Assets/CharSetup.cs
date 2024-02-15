using FMOD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSetup : MonoBehaviour
{
    public int maxPlayers;
    public GameObject[] characterPositions;
    public GameObject[] toJoinDisplays;
    public GameObject[] playerSetupMenus;
    List<bool> spaceTaken = new List<bool>();
   public List<GameObject> players;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0;i<maxPlayers;i++)
        {
            spaceTaken.Add(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void handleNewPlayer()
    {
        UnityEngine.Debug.Log("WE ARE IN THE HANDLE NEW PLAYER");
        players = new List<GameObject>();
        int newPlayerSpace = findSpace();
        toJoinDisplays[newPlayerSpace].SetActive(false);
        playerSetupMenus[newPlayerSpace].SetActive(true);
        
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player);
        }
        players[players.Count - 1].GetComponent<UpdatedPlayerController>().enabled = false;
        players[players.Count - 1].GetComponentInChildren<Camera>().enabled = false;
        players[players.Count - 1].transform.SetPositionAndRotation(characterPositions[newPlayerSpace].transform.position, characterPositions[newPlayerSpace].transform.rotation);
        UnityEngine.Debug.Log(players[players.Count - 1].name);
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
}
