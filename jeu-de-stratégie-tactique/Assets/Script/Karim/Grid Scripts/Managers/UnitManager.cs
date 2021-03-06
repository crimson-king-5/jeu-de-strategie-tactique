using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TEAM2;
using UnityEngine;

public class UnitManager : MonoBehaviour
{

    [SerializeField] private GameManager _gameManager;
    [SerializeField] private AudioClip _endTurnAudioClip;
    public BattleGrid BattleGrid
    {
        get => _gameManager.BattleGrid;
    }

    public UIManager UIManager
    {
        get => _gameManager.UIManager;
        set => _gameManager.UIManager = value;
    }

    public PlayerManager PlayerManager
    {
        get => _gameManager.PlayerManager;
    }

    public Unit SelectedHero
    {
        get => _selectedHero;
        set => _selectedHero = value;
    }

    public bool CanSelectUnit { get; set; }

    [SerializeField] private List<ScriptableUnit> _units;
    [SerializeField] private Unit _selectedHero;
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
            //SelectUnit();
        }

        if (Input.GetKeyDown(KeyCode.A)) _selectedHero.EndTurn();
    }

    public void EndTurn()
    {
        if (!SelectedHero)
        {
            PlayerManager.CurrentPlayer.Units.Where(i => i.unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn).ForEach(i => i.Rest());
        }
    }

    public void Build()
    {
        SelectedHero.GetComponent<Builder>().BuildStructure(_gameManager);
    }

    public void SpawnUnit()
    {
        var buildSpawnUnit = SelectedHero.GetComponent<Building>();
       buildSpawnUnit.SpawnUnit(UIManager.UnitSheetUI.ScriptableUnit.unitsName);
       UIManager.InvokeUpdateUI();
    }

    private Character GetRandomCharacterPerFaction(Faction faction)
    {
        List<ScriptableUnit> FactionUnit = GetFactionScriptableUnits(faction);
        int randomIndex = Random.Range(0, FactionUnit.Count);
        GameObject unitObj = new GameObject("", typeof(Character), typeof(SpriteRenderer));
        Character newUnit = unitObj.GetComponent<Character>();
        SpriteRenderer unitRenderer = unitObj.GetComponent<SpriteRenderer>();
        newUnit.ScrUnit = FactionUnit[randomIndex];
        CheckifBuilder(newUnit);
        unitRenderer.sprite = newUnit.ScrUnit.renderUnit;
        unitRenderer.sortingOrder = 1;
        unitObj.name = newUnit.ScrUnit.unitsName;
        return newUnit;
    }

    private void CheckifBuilder(Character newUnit)
    {
        if (newUnit.ScrUnit.isBuilder)
        {
            Builder builder = newUnit.gameObject.AddComponent<Builder>();
            builder.BuilderUnit = newUnit;
        }
    }

    internal void UpdateUnitsList()
    {
        throw new System.NotImplementedException();
    }

    private bool UpdateAndCheckifTeamisDead(Faction currentFaction)
    {
        bool isDead = PlayerManager.GetPlayerPerFaction(currentFaction).Units.Count(i =>
                          i.ScrUnit.unitsName == "MotherBase" &&
                          i.unitStateMachine.currentState == UnitStateMachine.UnitState.Dead) ==
                      1;
        if (isDead)
        {
            UIManager.InvokeInformation("Partie termin? ! Faction " + currentFaction + " battue !");
        }
        return isDead;
    }

    private IEnumerable<Building> GetFactionBuilding(Faction currentFaction)
    {
        return PlayerManager.Players.Where(i => i.PlayerFaction == currentFaction).SelectMany(i => i.Buildings).ToList();
    }

    private IEnumerable<Character> GetFactionCharacters(Faction currentFaction)
    {
        //TODO
        return PlayerManager.Players.Where(i => i.PlayerFaction == currentFaction).SelectMany(i => i.Characters);
    }

    public void SelectUnit(Unit unit)
    {
        unit.GetComponent<SpriteRenderer>().color = Color.white;
        SelectedHero = unit;
        SelectedHero.GetComponent<SpriteRenderer>().color = Color.blue;
        UIManager.InvokeInformation("Tours de : " + SelectedHero.ScrUnit.unitsName + ".\n Appuyer sur A pour finir son action.");
    }

    public void DeselectUnit()
    {
        SelectedHero.GetComponent<SpriteRenderer>().color = Color.white;
        SelectedHero = null;
    }

    public void Init(GameManager gm)
    {
        _gameManager = gm;
        _gameManager.UnitManager.CanSelectUnit = true;
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
            Character randomPrefab = GetRandomCharacterPerFaction(currentfaction);
            Vector3Int randomSpawnBattleGridTile = _gameManager.BattleGrid.SpawnUnitPerFaction(currentfaction);
            _gameManager.PlayerManager.SetCharacter(randomPrefab, randomSpawnBattleGridTile);
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
                CheckifBuilder(newUnit);
                return newUnit;
            }
        }
        Debug.LogError("Unit was not found !");
        return null;
    }

    public Character GetSpecificCharacterPerIndex(int index, Faction UnitFaction)
    {
        List<ScriptableUnit> FactionUnit = GetFactionScriptableUnits(UnitFaction);
        GameObject unitObj = new GameObject("", typeof(Character), typeof(SpriteRenderer));
        Character newUnit = unitObj.GetComponent<Character>();
        SpriteRenderer unitRenderer = unitObj.GetComponent<SpriteRenderer>();
        newUnit.ScrUnit = FactionUnit[index];
        CheckifBuilder(newUnit);
        unitRenderer.sprite = newUnit.ScrUnit.renderUnit;
        unitRenderer.sortingOrder = 1;
        unitObj.name = newUnit.ScrUnit.unitsName;
        return newUnit;
    }

    public Building GetSpecificBuildingPerName(string unitName, Faction UnitFaction)
    {
        ScriptableUnit FactionUnit = _units.FirstOrDefault(i => i.unitsName == unitName).GetCloneUnit();
        FactionUnit.faction = UnitFaction;
        GameObject unitObj = new GameObject(unitName, typeof(Building), typeof(SpriteRenderer));
        Building newUnit = unitObj.GetComponent<Building>();
        SpriteRenderer unitRenderer = unitObj.GetComponent<SpriteRenderer>();
        newUnit.ScrUnit = FactionUnit;
        unitRenderer.sprite = newUnit.ScrUnit.renderUnit;
        unitRenderer.sortingOrder = 1;
        unitObj.name = newUnit.ScrUnit.unitsName;
        if (newUnit.ScrUnit != null)
        {
            return newUnit;
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

    public void UpdateCharactersRenderAndSate(List<Character> units)
    {
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].unitStateMachine.currentState != UnitStateMachine.UnitState.Dead)
            {
                SpriteRenderer uniRenderer = units[i].GetComponent<SpriteRenderer>();
                uniRenderer.color = Color.white;
                units[i].unitStateMachine.currentState = UnitStateMachine.UnitState.None;
                units[i].HasBeenUsed = false;
                units[i].InitCurrentMv();
            }
        }
    }

    public void UpdateBuildingsRenderAndSate(List<Building> units)
    {
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].unitStateMachine.currentState != UnitStateMachine.UnitState.Dead)
            {
                SpriteRenderer uniRenderer = units[i].GetComponent<SpriteRenderer>();
                uniRenderer.color = Color.white;
                units[i].unitStateMachine.currentState = UnitStateMachine.UnitState.None;
                units[i].HasBeenUsed = false;
                if (units[i].ScriptableBuilding.armorBonus > 0 && units[i].ScriptableBuilding.charactersUnlocked.Count < 0)
                {
                    units[i].BuildingMouseEvent();
                }
                else if (units[i].ScriptableBuilding.charactersUnlocked.Count > 0 )
                {
                    PlayerManager.CurrentPlayer.UpdateDefaultScriptableUnits(units[i].ScriptableBuilding.charactersUnlocked);
                }
            }
        }
    }

    public void UpdateUnitsList(Faction faction)
    {
        switch (faction)
        {
            case Faction.Hero:

                break;
            case Faction.Enemy:
                break;
            case Faction.Building:
                break;
            default:
                break;
        }
    }


    public IEnumerator GameLoop()
    {
        #region TMP

        UIManager.InvokeInformation("Debut de Game");
        bool gameOver = false;
        while (!gameOver)
        {
            PlayerManager.index = 0;
            PlayerManager.CurrentPlayer.AddResource();
            UpdateCharactersRenderAndSate(PlayerManager.CurrentPlayer.Characters);
            UpdateBuildingsRenderAndSate(PlayerManager.CurrentPlayer.Buildings);
            //SelectedHero = allDeployedHeroesCharacters[0] ;
            UIManager.InvokeUpdateUI();
            //SelectedHero = allDeployedHeroesCharacters[0];
            //SelectedHero.GetComponent<SpriteRenderer>().color = Color.blue;
            //UIManager.InvokeInformation("Tours de : " + SelectedHero.ScrUnit.unitsName);

            yield return new WaitUntil(() => PlayerManager.CurrentPlayer.CheckifAllUnitsHasEndTurn());
            gameOver = UpdateAndCheckifTeamisDead(Faction.Hero);
            _gameManager.SoundManager.PlaySound(_endTurnAudioClip);

            PlayerManager.index++;
            PlayerManager.CurrentPlayer.AddResource();
            UIManager.InvokeUpdateUI();
            //SelectedHero = allDeployedEnemiesCharacters[0];
            //SelectedHero.GetComponent<SpriteRenderer>().color = Color.blue;
            //UIManager.InvokeInformation("Tour de : " + SelectedHero.ScrUnit.unitsName);

            yield return new WaitUntil(() => PlayerManager.CurrentPlayer.CheckifAllUnitsHasEndTurn());
            gameOver = UpdateAndCheckifTeamisDead(Faction.Enemy);
            UpdateCharactersRenderAndSate(PlayerManager.CurrentPlayer.Characters);
            UpdateBuildingsRenderAndSate(PlayerManager.CurrentPlayer.Buildings);
            _gameManager.SoundManager.PlaySound(_endTurnAudioClip);
        }

        yield return null;
        #endregion
    }

}
