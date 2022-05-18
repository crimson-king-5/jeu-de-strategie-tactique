using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System;
public class NetworkManagerLobby : NetworkManager
{
    [Scene] [SerializeField] private string menuScene = string.Empty;

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayer roomPlayerLobby = null;

    public static event Action OnClientConnecter;
    public static event Action OnClientDiscoonected;

    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameHandler>("SpawnablePrefabs").ToList();

    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach (var prefab in spawnablePrefabs)
        {
            ClientScene.RegisterPrefab(prefab);
        }
    }


    public override Void OnClientConnect (networkConnection conn)
    {
        base.OnClientConnect(conn);
        OnClientConnectedCallback?.Invoke();
    }








}
