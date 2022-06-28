using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEAM2
{
    public class PlaySoundOnStrart : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;

        private void Start()
        {
            SoundManager.Instance.PlaySound(_clip);
        }
    }
}
