using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TEAM2
{
    public class UIManager : MonoBehaviour
    {
        private GameManager _gameManager;
        [SerializeField] private GameObject unitBuildUI;
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


        public event Action<int,string> UpdateResource;
        public event Action<IEnumerable<Unit>,UnitType> UpdateUnitsList;
        public event Action<string> InformationUpdate;
        public event Action<GameObject> BuildUI;

        public void Init(GameManager gm)
        {
            _gameManager = gm;
        }

       public void InvokeUpdateUI()
        {
            UpdateResource?.Invoke(Gold,Player.name);
            UpdateUnitsList?.Invoke(Player.GetUnitWithType(UnitType.Character),UnitType.Character);
            UpdateUnitsList?.Invoke(Player.GetUnitWithType(UnitType.Building),UnitType.Building);
        }

        public void InvokeBuildUI(GameObject gameObject)
        {
            BuildUI.Invoke(gameObject);
        }

        public void InvokeInformation(string information)
       {
           InformationUpdate?.Invoke(information);
       }
    }
}