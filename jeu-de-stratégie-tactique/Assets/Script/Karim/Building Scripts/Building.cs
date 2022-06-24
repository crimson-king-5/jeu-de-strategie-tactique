using System.Collections;
using System.Collections.Generic;
using TEAM2;
using UnityEngine;

public class Building : Unit
{
    [SerializeField] private Productor _productor = new Productor();
    [SerializeField] private int index = 0;
    public int currentlevelBuilding = 0;

    public UIManager UIManager => _gameManager.UIManager;
    public UnitManager unitManager { get => _gameManager.UnitManager; }

    public UprgadeList UpgradeList
    {
        get
        {
            ScriptableBuilding building = (ScriptableBuilding)_scrUnit;
            return building.upgrades;
        }

    }

    public void BuildingMouseEvent()
    {
        if (unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn)
        {
            Vector3 mouseWorldPosition = BattleGrid.GetMouseWorldPosition();
            Vector3Int gridPos = GetSpecificGridPosition(mouseWorldPosition);
            //Vector3Int place = BattleGrid.Tilemap.GetCellCenterWorld(gridPos);
            if (BattleGrid.Tilemap.HasTile(gridPos))
            {
                FactionTile factionTile = (FactionTile)BattleGrid.Tilemap.GetTile(gridPos);
                if (factionTile.faction == Faction && factionTile.currentTileType == BattleGridTile.TileType.Spawn && !PlayerManager.CheckifUnitWasHere(gridPos) && PlayerManager.CurrentPlayer.Gold >= PlayerManager.CurrentPlayer.CostGold)
                {
                    Character character = unitManager.GetSpecificCharacterPerIndex(index, Faction);
                    PlayerManager.CurrentPlayer.Units.Add(character);
                    PlayerManager.CurrentPlayer.Gold -= PlayerManager.CurrentPlayer.CostGold;
                    UIManager.InvokeUpdateUI();
                    PlayerManager.SetCharacter(character, gridPos);
                    Rest();
                }
            }
        }
    }

    public void UpdateBuilding(ScriptableBuilding newBuilding, Cell buildingCell,GameManager gm)
    {
        _gameManager = gm;
        Destroy(buildingCell.Contains);
        buildingCell.Contains = _gameManager.UnitManager.GetSpecificBuildingPerName(newBuilding.unitsName, 
            PlayerManager.CurrentPlayer.PlayerFaction);
        buildingCell.Contains.Init(_gameManager, UnitType.Building);
        buildingCell.Contains.Rest();
        PlayerManager.CurrentPlayer.Buildings.Add(this);
        PlayerManager.CurrentPlayer.Units.Add(this);
    }

    public void Update()
    {
        if (_gameManager.UnitManager.SelectedHero == this)
        {
            if (Input.GetMouseButtonDown(0))
            {
                BuildingMouseEvent();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                //Rest();
            }
        }
    }

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