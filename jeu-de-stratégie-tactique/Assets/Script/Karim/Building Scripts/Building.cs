using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TEAM2;
using UnityEngine;

public class Building : Unit
{

    [SerializeField] private int index = 0;

    public ScriptableBuilding ScriptableBuilding => (ScriptableBuilding)ScrUnit;
    public List<ScriptableUnit> UnlockedUnits => ScriptableBuilding.charactersUnlocked;
    public UIManager UIManager => _gameManager.UIManager;
    public UnitManager unitManager { get => _gameManager.UnitManager; }

    public void BuildingMouseEvent()
    {
        if (unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn)
        {
            if (ScriptableBuilding.canAddUnits)
            {
                Vector3 mouseWorldPosition = BattleGrid.GetMouseWorldPosition();
                Vector3Int gridPos = GetSpecificGridPosition(mouseWorldPosition);
                //Vector3Int place = BattleGrid.Tilemap.GetCellCenterWorld(gridPos);
                if (BattleGrid.CellDict.ContainsKey(gridPos) && ScriptableBuilding.unitStats.range <= GetTileRange(mouseWorldPosition))
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
                        BattleGrid.CellDict.Values.First(i => i.Position == gridPos).Contains = character;
                    }
                }
            }
            else if (ScriptableBuilding.armorBonus < 0 )
            {
                PlayerManager.CurrentPlayer.ApplyBuildingArmor(this);
            }
            else
            {
                Rest();
            }
        }


    }

    public void UpdateBuilding(ScriptableBuilding newBuilding, Cell buildingCell, GameManager gm)
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
                Rest();
            }
        }
    }

    public override void OnClick()
    {
        base.OnClick();
    }

    public int GainResourcePerTurn(int gold)
    {
        return gold + ScriptableBuilding.gainResource;
    }
}