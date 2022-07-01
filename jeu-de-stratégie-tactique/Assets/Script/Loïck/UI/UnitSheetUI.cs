using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TEAM2
{
    public class UnitSheetUI : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private ScriptableUnit _scriptableUnit;

        public ScriptableUnit ScriptableUnit => _scriptableUnit;
        public Unit UnitReferenced { get; private set; }

        public UnitSheetUI Init(ScriptableUnit unit)
        {
            _scriptableUnit = unit;
            _cost.text = "Cost : " + unit.unitCost;
            _image.sprite = unit.renderUnit;
            if (unit.faction == Faction.Building)
            {
                var h = (ScriptableBuilding)unit;
                _image.sprite = h.uiSprite;
            }
            _description.text = unit.descritpion;
            return this;
        }
        public UnitSheetUI Init(Unit unit)
        {
            UnitReferenced = unit;
            Init(UnitReferenced.ScrUnit);
            return this;
        }

    }
}
