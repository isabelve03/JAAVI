using FishNet;
using FishNet.Connection;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Managing;
using Steamworks.Data;
using FishNet.Managing.Scened;
using UnityEngine.Timeline;

public class OnlineGameManager : NetworkBehaviour
{
    private int numReady = 0; // tracks number of players ready
    private bool _leftLobbyScene = false; // changes to true when lobby scene is left
    private NetworkManager _networkManager;
    private SceneManager _sceneManager;


    public override void OnStartServer()
    {
        Debug.Log("On Start Server");
        base.OnStartServer();
    }
    public override void OnStartClient()
    {
        Debug.Log("In OnStartClient");
        base.OnStartClient();

        if (!IsClientInitialized)
        {
            Debug.LogWarning("Client not initialized, returning...");
            return;
        }

        _networkManager = FindObjectOfType<NetworkManager>();
        if(_networkManager == null)
        {
            Debug.LogWarning("Could not find Network Manager...");
        }

        _sceneManager = FindObjectOfType<SceneManager>();
        if(_sceneManager == null)
        {
            Debug.LogWarning("Could not find Scene Manager...");
        }

        // subscribe to callbacks
        _sceneManager.OnClientLoadedStartScenes += SceneManager_OnClientLoadedStartScenes;
    }

    private void SceneManager_OnClientLoadedStartScenes(NetworkConnection conn, bool asServer)
    {
        Debug.Log("In start scenes");
        if (!asServer)
            return;

        // in lobby
        if (!_leftLobbyScene)
        {
            numReady++;
            Debug.Log($"Connection: {conn} is player #{numReady}");
        }
    }

    private void OnDestroy()
    {
        // unsubscribe from callbacks
        _sceneManager.OnClientLoadedStartScenes -= SceneManager_OnClientLoadedStartScenes;
    }
}

