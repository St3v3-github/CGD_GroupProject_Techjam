using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    FreeForAll,
    TeamDeathMatch
}
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameRules")]
public class GameRules : ScriptableObject
{
    [SerializeField] public GameMode gameMode;
    [SerializeField] public float timer;
    [SerializeField] public float respawnTimer;
    public int playerCount;
    [SerializeField] public int spawnPointVariance;
    public List<int> teamScore;
    public List<GameObject> players;
    public List<GameObject> team1;
    public List<GameObject> team2;
    public List<GameObject> spawnPoints;


}
