using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TEAM2
{
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        // Start is called before the first frame update
        void Start()
        {
            _slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMasterVolume(val));
        }

       
    }
}
