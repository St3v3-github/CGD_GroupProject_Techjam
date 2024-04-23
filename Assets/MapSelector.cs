using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MapSelector : MonoBehaviour
{

    public List<GameObject> portals = new List<GameObject>();
    public List<int> votes = new List<int>();
    public int minVotes = 2;
    public int playerCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] discoveredPortals = GameObject.FindGameObjectsWithTag("Portal");
        foreach (GameObject portal in discoveredPortals)
        {
            portals.Add(portal);
            votes.Add(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int votesCount = 0;
        foreach (int vote in votes)
        {
            votesCount += vote;
        }
        if (votesCount >= minVotes && votesCount >= playerCount)
        {
            Debug.Log("LOAD MAP HERE");
            //LOAD MAP HERE
        }

    }

    public void IncrementVote(GameObject targetPortal)
    {
        for (int i = 0; portals.Count > i; i++)
        {
            if (targetPortal = portals[i])
            {
                votes[i]++;
            }
        }

    }
    public void DecreaseVote(GameObject targetPortal)
    {
        for (int i = 0; portals.Count > i; i++)
        {
            if (targetPortal = portals[i])
            {
                votes[i]--;
            }
        }
    }
}

