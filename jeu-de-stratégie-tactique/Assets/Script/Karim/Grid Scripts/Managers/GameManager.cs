using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public EffectManager effectManager;

    public GameState gameState;

    [MenuItem("GameObject/GameManager")]
    static void InstanceGameManager()
    {
        GameObject gameManager = new GameObject("GameManager", typeof(GameManager), typeof(UnitManager));
    }

    public void InstantiateEffect(Vector3 effectPos, int index)
    {
        effectManager.index = index;
        GameObject effect = Instantiate(effectManager.currentEffect.gameObjectEffect);
        effect.transform.position = effectPos;
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
            case GameState.LaunchGameLoop:
                StartCoroutine(UnitManager.Instance.GameLoop());
                break;
        }
    }

    public void ChangeState(int newState)
    {
        gameState = (GameState)newState;
        switch ((GameState)newState)
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
            case GameState.LaunchGameLoop:
                StartCoroutine(UnitManager.Instance.GameLoop());
                break;
        }
    }


    public enum GameState
    {
        SpawnHeroes = 0,
        SpawnEnemies = 1,
        HerosTurn = 2,
        EnemiesTurn = 3,
        LaunchGameLoop = 4,
    }
}

[System.Serializable]
public class EffectManager
{
    public List<Effect> effects;
    public int index = 0;

    public Effect currentEffect
    {
        get
        {
            return effects[index];
        }
    }
}

[System.Serializable]
public class Effect
{
    public GameObject gameObjectEffect;
}
