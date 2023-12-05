using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
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
    public int playerCount;
    public List<GameObject> players;
    public List<GameObject> team1;
    public List<GameObject> team2;
    public int team1Score;
    public int team2Score;
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
            lobbyText.text = "Game time" + timer.ToString();

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

        foreach (GameObject player in players)
        {
            
            int rand = Random.Range(0, spawnPoints.Count);

            while (spawnPoints[rand].GetComponent<SpawnPoint>().used == true)
            {
                rand = Random.Range(0, spawnPoints.Count);
            }

            if (spawnPoints[rand].GetComponent<SpawnPoint>().used == false)
            {
                Debug.Log("teleporting player to " + spawnPoints[rand].transform.position);
                player.GetComponent<CharacterController>().enabled = false;
                player.transform.SetPositionAndRotation(spawnPoints[rand].transform.position, spawnPoints[rand].transform.rotation);
                player.GetComponent<CharacterController>().enabled = true;
                spawnPoints[rand].GetComponent<SpawnPoint>().used = true;
            }
        }
        Debug.Log("Setting tags");
        switch (game.gameMode)
        {
            case GameMode.FreeForAll:
                int i = 1;
                foreach (GameObject player in players)
                {

                    string tagString = "Player" + i.ToString();
                    Debug.Log(tagString);
                    player.tag = tagString;
                    player.GetComponent<AbilityManager2>().spell_controller.gameObject.tag = tagString;
                    player.GetComponent<UIController>().attributeController.gameObject.tag = tagString;
                    i++;
                }
                break;
            case GameMode.TeamDeathMatch:
                foreach (GameObject player in team1)
                {
                    player.tag = "Player1";
                    
                }
                foreach (GameObject player in team2)
                {
                    player.tag = "Player2";
                   
                }
                break;
        }

        // Game mode specific setup
        /*switch (game.gameMode)
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
        }*/

        

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

    public void tagSetter()
    {
        switch (game.gameMode)
        {
            case GameMode.FreeForAll:
                int i = 1;
                foreach (GameObject player in players)
                {

                    string tagString = "Player" + i.ToString();
                    Debug.Log(tagString);
                    player.tag = tagString; i++;
                }
                break;
            case GameMode.TeamDeathMatch:
                foreach (GameObject player in team1)
                {
                    player.tag = "Player1";
                }
                foreach (GameObject player in team2)
                {
                    player.tag = "Player2";
                }
                break;
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
                player.transform.rotation = spawnPoints[rand].transform.rotation;
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

        prayer.deaded.transform.Find("AnimationController").GetComponent<AnimationManager>().toggleDeadBool(true);
        prayer.deaded.transform.Find("AttributeController").GetComponent<AttributeManager>().currentHealth = 0;
        prayer.deaded.transform.Find("AttributeController").GetComponent<AttributeManager>().healthbar.value = 0;
        prayer.deaded.transform.Find("AttributeController").gameObject.SetActive(false);
        prayer.deaded.transform.Find("AnimationController").GetComponent<AnimationManager>().toggleDeadBool(false);
        prayer.deaded.transform.Find("Mesh").gameObject.SetActive(false);
        prayer.deaded.GetComponent<CharacterController>().enabled = false;


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
        Debug.Log("Random number is" +  randomNumber);
        return possibleSpawnPoints[randomNumber];
    }

    private IEnumerator reincarnatePlayer(GameObject player, GameObject respawnPoint)
    {
        yield return new WaitForSeconds(game.respawnTimer);
        player.transform.Find("AnimationController").GetComponent<AnimationManager>().toggleDeadBool(false);
        player.transform.Find("AttributeController").gameObject.SetActive(true);
        player.transform.Find("Mesh").gameObject.SetActive(true);
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.SetPositionAndRotation(respawnPoint.transform.position, respawnPoint.transform.rotation);
        player.GetComponent<CharacterController>().enabled = true;
        player.transform.Find("AttributeController").GetComponent<AttributeManager>().currentHealth = player.transform.GetChild(5).GetComponent<AttributeManager>().maxHealth;
    }

    // FFA Score below
    public void UpdateFFAScore(GameObject killer)
    {
        if (killer.GetComponent<AttributeManager>() != null) { killer.GetComponent<AttributeManager>().score += 1; }
        else { killer.transform.Find("AttributeController").GetComponent<AttributeManager>().score += 1; }
        
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
                WinningScore = player.transform.Find("AttributeController").GetComponent<AttributeManager>().score;
                Winner = player;
            }
            else
            {
                if (player.transform.Find("AttributeController").GetComponent<AttributeManager>().score > WinningScore)
                {
                    WinningScore = player.transform.Find("AttributeController").GetComponent<AttributeManager>().score;
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
                team1Score += 1;
            }
        }
        foreach (GameObject player in team2)
        {
            if (player == killer)
            {
                team2Score += 1;
            }
        }
    }

    public void AnnounceTDMWinner()
    {
        if (team1Score == team2Score)
        {
            Debug.Log("Time Up. DRAW");
        }
        else if (team1Score > team2Score)
        {
            Debug.Log("Time Up. Team 1 WINS");
        }
        else if (team1Score < team2Score)
        {
            Debug.Log("Time Up. Team 2 WINS");
        }
    }


}
