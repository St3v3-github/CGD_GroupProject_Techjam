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
    [SerializeField] public int spawnPointVariance;



}
