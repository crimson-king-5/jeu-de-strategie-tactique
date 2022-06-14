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
        private int Lunarite
        {
            get => Player.Lunarite;
        }

        public event Action<int,int> UpdateResource;
        public event Action<string,string> UpdateUnitsList;

        public void Init(GameManager gm)
        {
            _gameManager = gm;
        }

       public void InvokeUpdateUI()
        {
            UpdateResource?.Invoke(Lunarite,Gold);
            UpdateUnitsList?.Invoke(Player.GetListUnitNamePerUnitype(UnitType.Character), Player.GetListUnitNamePerUnitype(UnitType.Building));
        }
    }
}