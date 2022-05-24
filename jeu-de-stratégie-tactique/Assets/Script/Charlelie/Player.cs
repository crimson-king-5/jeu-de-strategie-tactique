using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEAM2
{
    public class Player : MonoBehaviour
    {
        List<Unit> unitsList = new List<Unit>();

        List<Order> orderList = new List<Order>();
        bool hasFinishOrder = false;


        public bool Ready
        {
            get { return hasFinishOrder; }
        }


        public void Init()
        {
            SpawnUnit();
        }

        public void SpawnUnit()
        {
            Character[] list = UnitManager.Instance.SpawnCharacter(1);
            for (int i = unitsList.Count; i < list.Length; i++)
            {
                unitsList.Add(list[i]);
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
    }
}
