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

        public GameObject[] PlayersGameObjects
        {
            get => _playersGameObjects;
        }
        private GameObject[] _playersGameObjects;

        private List<Player> players = new List<Player>();

        public Player CurrentPlayer
        {
            get => players[index];
        }

        public int index;

        public void Init(GameManager gm)
        {
            _gameManager = gm;
            index = 0;
            _playersGameObjects = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < _playersGameObjects.Length; i++)
            {
                players.Add(_playersGameObjects[i].GetComponent<Player>());
                players[i].Init(_gameManager);
            }
            players[0].PlayerFaction = Faction.Hero;
            players[1].PlayerFaction = Faction.Enemy;
        }

        public void SetUnit(Character unit, Vector3 unitPos)
        {
            unit.transform.position = unitPos;
        }

        public void SetBuilding(Building unit, Vector3 unitPos)
        {
            unit.transform.position = unitPos;
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
            for (int i = 0; i < players.Count; i++)
            {
                Units.AddRange(players[i].Units);
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
