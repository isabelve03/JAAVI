using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Managing;
using FishNet.Connection;
using System;

public class OnlinePlayerConnections : MonoBehaviour
{
    private NetworkManager _networkManager;
    public NetworkConnection _connectionHost { get; private set; }
    public NetworkConnection _connectionClient { get; private set; }
    public int _connCnt { get; private set; }

    private void Awake()
    {
        _networkManager = GetComponent<NetworkManager>();
        if(_networkManager == null)
        {
            Debug.Log("Could not find network manager....");
        }
        _connCnt = 0;
        _networkManager.SceneManager.OnClientLoadedStartScenes += SceneManager_OnClientLoadedStartScenes;
    }

    // when clients loads in this callback stores their network connection. Firt client is host, 2nd is reg client
    private void SceneManager_OnClientLoadedStartScenes(NetworkConnection conn, bool asServer)
    {
        if (!asServer)
            return;

        _connCnt++;
        if(_connCnt == 1)
        {
            _connectionHost = conn;
        }else if(_connCnt == 2)
        {
            _connectionClient = conn;
        }
        else
        {
            Debug.LogWarning("WTFFF", this);
        }
    }
    private void OnDestroy()
    {
        _networkManager.SceneManager.OnClientLoadedStartScenes += SceneManager_OnClientLoadedStartScenes;
    }
}
