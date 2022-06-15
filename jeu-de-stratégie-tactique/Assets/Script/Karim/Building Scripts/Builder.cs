using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEAM2
{
    public class Builder : MonoBehaviour
    {
        [SerializeField]
        private GameObject unitBuildUI;
        private Unit builderUnit;

        public void HandleSelection(GameObject selectedObject)
        {
            RestBuildingSystem();

            if (selectedObject == null)
                return;
            builderUnit = selectedObject.GetComponent<Unit>();
            if(builderUnit != null)
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
