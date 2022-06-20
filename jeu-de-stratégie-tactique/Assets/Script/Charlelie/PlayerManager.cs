using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering.UI;

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

        [SerializeField] float moveRange;

        public float MoveRange { get => moveRange; }

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

        public void SetCharacter(Unit unit, Vector3Int unitPos)
        {
            unit.transform.position = _gameManager.BattleGrid.CellDict[unitPos].PosCenter;            
            _gameManager.BattleGrid.CellDict[unitPos].Contains = unit;
            (unit as Character).CellOn = _gameManager.BattleGrid.CellDict[unitPos];
            unit.Init(_gameManager,UnitType.Character);
        }

        public void SetBuilding(Building unit, Vector3 unitPos)
        {
            unit.transform.position = unitPos;
            unit.Init(_gameManager,UnitType.Building);
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

        public IEnumerable<Unit> GetAllUnits()
        {
            return _players.SelectMany(i => i.Units);
        }

        public Unit GetUnit(Vector3Int gridPos)
        {
            return GetAllUnits().FirstOrDefault(i => i.OccupiedTileGridPosition == gridPos);
        }

        public bool CheckifUnitWasHere(Vector3Int newUnitPos)
        {
            return GetAllUnits().FirstOrDefault(i => i.OccupiedTileGridPosition == newUnitPos);
        }
        public Player GetPlayerPerFaction(Faction faction)
        {
            switch (faction)
            {
                case Faction.Hero:
                    return Players[0];
                    break;
                case Faction.Enemy:
                    return Players[1];
                    break;
            }
            Debug.LogError("Faction Non reconnue");
            return null;
        }

    }
}
