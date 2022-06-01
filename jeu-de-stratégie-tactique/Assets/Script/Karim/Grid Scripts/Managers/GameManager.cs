using System;
using System.Collections;
using System.Collections.Generic;
using TEAM2;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UnitManager _unitManager;
    [SerializeField] private BattleGrid _battleGrid;
    [SerializeField] private PlayerManager _playerManager;

    public UnitManager UnitManager
    {
        get => _unitManager;
    }
    public BattleGrid BattleGrid
    {
        get => _battleGrid;
    } 
    public PlayerManager PlayerManager
    {
        get => _playerManager;
    }

    public static GameManager Instance;
    public EffectManager effectManager;

    public GameState gameState;

    Player p1;
    Player p2;

    [MenuItem("GameObject/GameManager")]
    static void InstanceGameManager()
    {
        GameObject gameManager = new GameObject("GameManager", typeof(GameManager));
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


    void Reset()
    {
        _unitManager = _unitManager ?? GetComponent<UnitManager>() ?? gameObject.AddComponent<UnitManager>();
        _battleGrid = _battleGrid ?? GetComponent<BattleGrid>() ?? gameObject.AddComponent<BattleGrid>();
        _playerManager = _playerManager ?? GetComponent<PlayerManager>() ?? gameObject.AddComponent<PlayerManager>();
        gameObject.tag = "GameManager";
    }

    //When Game starting
    public void OnGameStart()
    {
        _unitManager.Init(this);
        //First, spawn grid
        _battleGrid.Init(this);
        //Then spawn each players characters randomly on grid
        _playerManager.Init(this);
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
