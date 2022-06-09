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
        }
        public bool Ready
        {
            get { return hasFinishOrder; }
        }
        public Faction PlayerFaction;

        [SerializeField] private List<Unit> _unitsList = new List<Unit>();
        private List<Order> orderList = new List<Order>();

        private GameManager _gameManager;

        [SerializeField] private float _resource = 0;

        private bool hasFinishOrder = false;

        public void Init(GameManager gm)
        {
            _gameManager = gm;
            SpawnCharacter();
            for (int i = 0; i < _unitsList.Count; i++)
            {
                _unitsList[i].Init(gm);
            }
        }


        public void AddResource()
        {
            List<Building> buildings = GetUnitWithType(UnitType.Building).Cast<Building>().ToList();
            for (int i = 0; i < buildings.Count; i++)
            {
                if (buildings[i].faction == PlayerFaction)
                {
                    _resource += buildings[i].GainResourcePerTurn;
                }
            }
        }

        public void SpawnCharacter()
        {
            Character[] list = _gameManager.UnitManager.SpawnCharacter(2, PlayerFaction);
            for (int i = _unitsList.Count; i < list.Length; i++)
            {
                _unitsList.Add(list[i]);
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

        public List<Unit> GetUnitWithType(UnitType currentUnitType)
        {
            List<Unit> units = new List<Unit>();
            for (int i = 0; i < _unitsList.Count; i++)
            {
                if (GetUnitClass(_unitsList[i]) == currentUnitType)
                {
                    units.Add(_unitsList[i]);
                }
            }

            return units;
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
        private UnitType GetUnitClass(Unit unit)
        {
            Character unitCharacter = unit as Character;
            Building unitBuilding = unit as Building;
            if (unitCharacter != null)
            {
                return UnitType.Character;
            }
            else
            {
                return UnitType.Building;
            }
        }
    }
}
