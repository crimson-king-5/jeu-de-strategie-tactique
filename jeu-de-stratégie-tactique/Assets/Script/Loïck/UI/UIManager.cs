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
        [SerializeField] private GameObject _unitUI;
        [SerializeField] private GameObject _buttonBuilding;
        [SerializeField] private GameObject _buttonUnit;
        [SerializeField] private GameObject _sheetUI;
        public Building CurrentBuilding { get => (Building)_currentCell.Contains; }
        public Cell CurrentCell { get => _currentCell; set => _currentCell = value; }
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


        public event Action<int, string> UpdateResource;
        public event Action<IEnumerable<Unit>> UpdateUnitsList;
        public event Action<string> InformationUpdate;

        public event Action<List<ScriptableUnit>> UpdateScriptablelist;

        public void Init(GameManager gm)
        {
            _gameManager = gm;
        }

        public void InvokeUpdateUI()
        {
            UpdateResource?.Invoke(Gold, Player.name);
            UpdateUnitsList?.Invoke(Player.Units);
        }

        public void InvokeBuildUI(Cell newBuildingCell, List<ScriptableUnit> units)
        {
            if (!_unitUI.activeSelf && !_buttonBuilding.activeSelf)
            {
                InvokeUI(newBuildingCell,units);
                
                _buttonBuilding.SetActive(true);
                UpdateScriptablelist?.Invoke(units);
            }
        }

       public void InvokeUnitUI(Cell newBuildingCell, List<ScriptableUnit> units)
        {
            if (!_unitUI.activeSelf && !_buttonUnit.activeSelf)
            {
                InvokeUI(newBuildingCell, units);
                _buttonUnit.SetActive(true);
                UpdateScriptablelist?.Invoke(units);
            }
        }

        private void InvokeUI(Cell newBuildingCell, List<ScriptableUnit> units)
        {
            if (!newBuildingCell.Contains)
            {

                newBuildingCell.Contains = new Building();
            }
            _currentCell = newBuildingCell;
            _unitUI.SetActive(true);
        }

        public void InvokeInformation(string information)
        {
            InformationUpdate?.Invoke(information);
        }
    }
}