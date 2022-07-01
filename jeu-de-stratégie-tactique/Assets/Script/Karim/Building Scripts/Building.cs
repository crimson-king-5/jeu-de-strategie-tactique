using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TEAM2;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Building : Unit
{


    public ScriptableBuilding ScriptableBuilding => (ScriptableBuilding)ScrUnit;
    public List<ScriptableUnit> UnlockedUnits => ScriptableBuilding.charactersUnlocked;
    public UIManager UIManager => unitManager.UIManager;
    public UnitManager unitManager { get => _gameManager.UnitManager; }
    public bool ActiveMouseEvent => _activeMouseEvent;

    private Vector3Int gridPos;
    private BuildingManager BuildingManager => _gameManager.BuildingManager;

    private bool _activeMouseEvent = false; 

    public void BuildingMouseEvent()
    {
        if (unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn)
        {
            if (ScriptableBuilding.canAddUnits)
            {
                Vector3 mouseWorldPosition = BattleGrid.GetMouseWorldPosition();
                Vector3Int gridPos = GetSpecificGridPosition(mouseWorldPosition);
                if (BattleGrid.CellDict.ContainsKey(gridPos) && BattleGrid.CellDict[gridPos] != CellOn && !_activeMouseEvent)
                {
                    _activeMouseEvent = true;
                    this.gridPos = gridPos; 
                    var units = PlayerManager.CurrentPlayer.DefaultScriptable.Where(i =>i.faction == PlayerManager.CurrentPlayer.PlayerFaction).ToList();
                    UIManager.InvokeUnitUI(CellOn, units);
                }
            }
            else if (ScriptableBuilding.armorBonus > 0)
            {
                Debug.Log("Gain Armor : +" + ScriptableBuilding.armorBonus);
                PlayerManager.CurrentPlayer.ApplyBuildingArmor(this);
                Rest();
            }
            else
            {
                Rest();
            }
        }


    }

  public void SpawnUnit(string unitname)
    {
        Cell mouseCell = BattleGrid.CellDict[gridPos];
        if (!PlayerManager.CheckifUnitWasHere(gridPos) &&
            PlayerManager.CurrentPlayer.Gold >= PlayerManager.CurrentPlayer.CostGold)
        {
            bool isCasern = CellOn._Neighbors.a.FirstOrDefault(i => i == mouseCell).Contains == null;
            
            if (isCasern)
            {
                Character character = unitManager.GetSpecificCharacterPerName(unitname, Faction);
                PlayerManager.CurrentPlayer.Gold -= PlayerManager.CurrentPlayer.CostGold;
                PlayerManager.SetCharacter(character, gridPos);
                PlayerManager.CurrentPlayer.Units.Add(character);
                UIManager.InvokeUpdateUI();
                _activeMouseEvent = false;
                Rest();
                character.Rest();
            }
        }
        else
        {
            UIManager.InvokeInformation("Gold insuffisant.");
        }
    }

    public void UpdateBuilding(ScriptableBuilding newBuilding, Cell buildingCell, GameManager gm)
    {
        _gameManager = gm;
        Building newUnitBuilding = _gameManager.UnitManager.GetSpecificBuildingPerName(newBuilding.unitsName, PlayerManager.CurrentPlayer.PlayerFaction);
        PlayerManager.SetBuilding(newUnitBuilding, buildingCell.Position);
        buildingCell.Contains.Rest();
        PlayerManager.CurrentPlayer.Buildings.Add(newUnitBuilding);
        PlayerManager.CurrentPlayer.Units.Add(newUnitBuilding);
        Building oldBuilding = BuildingManager.Buildings.First(i => i == this);
        oldBuilding = newUnitBuilding;
        Destroy(gameObject);
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