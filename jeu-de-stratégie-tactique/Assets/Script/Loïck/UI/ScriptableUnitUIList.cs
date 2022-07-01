using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using Sirenix.OdinInspector;
using TEAM2;
using UnityEngine;

namespace TEAM2
{
    public class ScriptableUnitUIList : MonoBehaviour
    {

        [SerializeField] private Transform _parentTransform;
        [SerializeField] private GameObject _prefabSheet;
        [SerializeField] private UIManager _uiManager;

        void Awake()
        {
            _uiManager.UpdateScriptablelist += GenerateList;
        }

        void OnDestroy()
        {
            _uiManager.UpdateScriptablelist -= GenerateList;
        }

        void GenerateList(List<ScriptableUnit> _database)
        {
            if (_parentTransform.transform.childCount != 0)
            {
                foreach (Transform child in _parentTransform)
                {
                    Destroy(child.gameObject);
                }
            }

            foreach (var el in _database)
            {
                Instantiate(_prefabSheet, _parentTransform).GetComponent<UnitSheetUI>().Init(el);
            }
        }
    }

}