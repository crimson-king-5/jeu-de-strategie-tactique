using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TEAM2
{
    public class ResourceUI : MonoBehaviour
    {
      [SerializeField]  private TextMeshProUGUI _resourceText;
      [SerializeField]  private UIManager _uIManager;

      void Start()
      {
          _uIManager.UpdateResource += UIResourceUpdate;
      }

      void OnDestroy()
      {
          _uIManager.UpdateResource -= UIResourceUpdate;
      }

      void UIResourceUpdate(int lunarite,int gold,string PlayerName)
      {
          _resourceText.text = PlayerName + "  " + lunarite + " : Lunarite " + "/ " + gold + " Miaounai";
      }
    }
}
