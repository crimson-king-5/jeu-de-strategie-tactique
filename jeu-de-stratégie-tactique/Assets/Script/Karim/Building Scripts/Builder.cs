using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;

namespace TEAM2
{
    public class Builder : MonoBehaviour
    {
        private UIManager _uIManager => builderUnit.UIManager;
        private Character builderUnit;
        public Character BuilderUnit { get => builderUnit; set => builderUnit = value; }

        public void BuildStructure(GameManager gameManager)
        {
            if (gameManager.PlayerManager.CurrentPlayer.Gold >= _uIManager.SheetUI.ScriptableUnit.unitCost)
            {
                _uIManager.InvokeInformation("Placing structure ");
                _uIManager.CurrentBuilding.UpdateBuilding((ScriptableBuilding)_uIManager.SheetUI.ScriptableUnit, _uIManager.CurrentCell, gameManager);
                builderUnit.OnBuild();
            }
            else
            {
                _uIManager.InvokeInformation("Gold insufficient ");
            }
        }
    }
}
