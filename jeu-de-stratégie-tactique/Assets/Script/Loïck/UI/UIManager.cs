using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEAM2
{
    public class UIManager : MonoBehaviour
    {
        private GameManager _gameManager;

        public void Init(GameManager gm)
        {
            _gameManager = gm;
        }
    }
}