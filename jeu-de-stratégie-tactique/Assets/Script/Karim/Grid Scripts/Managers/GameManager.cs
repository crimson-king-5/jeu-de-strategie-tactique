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

    TEAM2.Player p1;
    TEAM2.Player p2;

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

    private void Start()
    {
        OnGameStart();//TODO: Move func elsewhere
    }

    //When Game starting
    public void OnGameStart()
    {
        //First, spawn grid
        BattleGrid.instance.Init();
        //Then spawn each players characters randomly on grid
        TEAM2.PlayerManager.instance.InitPlayers();
        //thirdly spawn pre-placed buildings (with some effects)

        //after choose randomly a player to start (Online Stuff)

        //Finally begin choose action part
        ChangeState(GameState.CHOOSEACTION);
    }

    public void ChangeState(GameState newState)
    {
        gameState = newState;
    }

    public void ChangeState(int newState)
    {
        gameState = (GameState)newState;
    }


    public enum GameState
    {
        CHOOSEACTION = 0,
        RESOLUTIONPHASE = 1
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
