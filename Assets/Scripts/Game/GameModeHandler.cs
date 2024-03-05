using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameModeHandler : MonoBehaviour
{
    private class Team
    {
        public int team_kills = 0;
        public int team_deaths = 0;
        public int score = 0; 
    }

    public class GameRuleSetting
    {
        public int gameMode = 0;
        public float gameTime = 300.0f;
        public float respawnTimer = 3.0f;
        public float respawnThreshold = 100.0f;
        public int countdownStartTimer = 5;
    }

    public GameRuleSetting ruleSetting = new GameRuleSetting(); //Edit This on Menu
    int gameMode = 0;
    List<Team> teams;

    [SerializeField] List<GameObject> players;
    [SerializeField] List<ComponentRegistry> playerRegistries;
    [SerializeField] List<GameObject> spawnPoints;
    [SerializeField] List<GameObject> spawnFlag;
    public List<GameObject> podiumSpots;
    public Camera podiumCamera;
    [SerializeField] List<float> spawnPointDistances;
    [SerializeField] float currentGameTime;
    [SerializeField] int countdownStartTimer;
    [SerializeField] float respawnThreshold;
    [SerializeField] float respawnTimer;
    //[SerializeField] GameObject playerManager;
    // Start is called before the first frame update
    void Start()
    {
        teams = new List<Team>();
        players = new List<GameObject>();
        spawnPoints = new List<GameObject>();
        spawnPointDistances = new List<float>();
        spawnFlag = new List<GameObject>();

        foreach(var newSpawnPoint in GameObject.FindGameObjectsWithTag("SpawnPoint"))
        {
            spawnPoints.Add(newSpawnPoint);
            spawnPointDistances.Add(float.MaxValue);
        }

        foreach (var newFlagSpawn in GameObject.FindGameObjectsWithTag("FlagPoint"))
        {
            spawnPoints.Add(newFlagSpawn);
            spawnPointDistances.Add(float.MaxValue);
        }

        //playerManager = GameObject.FindGameObjectWithTag("PlayerManager");
        
        //LoadGameSettings();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < players.Count;i++)
        {
            var attributeComp = playerRegistries[i].attributeManager;
            if (!attributeComp.dead && attributeComp.currentHealth <= 0)
            {
                attributeComp.dead = true;
                attributeComp.currentHealth = 0;
                attributeComp.healthbar.value = 0;
                playerRegistries[i].animationManager.toggleDeadBool(true);
                playerRegistries[i].playerController.enabled = false;
                var deadScoreInfo = players[i].GetComponent<PlayerScoreInfo>();
                var killerScoreInfo = deadScoreInfo.lastDamagedBy.GetComponent<PlayerScoreInfo>();
                killerScoreInfo.kill_count++;
                teams[killerScoreInfo.team].team_kills++;
                deadScoreInfo.death_count++;
                teams[deadScoreInfo.team].team_deaths++;
                if (gameMode == 0)
                {
                    teams[killerScoreInfo.team].score++;
                }

                StartCoroutine(reincarnatePlayer(players[i], FindSpawnPoint()));
            }
        }

        currentGameTime -= Time.deltaTime;
        if(currentGameTime > 0)
        {
            //currentGameTime = 0;
            //TODO: Update UI Visual
        }
        else
        {
            currentGameTime = 0;
            List<int> ranking = new List<int>();
            Vector2Int currentHighest = new Vector2Int(0, int.MinValue);
            for(int i = 0; i< teams.Count; i++) 
            {
                for(int j = 0; j < teams.Count; j++) 
                {
                    if (!ranking.Contains(j) && teams[j].score > currentHighest.y)
                    {
                        currentHighest.x = j;
                        currentHighest.y = teams[j].score;
                    }
                }
                ranking.Add(currentHighest.x);
                currentHighest.x = 0;
                currentHighest.y = int.MinValue;
            }

            //Sort players according to team rankings
            //Example: playersSortedByRanking[0][0] is first player of first team
            //playersSortedByRanking[1][2] is third player of second team
            List<List<GameObject>> playersSortedByRanking = new List<List<GameObject>>();
            for(int i = 0; i < ranking.Count; i++)
            {
                playersSortedByRanking.Add(new List<GameObject>());
                foreach(var player in players) 
                {
                    if(player.GetComponent<PlayerScoreInfo>().team == ranking[i])
                    {
                        playersSortedByRanking[i].Add(player);
                    }
                }
            }

            //Disable players and push them to the podium spots
            foreach(var playerReg in playerRegistries)
            {
                playerReg.inputManager.enabled = false;
                playerReg.advancedProjectileSystem.enabled = false;
                playerReg.playerCamera.enabled = false;
                playerReg.playerController.enabled = false;
            }
            for(int i = 0; i<playersSortedByRanking.Count;i++)
            {
                for(int j = 0; j < playersSortedByRanking[i].Count;j++)
                {
                    var targetCompRegistry = playersSortedByRanking[i][j].GetComponent<ComponentRegistry>();
                    targetCompRegistry.rigidBody.MovePosition(podiumSpots[i].transform.position);
                    targetCompRegistry.rigidBody.MoveRotation(podiumSpots[i].transform.rotation);
                }
            }
            //Enable Podium Camera
            podiumCamera.enabled = true;

            //TODO: Add buttons to restart or go back to menu
        }
    }

    private IEnumerator reincarnatePlayer(GameObject player, GameObject respawnPoint)
    {
        yield return new WaitForSeconds(respawnTimer);
        player.transform.Find("AnimationController").GetComponent<AnimationManager>().toggleDeadBool(false);
        player.transform.SetPositionAndRotation(respawnPoint.transform.position, respawnPoint.transform.rotation);
        player.GetComponent<UpdatedPlayerController>().enabled = true;
      
        AttributeManager attributeComp = player.transform.GetComponent<AttributeManager>();
        attributeComp.currentHealth = attributeComp.maxHealth;
        attributeComp.dead = false;
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

    public void LoadGameSettings()
    {
        respawnThreshold = ruleSetting.respawnThreshold;
        respawnTimer = ruleSetting.respawnTimer;
        countdownStartTimer = ruleSetting.countdownStartTimer;
        gameMode = ruleSetting.gameMode;
        currentGameTime = ruleSetting.gameTime + countdownStartTimer;
        teams = new List<Team>();
        players = new List<GameObject>();
        playerRegistries = new List<ComponentRegistry>();
        foreach (var newPlayer in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(newPlayer);
            //TODO: Change the index to work
            var spawnPoint = FindSpawnPoint();
            newPlayer.transform.position = spawnPoint.transform.position;
            newPlayer.transform.rotation = spawnPoint.transform.rotation;
            var compRegistry = newPlayer.GetComponent<ComponentRegistry>();
            compRegistry.rigidBody.position = newPlayer.transform.position;
            compRegistry.playerCamera.enabled = true;
            playerRegistries.Add(compRegistry);
            while(teams.Count <= newPlayer.GetComponent<PlayerScoreInfo>().team) //FORBIDDEN WHILE LOOP, DONT USE WHILE
            {
                teams.Add(new Team());
            }
        }
        int i = 1;
        for(i = 1; (players.Count - 1) / i >= i; i++)
        {
            Debug.Log(i.ToString());
        }
        int camColumns = i;
        int camRows = i;
        if(i*(i-1) >= players.Count)
        {
            camRows = i - 1;
        }
        float camXSize = 1.0f / camColumns;
        float camYSize = 1.0f / camRows;
        for(int j = 0; j < players.Count;j++)
        {
            Debug.Log(camXSize.ToString());
            Debug.Log(camYSize.ToString());
            playerRegistries[j].playerCamera.rect = new Rect((float)(j%camColumns)*camXSize,(float)(j/camColumns)*camYSize,camXSize,camYSize);
        }
        StartCoroutine(StartCountdownTimer());
    }

    public IEnumerator StartCountdownTimer()
    {
        for(int i = 0; i < countdownStartTimer; i++)
        {
            yield return new WaitForSeconds(1);
            //TODO: Update UI visuals
        }
        CountDownEnd();
    }

    public void CountDownEnd()
    {
        foreach(var player in players)
        {
            var componentRegistry = player.GetComponent<ComponentRegistry>();
            componentRegistry.inputManager.enabled = true;
            componentRegistry.advancedProjectileSystem.enabled = true;
            componentRegistry.playerController.enabled = true;
        }
    }
}
