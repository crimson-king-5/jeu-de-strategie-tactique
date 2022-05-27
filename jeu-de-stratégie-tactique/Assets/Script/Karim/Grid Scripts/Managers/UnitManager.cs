using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private static UnitManager _instance;

    [SerializeField] private GameManager _gameManager;

    public static UnitManager Instance
    {
        get => _instance;
    }

    [SerializeField] private List<ScriptableUnit> _units;

    public Character SelectedHero;

  [SerializeField] private List<ScriptableUnit> heroesUnits = new List<ScriptableUnit>();
  [SerializeField] private List<ScriptableUnit> enemyUnits = new List<ScriptableUnit>();
  [SerializeField] private List<ScriptableUnit> neutralUnits = new List<ScriptableUnit>();

    [Button("LoadUnits",ButtonSizes.Large)]
    public void LoadUnits()
    {
        ClearUnits();
        _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
        for (int i = 0; i < _units.Count; i++)
        {
            if (_units[i].faction == Faction.Hero)
            {
                heroesUnits.Add(_units[i]);
            }
            else
            {
                enemyUnits.Add(_units[i]);
            }
        }
    }

    private void ClearUnits()
    { 
        _units?.Clear();
        heroesUnits?.Clear();
        neutralUnits?.Clear();
        enemyUnits?.Clear();
    }

    void Reset()
    {
        LoadUnits();
    }

    void Awake()
    {
        _instance = this;
    }

    public void Init(GameManager gm)
    {
        _gameManager = gm;
    }

    public Character[] SpawnCharacter(int unitCount,Faction currentfaction)
    {
        Character[] chars = new Character[unitCount];

        for (int i = 0; i < unitCount; i++)
        {
            Character randomPrefab = GetRandomUnitPerFaction(currentfaction);
            Tile randomSpawnTile = _gameManager.BattleGrid.SpawnRandomUnit();
            randomPrefab.xPos = randomSpawnTile.tileXPos;
            randomPrefab.yPos = randomSpawnTile.tileYPos;
            randomSpawnTile.SetUnit(randomPrefab);
            chars[i] = randomPrefab;
        }
        return chars;
    }

    private Character GetRandomUnitPerFaction(Faction faction)
    {
        List<ScriptableUnit> FactionUnit = GetFactionScriptableUnits(faction);
        int randomIndex = Random.Range(0, FactionUnit.Count);
        GameObject unitObj = new GameObject("", typeof(Character), typeof(SpriteRenderer));
        Character newUnit = unitObj.GetComponent<Character>();
        SpriteRenderer unitRenderer = unitObj.GetComponent<SpriteRenderer>();
        newUnit.scriptableUnit = FactionUnit[randomIndex];
        unitRenderer.sprite = newUnit.scriptableUnit.renderUnit;
        unitRenderer.sortingOrder = 1;
        unitObj.name = newUnit.scriptableUnit.unitsName;
        return newUnit;
    }

    public List<ScriptableUnit> GetFactionScriptableUnits(Faction currentFaction)
    {
        switch (currentFaction)
        {
            case Faction.Hero:
                return heroesUnits;
            case Faction.Enemy:
                return enemyUnits;
        }

        return null;
    }


    public void ResetUnitsTurns(List<Character> units)
    {
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].unitStateMachine.currentState != UnitStateMachine.UnitState.Dead)
            {
                units[i].unitStateMachine.currentState = UnitStateMachine.UnitState.None;
            }
        }
    }

    public IEnumerator GameLoop()
    {
        #region TMP
        /*
        Debug.Log("Debut de Game");
        bool gameOver = false;
        while (!gameOver)
        {
            ResetUnitsTurns(allDeployedHeroesUnits);

            for (int i = 0; i < allDeployedHeroesUnits.Count; i++)
            {
                SelectedHero = allDeployedHeroesUnits[i];
                Debug.Log(SelectedHero.scriptableUnit.unitsName);
                if (SelectedHero.scriptableUnit.unitStats.life <= 0 && SelectedHero.unitStateMachine.currentState != UnitStateMachine.UnitState.Dead)
                {
                    SelectedHero.OccupiedTile.OccupiedUnit = null;
                    SelectedHero.OccupiedTile = null;
                    SelectedHero.xPos = 999;
                    SelectedHero.yPos = 999;
                    SelectedHero.unitStateMachine.currentState = UnitStateMachine.UnitState.Dead;
                    SelectedHero.gameObject.SetActive(false);
                }
                else
                {
                    yield return new WaitUntil(() => SelectedHero.unitStateMachine.currentState == UnitStateMachine.UnitState.EndTurn);
                }
            }
            gameOver = CheckifTeamisDead(Faction.Hero);

            for (int i = 0; i < allDeployedEnemiesUnits.Count; i++)
            {
                SelectedHero = allDeployedEnemiesUnits[i];
                Debug.Log(SelectedHero.scriptableUnit.unitsName);
                if (SelectedHero.scriptableUnit.unitStats.life <= 0 && SelectedHero.unitStateMachine.currentState != UnitStateMachine.UnitState.Dead)
                {
                    Debug.Log(SelectedHero.scriptableUnit.unitsName + " est mort !");
                    SelectedHero.OccupiedTile.OccupiedUnit = null;
                    SelectedHero.OccupiedTile = null;
                    SelectedHero.xPos = 999;
                    SelectedHero.yPos = 999;
                    SelectedHero.unitStateMachine.currentState = UnitStateMachine.UnitState.Dead;
                    SelectedHero.gameObject.SetActive(false);
                }
                else
                {
                    yield return new WaitUntil(() => SelectedHero.unitStateMachine.currentState == UnitStateMachine.UnitState.EndTurn);
                }
            }

            gameOver = CheckifTeamisDead(Faction.Enemy);
            ResetUnitsTurns(allDeployedEnemiesUnits);
        }
        */
        yield return null;
        #endregion
    }

    private bool CheckifTeamisDead(Faction currentFaction)
    {
        bool isDead = true;
        List<Character> factionUnits = GetFactionUnits(currentFaction);
        for (int i = 0; i < factionUnits.Count; i++)
        {
            if (factionUnits[i].gameObject.activeSelf)
            {
                isDead = false;
                break;
            }
        }

        if (isDead)
        {
            Debug.Log("Partie terminé ! Faction " + currentFaction.ToString() + " battue !");
        }
        return isDead;
    }

    private List<Character> GetFactionUnits(Faction CurrentFaction)
    {
        //TODO
        return null;
    }

    //public void SetSelectedHero(BaseUnit hero)
    //{
    //    if (hero.unitStateMachine.currentState == UnitStateMachine.UnitState.EndTurn)
    //    {
    //        SelectedHero = hero;
    //        MenuManager.Instance.ShowSelectedHero(hero);
    //    }
    //}
}
