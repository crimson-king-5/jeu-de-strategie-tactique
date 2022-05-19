using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System;

namespace TEAM2
{
    public class LobbyUI : NetworkBehaviour
    {

        [Header("References")]
        [SerializeField] private LobbyPlayerCard[] lobbyPlayerCards;
        [SerializeField] private Button startGameButton;

        private NetworkList<LobbyPlayerState> lobbyPlayers = new NetworkList<LobbyPlayerState>();
        public override void OnNetworkSpawn()
        {
            if (NetworkManager.Singleton.IsClient)
            {
                lobbyPlayers.OnListChanged += HandheldLobbyPlayersStateChanged;
            }
            if (NetworkManager.Singleton.IsServer)
            {

            }
        }

        private void HandheldLobbyPlayersStateChanged(NetworkListEvent<LobbyPlayerState> changeEvent)
        {
            throw new NotImplementedException();
        }
    }
}
