using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEAM2
{
    public class Builder : MonoBehaviour
    {
        [SerializeField] private GameObject unitBuildUI;
        [SerializeField] private UIManager _uIManager;
        private Unit builderUnit;
        public Unit BuilderUnit { get => builderUnit; set => builderUnit = value; }
        public GameObject UnitBuildUI { get => unitBuildUI; set => unitBuildUI = value; }

        private void Awake()
        {
            _uIManager.BuildUI += HandleSelection;
        }

        private void OnDestroy()
        {
            _uIManager.BuildUI -= HandleSelection;
        }

        public void HandleSelection(GameObject selectedObject)
        {
            RestBuildingSystem();

            if (selectedObject == null)
                return;
            builderUnit = selectedObject.GetComponent<Unit>();
            if (builderUnit != null)
            {
                HandleUnitSelection();
            }
        }

        public void HandleUnitSelection()
        {
            unitBuildUI.SetActive(true);
        }

        private void RestBuildingSystem()
        {
            builderUnit = null;
            unitBuildUI.SetActive(false);
        }

        public void BuildStructure()
        {
            Debug.Log("Placing structure at" + this.builderUnit.transform.position);
        }
    }
}
