using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeHandler : MonoBehaviour
{
    private class Team
    {
        public int team_kills = 0;
        public int team_deaths = 0;
    }

    public class GameRuleSetting
    {
        public float gameTime = 300.0f;
        public float respawnTimer = 3.0f;
        public float respawnThreshold = 100.0f;
        public int spawnPointCount = 0;
        public List<GameObject> playerCharacters = new List<GameObject>();
        public List<GameObject> initialSpawn = new List<GameObject>();
    }

    List<Team> teams;
    List<GameObject> players;
    List<GameObject> spawnPoints;
    List<float> spawnPointDistances;
    private float currentGameTime;
    float respawnThreshold;
    float respawnTimer;
    // Start is called before the first frame update
    void Start()
    {
        teams = new List<Team>();
        players = new List<GameObject>();
        spawnPoints = new List<GameObject>();

        foreach(var newSpawnPoint in GameObject.FindGameObjectsWithTag("SpawnPoint"))
        {
            spawnPoints.Add(newSpawnPoint);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var prayer in players)
        {
            AttributeManager keepHoldOfComp = prayer.GetComponent<AttributeManager>();
            if (keepHoldOfComp.currentHealth <= 0)
            {
                keepHoldOfComp.currentHealth = 0;
                keepHoldOfComp.healthbar.value = 0;
                prayer.transform.Find("AnimationController").GetComponent<AnimationManager>().toggleDeadBool(true);
                prayer.GetComponent<CharacterController>().enabled = false;
                var deadScoreInfo = prayer.GetComponent<PlayerScoreInfo>();
                var killerScoreInfo = deadScoreInfo.lastDamagedBy.GetComponent<PlayerScoreInfo>();
                killerScoreInfo.kill_count++;
                teams[killerScoreInfo.team].team_kills++;
                deadScoreInfo.death_count++;
                teams[deadScoreInfo.team].team_deaths++;

                StartCoroutine(reincarnatePlayer(prayer, FindSpawnPoint()));
            }
        }
    }

    private IEnumerator reincarnatePlayer(GameObject player, GameObject respawnPoint)
    {
        yield return new WaitForSeconds(respawnTimer);
        player.transform.Find("AnimationController").GetComponent<AnimationManager>().toggleDeadBool(false);
        //player.GetComponent<CharacterController>().enabled = false;
        player.transform.SetPositionAndRotation(respawnPoint.transform.position, respawnPoint.transform.rotation);
        player.GetComponent<CharacterController>().enabled = true;
        player.transform.GetComponent<AttributeManager>().currentHealth = player.transform.GetComponent<AttributeManager>().maxHealth;
    }

    private GameObject FindSpawnPoint()
    {
        int possibleSpawns = 0;
        for(int i = 0; i<spawnPoints.Count;i++)
        {
            spawnPointDistances[i] = float.MaxValue;
            foreach(var player in players)
            {
                float newDistance = Vector3.Distance(player.transform.position, spawnPoints[i].transform.position);
                if (newDistance < spawnPointDistances[i])
                {
                    spawnPointDistances[i] = newDistance;
                }
            }
            if (spawnPointDistances[i]>respawnThreshold)
            {
                possibleSpawns++;
            }
        }
        if (possibleSpawns != 0)
        {
            int randomNumber = UnityEngine.Random.Range(0, possibleSpawns);
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                if (spawnPointDistances[i] > respawnThreshold)
                {
                    if (randomNumber == 0)
                    {
                        return spawnPoints[i];
                    }
                    else
                    {
                        randomNumber--;
                    }
                }
            }
        }
        //Backdrop, if no spawn points are possible
        return spawnPoints[UnityEngine.Random.Range(0,spawnPoints.Count)];
    }

    public void LoadGameSettings(GameRuleSetting newSettings)
    {
        respawnThreshold = newSettings.respawnThreshold;
        respawnTimer = newSettings.respawnTimer;
        foreach(var newPlayer in newSettings.playerCharacters)
        {
            players.Add(newPlayer);
            //TODO: Change the index to work
            newPlayer.transform.position = newSettings.initialSpawn[0].transform.position;
            newPlayer.transform.rotation = newSettings.initialSpawn[0].transform.rotation;
        }
    }
}
