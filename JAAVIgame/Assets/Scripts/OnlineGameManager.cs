using FishNet;
using FishNet.Connection;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Managing;
using Steamworks.Data;

public class OnlineGameManager : NetworkBehaviour
{
    public NetworkObject Player1 { get; private set; }
    public NetworkObject Player2 { get; private set; }
    private NetworkConnection _playerConnection1;
    private NetworkConnection _playerConnection2;
    private int numPlayers = 0;
    private int numConns = 0;
    private NetworkManager _networkManager;

    private void Awake()
    {
        if (InstanceFinder.IsServerStarted)
        {
            Debug.Log("Host");
        }
        else
        {
            Debug.Log("client");
        }
        _networkManager = FindObjectOfType<NetworkManager>();
        _networkManager.SceneManager.OnClientLoadedStartScenes += SceneManager_OnClientLoadedStartScenes;
    }

    private void SceneManager_OnClientLoadedStartScenes(NetworkConnection conn, bool asServer)
    {

    }

    private void OnDestroy()
    {
        _networkManager.SceneManager.OnClientLoadedStartScenes -= SceneManager_OnClientLoadedStartScenes;
    }
}
