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
        [SerializeField] private List<Unit> _unitsList = new List<Unit>();
        public List<Unit> Units
        {
            get => _unitsList;
        }

       [SerializeField] private List<Character> _characters = new List<Character>();
        public List<Character> Characters
        {
            get => Characters;
        }

        List<Order> orderList = new List<Order>();

        public Faction PlayerFaction;

        private GameManager _gameManager;

        bool hasFinishOrder = false;


        public bool Ready
        {
            get { return hasFinishOrder; }
        }


        public void Init(GameManager gm)
        {
            _gameManager = gm;
            SpawnCharacter();
            for (int i = 0; i < _unitsList.Count; i++)
            {
                _unitsList[i].Init(gm);
            }
        }

        public void SpawnCharacter()
        {
            Character[] list = _gameManager.UnitManager.SpawnCharacter(1, PlayerFaction);
            for (int i = _unitsList.Count; i < list.Length; i++)
            {
                _unitsList.Add(list[i]);
                _characters.Add(list[i]);
            }
        }

        void SendOrdersToClientRPC()
        {
            //Send order list to client, so he can play resolution phase without latency or desync
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
