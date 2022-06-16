using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEAM2
{
    public class BuildingManager : MonoBehaviour
    {
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private List<Building> _buildings;
        public List<Building> Buildings
        {
            get => _buildings;
        }

        public void Init(GameManager gm)
        {
            _gameManager = gm;
        }
    }
}
