using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEAM2
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject unitBuildUI;
        private GameManager _gameManager;
        public GameObject UnitBuildUI { get => unitBuildUI; }
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
        public event Action<GameObject> BuildUI;

        public void Init(GameManager gm)
        {
            _gameManager = gm;
        }

        public void InvokeBuildUI(GameObject gameObject)
        {
            BuildUI.Invoke(gameObject);
        }

        public void InvokeUpdateUI()
        {
            UpdateResource?.Invoke(Lunarite,Gold);
            UpdateUnitsList?.Invoke(Player.GetListUnitNamePerUnitype(UnitType.Character), Player.GetListUnitNamePerUnitype(UnitType.Building));
        }
    }
}