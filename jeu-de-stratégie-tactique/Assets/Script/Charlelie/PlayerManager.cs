using System.Collections;
using System.Collections.Generic;
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
        }

        public void SetUnit(Character unit,Vector3Int unitPos)
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
    }
}
