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
        private Cell _currentCell;
        [SerializeField] private GameObject unitBuildUI;
        [SerializeField] private GameObject _sheetUI;
        public Building CurrentBuilding { get => (Building)_currentCell.Contains;}
        public Cell CurrentCell { get => _currentCell; set => _currentCell = value; }
        public GameObject UnitBuildUI { get => unitBuildUI; }
        public UnitSheetUI SheetUI => _sheetUI.transform.GetChild(1).GetComponent<UnitSheetUI>();

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
        public event Action<List<ScriptableUnit>> UpdateScriptablelist;

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

        public void InvokeBuildUI(Cell newBuildingCell)
        {
            if (!newBuildingCell.Contains)
            {
                newBuildingCell.Contains = new Building();
            }
            
            _currentCell = newBuildingCell;
            BuildUI?.Invoke(unitBuildUI);
            UpdateScriptablelist?.Invoke(Player.CurrentUnlockedUnits);
        }

        public void InvokeInformation(string information)
       {
           InformationUpdate?.Invoke(information);
       }
    }
}