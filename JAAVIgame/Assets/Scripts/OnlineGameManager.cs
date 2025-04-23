using FishNet;
using FishNet.Connection;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Managing;
using Steamworks.Data;
using FishNet.Managing.Scened;

public class OnlineGameManager : NetworkBehaviour
{
    public NetworkObject Player1 { get; private set; }
    public NetworkObject Player2 { get; private set; }
    private NetworkConnection _playerConnection1; // host client
    private NetworkConnection _playerConnection2; // regular client
    private int numReady = 0; // num people clicked start/ready button
    private int numConns = 0;
    private NetworkManager _networkManager;
    private SceneManager _sceneManager;
    private CharacterSelectionManager _characterSelectionManager;

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
        if(_networkManager == null )
        {
            Debug.LogError("Could not find network manager...");
        }

        _characterSelectionManager = FindObjectOfType<CharacterSelectionManager>();
        if(_characterSelectionManager == null)
        {
            Debug.LogError("Could not find character selection manager...");
        }

        _sceneManager = _networkManager.SceneManager;
        if( _sceneManager == null)
        {
            Debug.LogError("Could not find scene manager");
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!IsClientInitialized)
            return;

        ServerPlayerRegister(_characterSelectionManager.SelectedNetworkCharacter, InstanceFinder.IsServerStarted);
    }

    [ServerRpc]
    public void ServerReadyToStartGame(bool clicked)
    {
        // clicked will be false if 1st time clicking, else will be true
        if (!clicked)
        {
            // increment if first time clicking ready/start
            numReady++;
        }

        if (numReady == 2)
            LoadBattleScene();
    }

    [ServerRpc]
    private void LoadBattleScene()
    {
        SceneLoadData sld = new SceneLoadData("TEST_ONLINE_BATTLE");
        sld.Options.AllowStacking = false; // replaces the existing scene
        _sceneManager.LoadGlobalScenes(sld);
    }
    [ServerRpc (RequireOwnership = false)]
    public void ServerPlayerRegister(NetworkObject player, bool isHost)
    {
        Debug.Log("SERVER: In ServerPlayerRegister rpc");
        if (isHost)
        {
            Player1 = player;
            numPlayers++;
            _playerConnection1 = _networkManager.GetComponent<OnlinePlayerConnections>()._connectionHost;
            Debug.Log($"Registered player 1 (host) as {Player1.name}");
            Debug.Log($"Registered player 1's connection (host) as {_playerConnection1}");
        }
        else
        {
            Player2 = player;
            numPlayers++;
            _playerConnection2 = _networkManager.GetComponent<OnlinePlayerConnections>()._connectionClient;
            Debug.Log($"Registered player 2 (client) as {Player2.name}");
            Debug.Log($"Registered player 2's connection (client) as {_playerConnection2}");

        }
    }
}
