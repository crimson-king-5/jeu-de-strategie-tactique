using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEAM2
{
    public class BuildingManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject unitBuildUI;
        private Unit builderUnit;

        public void HandleSelection(GameObject selectedObject)
        {
            ResetBuildingSystem();

            if (selectedObject == null)
                return;

            builderUnit = selectedObject.GetComponent<Unit>();
            if(builderUnit != null)
            {
                HandleUnitSelection();
            }
        }

        private void HandleUnitSelection()
        {
            unitBuildUI.SetActive(true);
        }

        private void ResetBuildingSystem()
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
