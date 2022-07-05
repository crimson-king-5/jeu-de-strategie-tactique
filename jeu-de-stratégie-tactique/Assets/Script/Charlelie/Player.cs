using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;

namespace TEAM2
{
    public class Player : MonoBehaviour
    {
        public List<Unit> Units
        {
            get => _unitsList;
            set => _unitsList = value;
        }


        public List<Building> Buildings
        {
            get => _unitsList.Where(i => i.UnitType == UnitType.Building).Cast<Building>().ToList();
        }

        public List<Character> Characters
        {
            get => _unitsList.Where(i => i.UnitType == UnitType.Character).Cast<Character>().ToList() ;
        }

        public BuildingManager BuildingManager
        {
            get => _gameManager.BuildingManager;
        }

        public  List<ScriptableUnit> DefaultScriptable => _defaultScriptableUnits;


        public bool Ready
        {
            get { return hasFinishOrder; }
        }
        public int Gold
        {
            get => _gold;
            set => _gold = value;
        }

        public int CostGold
        {
            get => _costgold;
            set => _costgold = value;
        }


        public Faction PlayerFaction;

        private List<Order> orderList = new List<Order>();

        private GameManager _gameManager;

        [SerializeField] private List<ScriptableUnit> _defaultScriptableUnits;
        [SerializeField] private List<Unit> _unitsList = new List<Unit>();
        [SerializeField] private int _costgold = 1;
        [SerializeField] private int _gold = 0;
        [SerializeField] private int charsToCreate = 0;

        private bool hasFinishOrder = false;

        public Order CurrentOrder { get; set; }

        public void Init(GameManager gm)
        {
            _gameManager = gm;
            SpawnCharacter();
            for (int i = 0; i < _unitsList.Count; i++)
            {
                //_unitsList[i].Init(gm,UnitType.Character);
            }
            for (int i = 0; i < BuildingManager.Buildings.Count; i++)
            {
                if (BuildingManager.Buildings[i].Faction == PlayerFaction)
                {
                    _unitsList.Add(BuildingManager.Buildings[i]);
                }
            }
        }

        public string GetListUnitNamePerUnitype(UnitType unitType)
        {
            string listName = "";
            switch (unitType)
            {
                case UnitType.Building:
                    listName = string.Join("\n", Buildings.Select(i => i.ScrUnit.unitsName));
                    break;    
                case UnitType.Character:
                    listName = string.Join("\n", Characters.Select(i => i.ScrUnit.unitsName));
                    break;
            }
            return listName;
        }

        public void AddResource()
        {
            for (int i = 0; i < Buildings.Count; i++)
            {
                if (Buildings[i].Faction == PlayerFaction)
                {
                    _gold = Buildings[i].GainResourcePerTurn(_gold);
                }
            }
        }

        public void UpdateDefaultScriptableUnits(List<ScriptableUnit> scriptableUnits)
        {
            _defaultScriptableUnits.AddRange( scriptableUnits.Where(i => i.faction == PlayerFaction).Except(_defaultScriptableUnits).ToList());
        }

        public List<Building> SpecificBuildingsListPerUnitNames(string buildingName)
        {
             return _unitsList.Where(i => i.ScrUnit.unitsName == buildingName && i.UnitType == UnitType.Building).Cast<Building>().ToList();
        }

        public void SpawnCharacter()
        {
            Character[] list = _gameManager.UnitManager.SpawnCharacter(charsToCreate, PlayerFaction);
            for (int i = _unitsList.Count; i < list.Length; i++)
            {
                _unitsList.Add(list[i]);
                list[i].Master = this;
            }
        }


        public void AddOrderToList(OrderType orderType)
        {
            orderList.Add(new Order(orderType));
        }

        public void AddOrderToList(Order order)
        {
            orderList.Add(order);
        }

        public void ExecuteOrders(List<Order> orderList)
        {
            //Execute both own list and received order list from client
        }

        public IEnumerable<Unit> GetUnitWithType(UnitType currentUnitType)
        {
            return _unitsList.Where(i=> i.UnitType == currentUnitType);
        }

        public bool CheckifAllUnitsHasEndTurn()
        {
            for (int i = 0; i < _unitsList.Count; i++)
            {
                //Debug.Log(_unitsList[i].ScrUnit.name + "   " + _unitsList[i].unitStateMachine.currentState);
                if (_unitsList[i].unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn)
                {
                    return false;
                }

            }

            return true;
        }

        public void MakeUnitsEnd()
        {
            for (int i = 0; i < _unitsList.Count; i++)
            {
                _unitsList[i].unitStateMachine.currentState = UnitStateMachine.UnitState.EndTurn;

            }
        }

        public void ApplyBuildingArmor(Building building)
        { 
            building.CellOn.CheckAllyNeighbours(building).Where(i => i.UnitType == UnitType.Character).ForEach( i => i.AddArmor(building.ScriptableBuilding.armorBonus));
        }
    }
}
