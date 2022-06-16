using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TEAM2
{
    public class UnitListUI : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI _charactersText;
        [SerializeField] private TextMeshProUGUI _buildingsText;
        [SerializeField] private GameObject _prefabCharacter;
        [SerializeField] private GameObject _prefabBuilding;
        [SerializeField] private UIManager _uIManager;

        void Awake()
        {
            _uIManager.UpdateUnitsList += UpdateList;
        }

        void OnDestroy()
        {
            _uIManager.UpdateUnitsList -= UpdateList;
        }

        void UpdateList(string characters, string buildings)
        {
            if (characters != _charactersText.text)
            {
                _charactersText.text = characters;
                Instantiate(_prefabCharacter);
            }

            if (buildings != _buildingsText.text)
            {
                _buildingsText.text = buildings;
                Instantiate(_prefabCharacter);
            }
        }
    }
}
