using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TEAM2
{
    public class UnitSheetUI : MonoBehaviour
    {
        [SerializeField]private Image _image;
        [SerializeField]private TextMeshProUGUI _cost;
        [SerializeField]private TextMeshProUGUI _description;

        public Unit UnitReferenced { get; private set; }

        public UnitSheetUI Init(Unit unit)
        {
            UnitReferenced = unit;

            _cost.text = "Cost : " + unit.ScrUnit.unitCost;
            _image.sprite = unit.ScrUnit.renderUnit;
            _description.text = unit.ScrUnit.descritpion;
            return this;
        }

    }
}
