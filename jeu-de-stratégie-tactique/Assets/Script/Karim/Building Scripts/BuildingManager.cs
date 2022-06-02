using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEAM2
{
    public class BuildingManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject unitBuildUI;
        private GameManager _gameManager;
        public void Init(GameManager gm)
        {
            _gameManager = gm;
        }
    }
}
