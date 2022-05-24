using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEAM2
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager instance;

        public List<GameObject> players = new List<GameObject>();

        void Awake()
        {
            if (instance != null)
            {
                Destroy(instance);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(instance);
            }
        }


        public void InitPlayers()
        {
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
