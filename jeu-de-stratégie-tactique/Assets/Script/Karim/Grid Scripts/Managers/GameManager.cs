using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TEAM2;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UnitManager _unitManager;
    [SerializeField] private BattleGrid _battleGrid;
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private GridBuildingSystem _gridBuildingSystem;
    [SerializeField] private BuildingManager _buildingManager;
    public GridBuildingSystem GridBuildingSystem
    {
        get => _gridBuildingSystem;
        set => _gridBuildingSystem = value;
    }

    public BuildingManager BuildingManager
    {
        get => _buildingManager;
        set => _buildingManager = value;
    }

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

    public Player P1
    {
        get => PlayerManager.PlayersGameObjects[0].GetComponent<Player>();
    }
    public Player P2
    {
        get => PlayerManager.PlayersGameObjects[1].GetComponent<Player>();
    }

    [MenuItem("GameObject/GameManager")]
    static void InstanceGameManager()
    {
        GameObject gameManager = new GameObject("GameManager", typeof(GameManager));
    }

    public void InstantiateEffect(Vector3 effectPos, int index)
    {
        effectManager.index = index;
        GameObject effect = Instantiate(effectManager.currentEffect);
        effect.transform.position = effectPos;
    }

    void Awake()
    {
        if (Instance != this)
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

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    void Reset()
    {
        _unitManager = _unitManager ?? GetComponent<UnitManager>() ?? gameObject.AddComponent<UnitManager>();
        _battleGrid = _battleGrid ?? GetComponent<BattleGrid>() ?? gameObject.AddComponent<BattleGrid>();
        _playerManager = _playerManager ?? GetComponent<PlayerManager>() ?? gameObject.AddComponent<PlayerManager>();
        _gridBuildingSystem = _gridBuildingSystem ?? GetComponent<GridBuildingSystem>() ?? gameObject.AddComponent<GridBuildingSystem>();
        _buildingManager = _buildingManager ?? GetComponent<BuildingManager>() ?? gameObject.AddComponent<BuildingManager>();
        effectManager.effects = Resources.LoadAll<GameObject>("Effect").ToList();
        gameObject.tag = "GameManager";
    }

    //When Game starting
    public void OnGameStart()
    {
        //First, spawn grid and Building
        _buildingManager.Init(this);
        _gridBuildingSystem.Init(this);
        _battleGrid.Init(this);
        //Second, Unit manager
        _unitManager.Init(this);
        //Then spawn each playersGameObjects characters randomly on grid
        _playerManager.Init(this);
        //after choose randomly a player to start (Online Stuff)

        StartCoroutine(_unitManager.GameLoop());
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
    public List<GameObject> effects;
    public int index = 0;

    public GameObject currentEffect
    {
        get
        {
            return effects[index];
        }
    }
}
