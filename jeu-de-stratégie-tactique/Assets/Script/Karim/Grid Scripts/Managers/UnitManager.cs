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

        GameManager.Instance.ChangeState(GameManager.GameState.EnemiesTurn);
    }
    public void SpawnEnemies()
    {
        int enemyCount = 1;

        for (int i = 0; i < enemyCount; i++)
        {
            BaseUnit randomPrefab = GetRandomUnitPerFaction(Faction.Enemy);
            BaseUnit spawnedEnemy = Instantiate(randomPrefab);
            Tile randomSpawnTile = BattleGrid.instance.SpawnRandomUnit();

            randomSpawnTile.SetUnit(spawnedEnemy);
        }
        GameManager.Instance.ChangeState(GameManager.GameState.HerosTurn);
    }
    private BaseUnit GetRandomUnitPerFaction(Faction faction)
    {
        List<ScriptableUnit> FactionUnit = GetFactionUnits(faction);
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

    public List<ScriptableUnit> GetFactionUnits(Faction currentFaction)
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
    public void SetSelectedHero(BaseUnit hero)
    {
        SelectedHero = hero;
        MenuManager.Instance.ShowSelectedHero(hero);
    }
}
