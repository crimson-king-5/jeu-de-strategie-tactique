using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEAM2
{
    public class PlayerManager : MonoBehaviour
    {
        private GameManager _gameManager;

        public List<GameObject> players = new List<GameObject>();

        public void Init(GameManager gm)
        {
            _gameManager = gm;
            for (int i = 0; i < players.Count; i++)
            {
                players[i].GetComponent<Player>().Init();
            }
        }

        public void CheckIfOrdersFinished()
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].GetComponent<Player>().Ready)
                    return;
            }
            GameManager.Instance.ChangeState(GameManager.GameState.RESOLUTIONPHASE);
        }
    }
}
