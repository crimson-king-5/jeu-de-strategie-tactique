using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;

namespace TEAM2
{
    public class Builder : MonoBehaviour
    {
        [SerializeField] private GameObject unitBuildUI;
        [SerializeField] private UIManager _uIManager;
        private Character builderUnit;
        public UIManager UIManager { set => _uIManager = value; }
        public Character BuilderUnit { get => builderUnit; set => builderUnit = value; }
        public GameObject UnitBuildUI { set => unitBuildUI = value; }

        private void Start()
        {
            _uIManager.BuildUI += HandleSelection;
        }

        private void OnDestroy()
        {
            _uIManager.BuildUI -= HandleSelection;
        }

        public void HandleSelection(GameObject selectedObject)
        {
            if (selectedObject == null)
                return;
            HandleUnitSelection();
        }

        public void HandleUnitSelection()
        {
            unitBuildUI.SetActive(true);
        }

        public void BuildStructure(GameManager gameManager)
        {
            if (gameManager.PlayerManager.CurrentPlayer.Gold >= _uIManager.SheetUI.ScriptableUnit.unitCost)
            {
                _uIManager.InvokeInformation("Placing structure ");
                _uIManager.CurrentBuilding.UpdateBuilding((ScriptableBuilding)_uIManager.SheetUI.ScriptableUnit, _uIManager.CurrentCell, gameManager);
                BuilderUnit.DoAction();
            }
            else
            {
                _uIManager.InvokeInformation("Gold insufficient ");
            }
        }
    }
}
