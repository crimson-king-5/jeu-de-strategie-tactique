using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEAM2
{
    public class UIManager : MonoBehaviour
    {
        private GameManager _gameManager;

        public PlayerManager PlayerManager
        {
            get => _gameManager.PlayerManager;
        }

        public Player Player
        {
            get => PlayerManager.CurrentPlayer;
        }

        private int Gold
        {
            get => Player.Gold;
        }  


        public event Action<int,string> UpdateResource;
        public event Action<string,string> UpdateUnitsList;
        public event Action<string> InformationUpdate;

        public void Init(GameManager gm)
        {
            _gameManager = gm;
        }

       public void InvokeUpdateUI()
        {
            UpdateResource?.Invoke(Gold,Player.name);
            UpdateUnitsList?.Invoke(Player.GetListUnitNamePerUnitype(UnitType.Character), Player.GetListUnitNamePerUnitype(UnitType.Building));
        }

       public void InvokeInformation(string information)
       {
           InformationUpdate?.Invoke(information);
       }
    }
}