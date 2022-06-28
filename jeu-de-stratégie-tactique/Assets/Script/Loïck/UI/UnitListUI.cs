using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace TEAM2
{
    public class UnitListUI : MonoBehaviour
    {
#region INTERNAL TYPES
        [System.Serializable]
        struct UnitTypeToRoot
        {
            public UnitType UnitType;
            public Transform Root;
        }
#endregion

        [SerializeField] private GameObject _parentCharacterGameObject;
        [SerializeField] private GameObject _parentBuildingsGameObject;
        [SerializeField] private GameObject _prefabSheet;
        [SerializeField] private UIManager _uIManager;

        [SerializeField] private UnitTypeToRoot[] _unitTypeRoots;

        public List<UnitSheetUI> UnitSheets { get; private set; }

        void Awake()
        {
            UnitSheets = new List<UnitSheetUI>();
            _uIManager.UpdateUnitsList += UpdateList;
        }

        void OnDestroy()
        {
            _uIManager.UpdateUnitsList -= UpdateList;
        }

        void UpdateList(IEnumerable<Unit> units, UnitType unitType)
        {
            IEnumerable<Unit> AllUnitPerSheet() => UnitSheets.Select(i => i.UnitReferenced).Where(i => i.UnitType == unitType);

            if (units.Intersect(AllUnitPerSheet()).Count() != units.Count())      // Difference between the two collections
            {
                // Sheet a trop d'élements 	Void TEAM2.UIManager:InvokeUpdateUI ()+0x3d at F:\AganecyGame\jeu-de-stratégie-tactique\Assets\Script\Loïck\UI\UIManager.cs:[43:13-43:100]	C#

                foreach (var el in AllUnitPerSheet().Except(units)
                    .SelectMany((u => UnitSheets.Where(i => i.UnitReferenced==u)))) 
                {
                    Destroy(el.gameObject);
                }

                // Unit a de nouvelles instances
                foreach (var el in units.Except(AllUnitPerSheet())) // Sheet a trop d'élements
                {
                    UnitSheetUI unitSheetUi = Instantiate(_prefabSheet, _unitTypeRoots.FirstOrDefault(i => i.UnitType == unitType).Root)
                        .GetComponent<UnitSheetUI>()
                        .Init(el);
                    UnitSheets.Add(unitSheetUi);
                }
                Debug.Log("test");
            }
        }
    }
}
