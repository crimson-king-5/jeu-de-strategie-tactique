using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace TEAM2
{
    public class UnitListUI : MonoBehaviour
    {
        [SerializeField] private GameObject _parentUnitsGameObject;
        [SerializeField] private GameObject _prefabSheet;
        [SerializeField] private UIManager _uIManager;
        private Vector3 mousPos => Input.mousePosition;
        private Image background => GetComponent<Image>();

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

        public void MouseMove()
        {
            transform.position = new Vector2(mousPos.x, transform.position.y);
        }
        void UpdateList(IEnumerable<Unit> units)
        {
            IEnumerable<Unit> AllUnitPerSheet() => UnitSheets.Select(i => i.UnitReferenced);

            if (units.Intersect(AllUnitPerSheet()).Count() != units.Count())      // Difference between the two collections
            {
                // Sheet a trop d'élements 	Void TEAM2.UIManager:InvokeUpdateUI ()+0x3d at F:\AganecyGame\jeu-de-stratégie-tactique\Assets\Script\Loïck\UI\UIManager.cs:[43:13-43:100]	C#

                foreach (var el in AllUnitPerSheet().Except(units)
                    .SelectMany((u => UnitSheets.Where(i => i.UnitReferenced == u))))
                {
                    if (el)
                    {
                        Destroy(el.gameObject);
                    }
                }

                // Unit a de nouvelles instances
                foreach (var el in units.Except(AllUnitPerSheet())) // Sheet a trop d'élements
                {
                    UnitSheetUI unitSheetUi = Instantiate(_prefabSheet, _parentUnitsGameObject.transform)
                        .GetComponent<UnitSheetUI>()
                        .Init(el);
                    UnitSheets.Add(unitSheetUi);
                }
            }
        }
    }
}
