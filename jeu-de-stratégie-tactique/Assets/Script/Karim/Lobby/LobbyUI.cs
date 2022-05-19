using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using Unity.Networking.Collections;

public class LobbyUI : MonoBehaviour
{
    
    [Header("References")]
    [SerializeField] private LobbyPlayerCard[] lobbyPlayerCards;
    [SerializeField] private Button startGameButton;

    private NetworkList<LobbyPlayerState> lobbyPlayers = new NetworkList<LobbyPlayerState>();

}
