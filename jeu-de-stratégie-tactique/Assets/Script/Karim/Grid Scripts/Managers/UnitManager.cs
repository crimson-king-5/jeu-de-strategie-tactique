using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TEAM2;
using UnityEngine;

public class UnitManager : MonoBehaviour
{

    [SerializeField] private GameManager _gameManager;

    public BattleGrid BattleGrid
    {
        get => _gameManager.BattleGrid;
    }

    public PlayerManager PlayerManager
    {
        get => _gameManager.PlayerManager;
    }

    [SerializeField] private List<ScriptableUnit> _units;

    public Unit SelectedHero;

    [SerializeField] private List<ScriptableUnit> heroesUnits = new List<ScriptableUnit>();
    [SerializeField] private List<ScriptableUnit> enemyUnits = new List<ScriptableUnit>();
    [SerializeField] private List<ScriptableUnit> buildingUnits = new List<ScriptableUnit>();


    private void ClearUnits()
    {
        _units?.Clear();
        heroesUnits?.Clear();
        buildingUnits?.Clear();
        enemyUnits?.Clear();
    }

    private void Reset()
    {
        LoadUnits();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectUnit();
        }
    }

    private Character GetRandomUnitPerFaction(Faction faction)
    {
        List<ScriptableUnit> FactionUnit = GetFactionScriptableUnits(faction);
        int randomIndex = Random.Range(0, FactionUnit.Count);
        GameObject unitObj = new GameObject("", typeof(Character), typeof(SpriteRenderer));
        Character newUnit = unitObj.GetComponent<Character>();
        SpriteRenderer unitRenderer = unitObj.GetComponent<SpriteRenderer>();
        newUnit.ScrUnit = FactionUnit[randomIndex];
        unitRenderer.sprite = newUnit.ScrUnit.renderUnit;
        unitRenderer.sortingOrder = 1;
        unitObj.name = newUnit.ScrUnit.unitsName;
        return newUnit;
    }

    private bool UpdateAndCheckifTeamisDead(Faction currentFaction)
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
            Debug.Log("Partie termin� ! Faction " + currentFaction.ToString() + " battue !");
        }
        return isDead;
    }

    private List<Character> GetFactionUnits(Faction CurrentFaction)
    {
        //TODO
        List<Character> characters = new List<Character>();
        Character localCharacter;
        switch (CurrentFaction)
        {
            case Faction.Hero:
                for (int i = 0; i < _gameManager.P1.GetUnitWithType(UnitType.Character).Count; i++)
                {
                    localCharacter = (Character)_gameManager.P1.GetUnitWithType(UnitType.Character)[i];
                    characters.Add(localCharacter);
                }
                break;
            case Faction.Enemy:
                for (int i = 0; i < _gameManager.P2.GetUnitWithType(UnitType.Character).Count; i++)
                {

                    localCharacter = (Character)_gameManager.P2.GetUnitWithType(UnitType.Character)[i];
                    characters.Add(localCharacter);
                }
                break;
        }
        return characters;
    }

    private void SelectUnit()
    {
        Vector3 mousPos = BattleGrid.GetMouseWorldPosition();
        if (PlayerManager.CheckifUnitWasHere(BattleGrid.Tilemap.WorldToCell(mousPos)))
        {
            Unit selectedCharacter = _gameManager.PlayerManager.GetUnit(BattleGrid.Tilemap.WorldToCell(mousPos));
            if (selectedCharacter != null &&
                selectedCharacter.ScrUnit.faction == PlayerManager.CurrentPlayer.PlayerFaction &&
                selectedCharacter.unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn)
            {
                if (SelectedHero.GetComponent<SpriteRenderer>().color == Color.blue)
                {
                    SelectedHero.GetComponent<SpriteRenderer>().color = Color.white;
                }

                SelectedHero = selectedCharacter;
                SelectedHero.GetComponent<SpriteRenderer>().color = Color.blue;
                Debug.Log("Tours de: " + SelectedHero.ScrUnit.unitsName);
            }
        }
    }

    public void Init(GameManager gm)
    {
        _gameManager = gm;
    }

    [Button("LoadUnits", ButtonSizes.Large)]
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
            else if (_units[i].faction == Faction.Enemy)
            {
                enemyUnits.Add(_units[i]);
            }
            else
            {
                buildingUnits.Add(_units[i]);
            }

        }
    }

    public Character[] SpawnCharacter(int unitCount, Faction currentfaction)
    {
        Character[] chars = new Character[unitCount];

        for (int i = 0; i < unitCount; i++)
        {
            Character randomPrefab = GetRandomUnitPerFaction(currentfaction);
            Vector3 randomSpawnBattleGridTile = _gameManager.BattleGrid.SpawnUnitPerFaction(currentfaction);
            _gameManager.PlayerManager.SetUnit(randomPrefab, randomSpawnBattleGridTile);
            chars[i] = randomPrefab;
        }
        return chars;
    }

    public Unit GetSpecificUnitPerIndex(int index, Faction UnitFaction)
    {
        List<ScriptableUnit> FactionUnit = GetFactionScriptableUnits(UnitFaction);
        GameObject unitObj = new GameObject("", typeof(Building), typeof(SpriteRenderer));
        Building newUnit = unitObj.GetComponent<Building>();
        SpriteRenderer unitRenderer = unitObj.GetComponent<SpriteRenderer>();
        newUnit.ScrUnit = FactionUnit[index];
        unitRenderer.sprite = newUnit.ScrUnit.renderUnit;
        unitRenderer.sortingOrder = 1;
        unitObj.name = newUnit.ScrUnit.unitsName;
        return newUnit;
    }


    public Unit GetSpecificUnitPerName(string unitName, Faction UnitFaction)
    {
        List<ScriptableUnit> FactionUnit = GetFactionScriptableUnits(UnitFaction);
        for (int i = 0; i < FactionUnit.Count; i++)
        {
            if (FactionUnit[i].unitsName == unitName)
            {
                GameObject unitObj = new GameObject(unitName, typeof(Unit), typeof(SpriteRenderer));
                Unit newUnit = unitObj.GetComponent<Unit>();
                SpriteRenderer unitRenderer = unitObj.GetComponent<SpriteRenderer>();
                newUnit.ScrUnit = FactionUnit[i];
                unitRenderer.sprite = newUnit.ScrUnit.renderUnit;
                unitRenderer.sortingOrder = 1;
                unitObj.name = newUnit.ScrUnit.unitsName;

                return newUnit;
            }
        }
        Debug.LogError("Unit was not found !");
        return null;
    }

    public Character GetSpecificCharacterPerName(string unitName, Faction UnitFaction)
    {
        List<ScriptableUnit> FactionUnit = GetFactionScriptableUnits(UnitFaction);
        for (int i = 0; i < FactionUnit.Count; i++)
        {
            if (FactionUnit[i].unitsName == unitName)
            {
                GameObject unitObj = new GameObject(unitName, typeof(Character), typeof(SpriteRenderer));
                Character newUnit = unitObj.GetComponent<Character>();
                SpriteRenderer unitRenderer = unitObj.GetComponent<SpriteRenderer>();
                newUnit.ScrUnit = FactionUnit[i];
                unitRenderer.sprite = newUnit.ScrUnit.renderUnit;
                unitRenderer.sortingOrder = 1;
                unitObj.name = newUnit.ScrUnit.unitsName;

                return newUnit;
            }
        }
        Debug.LogError("Unit was not found !");
        return null;
    }

    public Building GetSpecificBuildingPerName(string unitName, Faction UnitFaction)
    {
        List<ScriptableUnit> FactionUnit = GetFactionScriptableUnits(UnitFaction);
        for (int i = 0; i < FactionUnit.Count; i++)
        {
            if (FactionUnit[i].unitsName == unitName)
            {
                GameObject unitObj = new GameObject(unitName, typeof(Building), typeof(SpriteRenderer));
                Building newUnit = unitObj.GetComponent<Building>();
                SpriteRenderer unitRenderer = unitObj.GetComponent<SpriteRenderer>();
                newUnit.ScrUnit = FactionUnit[i];
                unitRenderer.sprite = newUnit.ScrUnit.renderUnit;
                unitRenderer.sortingOrder = 1;
                unitObj.name = newUnit.ScrUnit.unitsName;
                return newUnit;
            }
        }
        Debug.LogError("Unit was not found !");
        return null;
    }

    public List<ScriptableUnit> GetFactionScriptableUnits(Faction currentFaction)
    {
        switch (currentFaction)
        {
            case Faction.Hero:
                return heroesUnits;
            case Faction.Enemy:
                return enemyUnits;
            case Faction.Building:
                return buildingUnits;
        }

        return null;
    }

    public void UpdateUnitsRenderAndSate(List<Character> units)
    {
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].unitStateMachine.currentState != UnitStateMachine.UnitState.Dead)
            {
                SpriteRenderer uniRenderer = units[i].GetComponent<SpriteRenderer>();
                uniRenderer.color = Color.white;
                units[i].unitStateMachine.currentState = UnitStateMachine.UnitState.None;
            }
        }
    }

    public IEnumerator GameLoop()
    {
        #region TMP

        Debug.Log("Debut de Game");
        List<Character> allDeployedHeroesUnits = GetFactionUnits(Faction.Hero);
        List<Character> allDeployedEnemiesUnits = GetFactionUnits(Faction.Enemy);
        bool gameOver = false;
        while (!gameOver)
        {
            PlayerManager.index = 0;
            PlayerManager.CurrentPlayer.AddResource();
            UpdateUnitsRenderAndSate(allDeployedHeroesUnits);
            SelectedHero = allDeployedHeroesUnits[0];
            SelectedHero.GetComponent<SpriteRenderer>().color = Color.blue;
            Debug.Log("Tours de : " + SelectedHero.ScrUnit.unitsName);

            yield return new WaitUntil(() => PlayerManager.CurrentPlayer.CheckifAllUnitsHasEndTurn());

            gameOver = UpdateAndCheckifTeamisDead(Faction.Hero);

            PlayerManager.index++;
            PlayerManager.CurrentPlayer.AddResource();
            SelectedHero = allDeployedEnemiesUnits[0];
            SelectedHero.GetComponent<SpriteRenderer>().color = Color.blue;
            Debug.Log("Tour de : " + SelectedHero.ScrUnit.unitsName);

            yield return new WaitUntil(() => PlayerManager.CurrentPlayer.CheckifAllUnitsHasEndTurn());

            gameOver = UpdateAndCheckifTeamisDead(Faction.Enemy);
            UpdateUnitsRenderAndSate(allDeployedEnemiesUnits);
        }

        yield return null;
        #endregion
    }

}
