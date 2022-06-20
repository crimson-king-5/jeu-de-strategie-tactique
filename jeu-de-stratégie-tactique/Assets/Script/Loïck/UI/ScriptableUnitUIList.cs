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
        [SerializeField] private string _folderName;
        [SerializeField] private Database _database;




        [Button("RefreshList")]
        void GenerateList()
        {
            if (_parentTransform.transform.childCount != 0)
            {
                foreach (Transform child in _parentTransform)
                {
                    DestroyImmediate(child.gameObject);
                }
            }

            foreach (var el in _database.ScriptableBuildings)
            {
                Instantiate(_prefabSheet, _parentTransform).GetComponent<UnitSheetUI>().Init(el);
            }
        }
    }

}