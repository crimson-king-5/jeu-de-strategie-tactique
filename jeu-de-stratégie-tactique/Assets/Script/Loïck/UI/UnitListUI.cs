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
        [SerializeField] private UIManager _uIManager;

        // Start is called before the first frame update
        void Start()
        {
            _uIManager.UpdateUnitsList += UpdateUnitList;
        }

        void OnDestroy()
        {
            _uIManager.UpdateUnitsList -= UpdateUnitList;
        }

        void UpdateUnitList(string characters, string buildings)
        {
            _charactersText.text = characters;
            _buildingsText.text = buildings;
        }
    }
}
