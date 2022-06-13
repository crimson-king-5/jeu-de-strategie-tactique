using System.Collections;
using System.Collections.Generic;
using TEAM2;
using UnityEngine;

public class Building : Unit
{
    [SerializeField] private Productor _productor = new Productor();
    [SerializeField] private List<string> _unitNames = new List<string>();
    [SerializeField] private int index = 0;
    public string UnitName { get => _unitNames[index];}
    public UnitManager unitManager { get => _gameManager.UnitManager;}
public void Rest()
    {
        SpriteRenderer unitRenderer = GetComponent<SpriteRenderer>();
        unitRenderer.color = Color.gray;
        unitStateMachine.currentState = UnitStateMachine.UnitState.EndTurn;

    }

    public void BuildingMouseEvent()
    {
        if (unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn)
        {
            Vector3 mouseWorldPosition = BattleGrid.GetMouseWorldPosition();
            Vector3Int gridPos = GetSpecificGridPosition(mouseWorldPosition);
            if (BattleGrid.Tilemap.HasTile(gridPos))
            {
                FactionTile factionTile = (FactionTile)BattleGrid.Tilemap.GetTile(gridPos);
                if (factionTile.faction == Faction && factionTile.currentTileType == BattleGridTile.TileType.Spawn && !PlayerManager.CheckifUnitWasHere(gridPos) && PlayerManager.CurrentPlayer.Gold >= PlayerManager.CurrentPlayer.CostGold)
                {
                    Character character = unitManager.GetSpecificCharacterPerName("Lorrence", Faction);
                    PlayerManager.CurrentPlayer.Units.Add(character);
                    PlayerManager.CurrentPlayer.Gold -= PlayerManager.CurrentPlayer.CostGold;
                    PlayerManager.SetUnit(character, mouseWorldPosition);
                    Rest();
                }
            }
        }
    }
    public Faction Faction
    {
        get => _scrUnit.faction;
    }

    public void Update()
    {
        if (_gameManager.UnitManager.SelectedHero == this)
        {
            if (Input.GetMouseButtonDown(0))
            {
                BuildingMouseEvent();
            }

            if (Input.GetMouseButtonDown(1))
            {
                Rest();
            }
        }
    }

    public ResourceType resourceType;

    public UprgadeList UpgradeList
    {
        get
        {
            ScriptableBuilding building = (ScriptableBuilding)_scrUnit;
            return building.upgrades;
        }

    }

    public GridBuildingSystem GridBuildingSystem
    {
        get => _gameManager.GridBuildingSystem;
    }

    public int currentlevelBuilding = 0;

    public void Upgrade()
    {
        if (currentlevelBuilding < UpgradeList.upgrades.Count)
        {
            currentlevelBuilding++;
            SwitchObject(currentlevelBuilding);
        }
    }

    public int GainResourcePerTurn(int currentResource)
    {
        return (int)(currentResource + _productor.resource * UpgradeList.upgrades[currentlevelBuilding].gainBonus);
    }

    void SwitchObject(int lvl)
    {
        for (int i = 0; i < UpgradeList.upgrades.Count; i++)
        {
            if (i == lvl)
            {
                GetComponent<SpriteRenderer>().sprite = UpgradeList.upgrades[i].upgradeRender;
            }
        }
    }

}