using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEAM2
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource, _effectsSource;
      public void PlaySound(AudioClip clip)
        {
            _effectsSource.PlayOneShot(clip);
        }
    }
}
