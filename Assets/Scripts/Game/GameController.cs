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
    // Start is called before the first frame update
    void Start()
    {

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
            game.timer -= Time.deltaTime;

            if (game.timer <= 0)
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
                game.players.Add(collider);
                game.playerCount++;
            }
        }

        // Creates list of spawnpoints in Map
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("SpawnPoint");

        foreach (GameObject objectWithTag in objectsWithTag)
        {
            game.spawnPoints.Add(objectWithTag);
        }

        // Game mode specific setup
        switch (game.gameMode)
        {
            case GameMode.FreeForAll:
                foreach (GameObject player in game.players)
                {
                    game.teamScore.Add(0);
                }
                break;
            case GameMode.TeamDeathMatch:
                StartTeams();
                game.teamScore.Add(0);
                game.teamScore.Add(0);
                break;
        }
    }

    public void StartTeams()
    {
        
        int count = 0;
        // Add the found GameObjects to the list
        foreach (GameObject player in game.players)
        {
            count++;
            
            if (count >= (game.playerCount / 2))
            {
                game.team1.Add(player.gameObject);
            }
            else
            {
                game.team2.Add(player.gameObject);
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
        
        foreach (GameObject spawnPoint in game.spawnPoints)
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
        foreach (GameObject player in game.players)
        {
            if (player == killer)
            {
                game.teamScore[count] += 1;
            }
            count++;
        }
    }

    public void AnnounceFFAWinner()
    {
        int count = 0;
        int WinningScore = 0;
        GameObject Winner = null;
        foreach (GameObject player in game.players)
        {
            if (count == 0)
            {
                WinningScore = game.teamScore[0];
                Winner = player;
            }
            else
            {
                if (game.teamScore[count] > WinningScore)
                {
                    WinningScore = game.teamScore[count];
                    Winner = player;
                }
            }
            count++;
        }
        Debug.Log(Winner.tag + "WINS with " + WinningScore + "kills");
    }

    // Update TDM below
    public void UpdateTDMScore(GameObject killer)
    {
        foreach (GameObject player in game.team1)
        {
            if (player == killer)
            {
                game.teamScore[0] += 1;
            }
        }
        foreach (GameObject player in game.team2)
        {
            if (player == killer)
            {
                game.teamScore[1] += 1;
            }
        }
    }

    public void AnnounceTDMWinner()
    {
        if (game.teamScore[0] == game.teamScore[1])
        {
            Debug.Log("Time Up. DRAW");
        }
        else if (game.teamScore[0] > game.teamScore[1])
        {
            Debug.Log("Time Up. Team 1 WINS");
        }
        else if (game.teamScore[0] < game.teamScore[1])
        {
            Debug.Log("Time Up. Team 2 WINS");
        }
    }


}
