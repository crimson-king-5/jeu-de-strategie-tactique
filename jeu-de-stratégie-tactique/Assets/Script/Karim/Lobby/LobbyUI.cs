using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System;
using DapperDino.UMT.Lobby.Networking;

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

            if (IsClient)
            {
                lobbyPlayers.OnListChanged += HandheldLobbyPlayersStateChanged;
            }
            if (IsServer)
            {
                startGameButton.gameObject.SetActive(true);

                NetworkManager.Singleton.OnClientConnectedCallback += HandlecClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback += HandlecClientDisconnect;


                foreach(NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
                {
                    HandlecClientConnected(client.ClientId);
                }
            }
        }
        private void OnDestroy()
        {
            lobbyPlayers.OnListChanged -= HandheldLobbyPlayersStateChanged;

            if (NetworkManager.Singleton)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= HandlecClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback -= HandlecClientDisconnect;

            }

       
        }


        private bool IsEveroneReady()
        {
            if(lobbyPlayers.Count < 2)
            {
                return false;

            }

            foreach (var  player in lobbyPlayers)
            {
                if (!player.IsReady)
                {
                    return false;
                }
            }
            return true;
        }
        private void HandlecClientConnected(ulong clientId)
        {
            var playerData= ServerGameNetPortal.Instance.GetPlayerData(clientId);

            if (!playerData.HasValue) { return; }

            lobbyPlayers.Add(new LobbyPlayerState(clientId, playerData.Value.PlayerName, false));
        }

        private void HandlecClientDisconnect(ulong clientId)
        {
            throw new NotImplementedException();
        }

       
        
        private void HandheldLobbyPlayersStateChanged(NetworkListEvent<LobbyPlayerState> changeEvent)
        {
            throw new NotImplementedException();
        }
    }
}
