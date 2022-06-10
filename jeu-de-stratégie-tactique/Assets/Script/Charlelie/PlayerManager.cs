using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace TEAM2
{
    public class PlayerManager : MonoBehaviour
    {
        private GameManager _gameManager;

        private GameObject[] _playersGameObjects;

        private List<Player> _players = new List<Player>();

        public GameObject[] PlayersGameObjects
        {
            get => _playersGameObjects;
        }

        public Player CurrentPlayer
        {
            get => _players[index];
        }

        public int index;

        public List<Player> Players
        {
            get => _players;
        }

        public void Init(GameManager gm)
        {
            _gameManager = gm;
            index = 0;
            _playersGameObjects = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < _playersGameObjects.Length; i++)
            {
                _players.Add(_playersGameObjects[i].GetComponent<Player>());
                _players[i].Init(_gameManager);
            }
            _players[0].PlayerFaction = Faction.Hero;
            _players[1].PlayerFaction = Faction.Enemy;
        }

        public void SetUnit(Character unit, Vector3 unitPos)
        {
            unit.transform.position = unitPos;
            unit.Init(_gameManager);
        }

        public void SetBuilding(Building unit, Vector3 unitPos)
        {
            unit.transform.position = unitPos;
            unit.Init(_gameManager);
        }

        public void CheckIfOrdersFinished()
        {
            for (int i = 0; i < PlayersGameObjects.Length; i++)
            {
                if (!PlayersGameObjects[i].GetComponent<Player>().Ready)
                    return;
            }
            GameManager.Instance.ChangeState(GameManager.GameState.RESOLUTIONPHASE);
        }

        private List<Unit> GetAllUnits()
        {
            List<Unit> Units = new List<Unit>();
            for (int i = 0; i < _players.Count; i++)
            {
                Units.AddRange(_players[i].Units);
            }
            return Units;
        }

        public Unit GetUnit(Vector3Int gridPos)
        {
            List<Unit> characters = GetAllUnits();
            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i].OccupiedTileGridPosition == gridPos)
                {
                    return characters[i];
                }
            }
            return null;
        }

        public bool CheckifUnitWasHere(Vector3Int newUnitPos)
        {
            for (int i = 0; i < GetAllUnits().Count; i++)
            {
                if (GetAllUnits()[i].OccupiedTileGridPosition == newUnitPos)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
