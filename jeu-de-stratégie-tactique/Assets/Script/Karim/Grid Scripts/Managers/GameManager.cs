using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState gameState;

    [MenuItem("GameObject/GameManager")]
    static void InstanceGameManager()
    {
        GameObject gameManager = new GameObject("GameManager", typeof(GameManager),typeof(UnitManager));
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }

    public void ChangeState(GameState newState)
    {
        gameState = newState;
        switch (newState)
        {
            case GameState.SpawnHeroes:
                UnitManager.Instance.SpawnHeroes(1);
                break;
            case GameState.SpawnEnemies:
                UnitManager.Instance.SpawnEnemies();
                break;
            case GameState.HerosTurn:
                break;
            case GameState.EnemiesTurn:
                break;
        }
    }


    public enum GameState
    {
        SpawnHeroes = 0,
        SpawnEnemies = 1,
        HerosTurn = 2,
        EnemiesTurn = 3,
    }
}
