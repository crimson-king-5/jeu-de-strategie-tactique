using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class U_NET_API : MonoBehaviour
{
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10,10,300,300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartLogin();
        }
        else
        {
            SatutsLabels();
        }
        GUILayout.EndArea();
    }

    static void StartLogin()
    {
        if (GUILayout.Button("Host"))NetworkManager.Singleton.StartHost();
        if (GUILayout.Button("Client"))NetworkManager.Singleton.StartClient();
        if (GUILayout.Button("Server"))NetworkManager.Singleton.StartServer();
    }

    static void SatutsLabels()
    {
        string mode = NetworkManager.Singleton.IsHost ? "host" :
            NetworkManager.Singleton.IsServer ? " Sever" : "Client";
        GUILayout.Label("Transport:" + NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode:" + mode);
    }
}
