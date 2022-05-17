using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    private List<ScriptableUnit> _units;

    public BaseUnit SelectedHero;

    private void Awake()
    {
        Instance = this;

        _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
    }

    public void SpawnHeroes(int unitCount)
    {
        int heroCount = unitCount;

        for(int i = 0; i < heroCount; i++)
        {
            BaseUnit randomPrefab = GetRandomUnitPerFaction(Faction.Hero);
            BaseUnit spawnedHero = Instantiate(randomPrefab);
            Tile randomSpawnTile = BattleGrid.instance.SpawnUnit();

            randomSpawnTile.SetUnit(spawnedHero);
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
            Tile randomSpawnTile = BattleGrid.instance.SpawnUnit();

            randomSpawnTile.SetUnit(spawnedEnemy);
        }
        GameManager.Instance.ChangeState(GameManager.GameState.HerosTurn);
    }
    private BaseUnit GetRandomUnitPerFaction(Faction faction)
    {
        List<BaseUnit> FactionUnit = new List<BaseUnit>();
        for (int i = 0; i < _units.Count; i++)
        {
            if (_units[i].Faction == faction)
            {
                GameObject unitObj = new GameObject("Null Unit", typeof(BaseUnit));
                BaseUnit newUnit = unitObj.GetComponent<BaseUnit>();
                newUnit.scriptableUnit = _units[i];
                FactionUnit.Add(newUnit);
            }
        }
        int randomIndex = Random.Range(0, FactionUnit.Count);
        return FactionUnit[randomIndex];
    }

    public void SetSelectedHero(BaseUnit hero)
    {
        SelectedHero = hero;
        MenuManager.Instance.ShowSelectedHero(hero);
    }
}
