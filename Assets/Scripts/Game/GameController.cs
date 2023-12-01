using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    // My name is Ozymandias, King of Kings; Look on my Works, ye Mighty, and despair!
    public GameRules game;
    private bool lobby = true;
    private float lobbyTimer = 30;
    public TextMeshProUGUI lobbyText;
    public LayerMask playerLayer;

    public float timer;
    private List<int> teamScore;
    public int playerCount;
    public List<GameObject> players;
    public List<GameObject> team1;
    public List<GameObject> team2;
    public List<GameObject> spawnPoints;


    // Start is called before the first frame update
    void Start()
    {
        timer = game.timer;
    }


    // Update is called once per frame
    void Update()
    {
        if (lobby)
        {
            lobbyTimer -= Time.deltaTime;

            // Update the UI text with the current countdown value
            lobbyText.text = Mathf.Round(lobbyTimer).ToString();

            // Check if the countdown has reached zero
            if (lobbyTimer <= 0)
            {
                lobbyText.text = "Game Starting";
                StartGame();
                Invoke("ClearText", 1f);

            }
        }
        else if (!lobby)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                switch (game.gameMode)
                {
                    case GameMode.FreeForAll:
                        AnnounceFFAWinner();


                        break;
                    case GameMode.TeamDeathMatch:
                        AnnounceTDMWinner();
                        break;
                }
            }
        }
        
    }

    public void StartGame()
    {
        lobby = false;
        // Creates list of players
        //Collider[] colliders = Physics.OverlapSphere(transform.position, Mathf.Infinity, playerLayer);
        GameObject[] colliders = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject collider in colliders)
        {
            if (collider.GetComponent<PlayerController>() != null)
            {
                players.Add(collider);
                playerCount++;
            }
        }

        // Creates list of spawnpoints in Map
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("SpawnPoint");

        foreach (GameObject objectWithTag in objectsWithTag)
        {
            spawnPoints.Add(objectWithTag);
        }

        Debug.Log("teleporting");
        foreach (GameObject player in players)
        {
            
            int rand = Random.Range(0, spawnPoints.Count);

            if (spawnPoints[rand].GetComponent<SpawnPoint>().used == false)
            {
                Debug.Log("teleporting player to " + spawnPoints[rand].transform.position);
                player.GetComponent<CharacterController>().enabled = false;
                player.transform.SetPositionAndRotation(spawnPoints[rand].transform.position, spawnPoints[rand].transform.rotation);
                player.GetComponent<CharacterController>().enabled = true;
                Debug.Log(player.name+ " is at" + player.transform.position);
                spawnPoints[rand].GetComponent<SpawnPoint>().used = true;
            }
        }

        // Game mode specific setup
        switch (game.gameMode)
        {
            case GameMode.FreeForAll:
                foreach (GameObject player in players)
                {
                    teamScore.Add(0);
                }
                break;
            case GameMode.TeamDeathMatch:
                StartTeams();
                teamScore.Add(0);
                teamScore.Add(0);
                break;
        }

    }

    public void StartTeams()
    {
        
        int count = 0;
        // Add the found GameObjects to the list
        foreach (GameObject player in players)
        {
            count++;
            
            if (count >= (playerCount / 2))
            {
                team1.Add(player.gameObject);
            }
            else
            {
                team2.Add(player.gameObject);
            }
            
        }

    }

    public void TeleportPlayers()
    {
        Debug.Log("teleporting");
        foreach (GameObject player in players)
        {
            Debug.Log("teleporting player");
            int rand = Random.Range(0, spawnPoints.Count);

            if (spawnPoints[rand].GetComponent<SpawnPoint>().used == false)
            {
                player.transform.position = spawnPoints[rand].transform.position;
                spawnPoints[rand].GetComponent<SpawnPoint>().used = true;
            }
        }
    }

    public void ClearText() {lobbyText.text = "";}

    //Respawning Code Below

    public void PrayToGod(killing_data prayer)
    {
        switch (game.gameMode)
        {
            case GameMode.FreeForAll:
                UpdateFFAScore(prayer.killer);
                break;
            case GameMode.TeamDeathMatch:
                UpdateTDMScore(prayer.killer);
                break;
        }

        StartCoroutine(reincarnatePlayer(prayer.deaded, FindSpawnPoint(prayer.deaded)));
    }

    public GameObject FindSpawnPoint(GameObject deadPlayer)
    {
        List<GameObject> possibleSpawnPoints = new();

        // Fills list with 0s
        for (int i = 0; i < game.spawnPointVariance; i++)
        {
            possibleSpawnPoints.Add(deadPlayer);
        }
        
        foreach (GameObject spawnPoint in spawnPoints)
        {
            float newDistanceToSpawnPoint = Vector3.Distance(spawnPoint.transform.position, deadPlayer.transform.position);

            for (int j = 0; j < possibleSpawnPoints.Count; j++)
            {
                if (newDistanceToSpawnPoint > Vector3.Distance(possibleSpawnPoints[j].transform.position, deadPlayer.transform.position))
                {
                    possibleSpawnPoints[j] = spawnPoint;
                }
            }
        }
        int randomNumber = Random.Range(0, game.spawnPointVariance);

        return possibleSpawnPoints[randomNumber];
    }

    private IEnumerator reincarnatePlayer(GameObject player, GameObject respawnPoint)
    {
        yield return new WaitForSeconds(game.respawnTimer);
        player.transform.position = respawnPoint.transform.position;
        player.transform.rotation = respawnPoint.transform.rotation;
    }

    // FFA Score below
    public void UpdateFFAScore(GameObject killer)
    {
        int count = 0;
        foreach (GameObject player in players)
        {
            if (player == killer)
            {
                teamScore[count] += 1;
            }
            count++;
        }
    }

    public void AnnounceFFAWinner()
    {
        int count = 0;
        int WinningScore = 0;
        GameObject Winner = null;
        foreach (GameObject player in players)
        {
            if (count == 0)
            {
                WinningScore = teamScore[0];
                Winner = player;
            }
            else
            {
                if (teamScore[count] > WinningScore)
                {
                    WinningScore = teamScore[count];
                    Winner = player;
                }
            }
            count++;
        }
        //Debug.Log(Winner.tag + "WINS with " + WinningScore + "kills");
    }

    // Update TDM below
    public void UpdateTDMScore(GameObject killer)
    {
        foreach (GameObject player in team1)
        {
            if (player == killer)
            {
                teamScore[0] += 1;
            }
        }
        foreach (GameObject player in team2)
        {
            if (player == killer)
            {
                teamScore[1] += 1;
            }
        }
    }

    public void AnnounceTDMWinner()
    {
        if (teamScore[0] == teamScore[1])
        {
            Debug.Log("Time Up. DRAW");
        }
        else if (teamScore[0] > teamScore[1])
        {
            Debug.Log("Time Up. Team 1 WINS");
        }
        else if (teamScore[0] < teamScore[1])
        {
            Debug.Log("Time Up. Team 2 WINS");
        }
    }


}
