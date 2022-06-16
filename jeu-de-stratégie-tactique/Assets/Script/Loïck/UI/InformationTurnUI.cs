using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TEAM2
{
    public class InformationTurnUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _informationTurnText;
        [SerializeField] private UIManager _uIManager;
        [SerializeField] [Range(1,11)] private int lineMax = 11;
        private List<string> _allInformation = new List<string>();

        void Start()
        {
            _uIManager.InformationUpdate += AddInformation;
        }
        void OnDestroy()
        {
            _uIManager.InformationUpdate -= AddInformation;
        }

        void AddInformation(string information)
        {
            _allInformation.Add(information);
            if (_allInformation.Count > lineMax)
            {
                _allInformation.Clear();
                _informationTurnText.text = "";
            }
            _informationTurnText.text += "\n" + information;
        }
    }
}
