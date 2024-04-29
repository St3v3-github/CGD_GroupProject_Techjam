using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelector : MonoBehaviour
{
    public List<GameObject> portals = new List<GameObject>();
    public List<int> votes = new List<int>();
    public int minVotes = 2;
    public int playerCount = 0;
    public LevelSelectController levelSelectController;
    public CharSetup charsetup;

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
            int mapIndex = 0;
            int currentHighest = 0;
            bool tied_votes = false;
            List<int> tiedVoteIndex = new List<int>();
            for (int i =0; i< votes.Count; i++)
            {
                if (votes[i] > 0 && votes[i] > currentHighest)
                {
                    mapIndex = i;
                    currentHighest = votes[i];
                    tied_votes = false;
                    tiedVoteIndex.Clear();
                    tiedVoteIndex.Add(votes[i]);
              
                }
                else if (votes[i] > 0 && votes[i] == currentHighest)
                {
                    tied_votes = true;
                    tiedVoteIndex.Add(votes[i]);
                }
            }
            string selectedScene = "";
            if (!tied_votes)
            {
                selectedScene = portals[mapIndex].name;
            }
            else
            {
               int randomIndex = Random.Range(1, tiedVoteIndex.Count);
                selectedScene = portals[randomIndex].name;
            }
            
            Debug.Log("LOAD MAP HERE");
            AudioManager.instance.EndMusic(FMODEvents.instance.music);
            levelSelectController.StartGameplayScene(selectedScene);
            //AudioManager.instance.Cleanup();
            //AudioManager.instance.InitializeMusic(FMODEvents.instance.music);
            
        }
      //  playerCount = charsetup.players.Length;
    }

    public void IncrementVote(GameObject targetPortal)
    {
        for (int i = 0; portals.Count > i; i++)
        {
            if (targetPortal == portals[i])
            {
                votes[i]++;
            }
        }

    }
    public void DecreaseVote(GameObject targetPortal)
    {
        for (int i = 0; portals.Count > i; i++)
        {
            if (targetPortal == portals[i])
            {
                votes[i]--;
            }
        }
    }
}