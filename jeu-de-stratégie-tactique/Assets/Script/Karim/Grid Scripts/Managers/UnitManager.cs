using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    private List<ScriptableUnit> _units;

    public BaseUnit SelectedHero;

    private List<ScriptableUnit> heroesUnits = new List<ScriptableUnit>();
    private List<ScriptableUnit> enemyUnits = new List<ScriptableUnit>();

    public List<BaseUnit> allDeployedHeroesUnits;
    public List<BaseUnit> allDeployedEnemiesUnits;

    private void Awake()
    {
        Instance = this;

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

    public void SpawnHeroes(int unitCount)
    {
        int heroCount = unitCount;

        for (int i = 0; i < heroCount; i++)
        {
            BaseUnit randomPrefab = GetRandomUnitPerFaction(Faction.Hero);
            Tile randomSpawnTile = BattleGrid.instance.SpawnRandomUnit();
            randomPrefab.xPos = randomSpawnTile.tileXPos;
            randomPrefab.yPos = randomSpawnTile.tileYPos;
            randomSpawnTile.SetUnit(randomPrefab);

        }
        GameManager.Instance.ChangeState(GameManager.GameState.SpawnEnemies);
    }
    public void SpawnEnemies()
    {

        int enemyCount = 1;

        for (int i = 0; i < enemyCount; i++)
        {
            BaseUnit randomPrefab = GetRandomUnitPerFaction(Faction.Enemy);
            Tile randomSpawnTile = BattleGrid.instance.SpawnRandomUnit();
            randomPrefab.xPos = randomSpawnTile.tileXPos;
            randomPrefab.yPos = randomSpawnTile.tileYPos;
            randomSpawnTile.SetUnit(randomPrefab);
        }
        GameManager.Instance.ChangeState(GameManager.GameState.LaunchGameLoop);
    }
    private BaseUnit GetRandomUnitPerFaction(Faction faction)
    {
        List<ScriptableUnit> FactionUnit = GetFactionScriptableUnits(faction);
        int randomIndex = Random.Range(0, FactionUnit.Count);
        GameObject unitObj = new GameObject("", typeof(BaseUnit), typeof(SpriteRenderer));
        BaseUnit newUnit = unitObj.GetComponent<BaseUnit>();
        SpriteRenderer unitRenderer = unitObj.GetComponent<SpriteRenderer>();
        newUnit.scriptableUnit = FactionUnit[randomIndex];
        unitRenderer.sprite = newUnit.scriptableUnit.renderUnit;
        unitRenderer.sortingOrder = 1;
        unitObj.name = newUnit.scriptableUnit.unitsName;
        switch (faction)
        {
            case Faction.Enemy:
                allDeployedEnemiesUnits.Add(newUnit);
                break;
            case Faction.Hero:
                allDeployedHeroesUnits.Add(newUnit);
                break;
        }
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


    public void ResetUnitsTurns(List<BaseUnit> units)
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
        Debug.Log("Debut de Game");
        bool gameOver = false;
        while (!gameOver)
        {
            ResetUnitsTurns(allDeployedHeroesUnits);
            GameManager.Instance.gameState = GameManager.GameState.HerosTurn;
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
            GameManager.Instance.gameState = GameManager.GameState.EnemiesTurn;
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
    }

    private bool CheckifTeamisDead(Faction currentFaction)
    {
        bool isDead = true;
        List<BaseUnit> factionUnits = GetFactionUnits(currentFaction);
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

    private List<BaseUnit> GetFactionUnits(Faction CurrentFaction)
    {
        List<BaseUnit> factionUnits = new List<BaseUnit>();
        switch (CurrentFaction)
        {
            case Faction.Hero:
                factionUnits = allDeployedHeroesUnits;
                break;
            case Faction.Enemy:
                factionUnits = allDeployedEnemiesUnits;
                break;
        }

        return factionUnits;
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
