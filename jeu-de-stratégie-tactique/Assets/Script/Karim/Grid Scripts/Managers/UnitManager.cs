using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TEAM2;
using UnityEngine;

public class UnitManager : MonoBehaviour
{

    [SerializeField] private GameManager _gameManager;


    [SerializeField] private List<ScriptableUnit> _units;

    public Character SelectedHero;

    [SerializeField] private List<ScriptableUnit> heroesUnits = new List<ScriptableUnit>();
    [SerializeField] private List<ScriptableUnit> enemyUnits = new List<ScriptableUnit>();
    [SerializeField] private List<ScriptableUnit> neutralUnits = new List<ScriptableUnit>();

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

    public void Init(GameManager gm)
    {
        _gameManager = gm;
    }

    public Character[] SpawnCharacter(int unitCount, Faction currentfaction)
    {
        Character[] chars = new Character[unitCount];

        for (int i = 0; i < unitCount; i++)
        {
            Character randomPrefab = GetRandomUnitPerFaction(currentfaction);
            Vector3 randomSpawnBattleGridTile = _gameManager.BattleGrid.SpawnRandomUnit();
            _gameManager.PlayerManager.SetUnit(randomPrefab, randomSpawnBattleGridTile);
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
        newUnit.ScrUnit = FactionUnit[randomIndex];
        unitRenderer.sprite = newUnit.ScrUnit.renderUnit;
        unitRenderer.sortingOrder = 1;
        unitObj.name = newUnit.ScrUnit.unitsName;
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


    public void UpdateUnitsTurns(List<Character> units)
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

        Debug.Log("Debut de Game");
        List<Character> allDeployedHeroesUnits = GetFactionUnits(Faction.Hero);
        List<Character> allDeployedEnemiesUnits = GetFactionUnits(Faction.Enemy);
        bool gameOver = false;
        while (!gameOver)
        {
            _gameManager.PlayerManager.index = 0;
            UpdateUnitsTurns(allDeployedHeroesUnits);
            for (int i = 0; i < allDeployedHeroesUnits.Count; i++)
            {
                SelectedHero = allDeployedHeroesUnits[i];
                Debug.Log(SelectedHero.ScrUnit.unitsName);
                if (SelectedHero.ScrUnit.unitStats.life <= 0 && SelectedHero.unitStateMachine.currentState != UnitStateMachine.UnitState.Dead)
                {
                    SelectedHero.unitStateMachine.currentState = UnitStateMachine.UnitState.Dead;
                    SelectedHero.gameObject.SetActive(false);
                }
                else
                {
                    yield return new WaitUntil(() => SelectedHero.unitStateMachine.currentState == UnitStateMachine.UnitState.EndTurn);
                }
            }
            gameOver = CheckifTeamisDead(Faction.Hero);
            _gameManager.PlayerManager.index++;
            for (int i = 0; i < allDeployedEnemiesUnits.Count; i++)
            {
                SelectedHero = allDeployedEnemiesUnits[i];
                Debug.Log("Tour de : " + SelectedHero.ScrUnit.unitsName);
                if (SelectedHero.ScrUnit.unitStats.life <= 0 && SelectedHero.unitStateMachine.currentState != UnitStateMachine.UnitState.Dead)
                {
                    Debug.Log(SelectedHero.ScrUnit.unitsName + " est mort !");
                    SelectedHero.unitStateMachine.currentState = UnitStateMachine.UnitState.Dead;
                    SelectedHero.gameObject.SetActive(false);
                }
                else
                {
                    yield return new WaitUntil(() => SelectedHero.unitStateMachine.currentState == UnitStateMachine.UnitState.EndTurn);
                }
            }

            gameOver = CheckifTeamisDead(Faction.Enemy);
            UpdateUnitsTurns(allDeployedEnemiesUnits);
        }

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
}
