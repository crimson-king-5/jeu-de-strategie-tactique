using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
            get => _buildings ;
        }

        public List<Character> Characters
        {
            get => _characters ;
        }

        public BuildingManager BuildingManager
        {
            get => _gameManager.BuildingManager;
        }

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

        [SerializeField] private List<Unit> _unitsList = new List<Unit>();
        private List<Order> orderList = new List<Order>();

        private GameManager _gameManager;
        private List<Character> _characters = new List<Character>();
        private List<Building> _buildings = new List<Building>();

        [SerializeField] private int _costgold = 1;
        [SerializeField] private int _gold = 0;

        private bool hasFinishOrder = false;

        public void Init(GameManager gm)
        {
            _gameManager = gm;
            SpawnCharacter();
            for (int i = 0; i < _unitsList.Count; i++)
            {
                _unitsList[i].Init(gm,UnitType.Character);
            }
            for (int i = 0; i < BuildingManager.Buildings.Count; i++)
            {
                if (BuildingManager.Buildings[i].Faction == PlayerFaction)
                {
                    _unitsList.Add(BuildingManager.Buildings[i]);
                    _buildings.Add(BuildingManager.Buildings[i]);
                }
            }
        }

        public string GetListUnitNamePerUnitype(UnitType unitType)
        {
            string listName = "";
            listName += GetUnitWithType(unitType).SelectMany(i => i.ScrUnit.unitsName + "\n");
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

        public void SpawnCharacter()
        {
            Character[] list = _gameManager.UnitManager.SpawnCharacter(2, PlayerFaction);
            for (int i = _unitsList.Count; i < list.Length; i++)
            {
                _unitsList.Add(list[i]);
                _characters.Add(list[i]);
            }
        }


        public void AddOrderToList(OrderType orderType)
        {
            orderList.Add(new Order(orderType));
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
                if (_unitsList[i].unitStateMachine.currentState != UnitStateMachine.UnitState.EndTurn)
                {
                    return false;
                }

            }

            return true;
        }
    }
}
