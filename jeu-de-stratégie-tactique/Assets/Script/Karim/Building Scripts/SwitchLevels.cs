using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEAM2
{
    public class SwitchLevels : MonoBehaviour
    {
        public GameObject[] levels;
        int current_level = 0;
        public void Upgrade()
        {
            if (current_level < levels.Length - 1)
            {
                current_level++;
                SwitchObject(current_level);
            }
        }

         void SwitchObject(int lvl)
        {
            for (int i = 0; i < levels.Length; i++)
            {
                if (i == lvl)
                    levels[i].SetActive(true);
                else
                    levels[i].SetActive(false);
            }
        }
    }
}
