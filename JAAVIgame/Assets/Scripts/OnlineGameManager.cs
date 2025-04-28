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
    [SerializeField] private NetworkObject _onlineDeathBarrier;
    private NetworkManager _networkManager;
    private OnlinePlayerSpawner _playerSpawner;
    private LobbyManager _lobbyManager;

    public NetworkObject _hostCharacter;
    public NetworkObject _clientCharacter;
    public NetworkConnection _hostConn;
    public NetworkConnection _clientConn;
    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        ServerSpawnCharacters();
        ServerSpawnDeathBarrier();
    }
    [ServerRpc]
    private void ServerSpawnCharacters()
    {
        _networkManager = FindAnyObjectByType<NetworkManager>();
        _playerSpawner = _networkManager.GetComponent<OnlinePlayerSpawner>();
        _lobbyManager = _networkManager.GetComponent<LobbyManager>();
        Debug.Log("SERVER: Spawning characters");
        _hostCharacter = _lobbyManager._hostCharacter;
        _clientCharacter = _lobbyManager._clientCharacter;
        _hostConn = _lobbyManager._hostConnection;
        _clientConn = _lobbyManager._clientConnection;

        _playerSpawner.Spawn(_hostCharacter, _hostConn);
        _playerSpawner.Spawn(_clientCharacter, _clientConn);
    }

    [ServerRpc]
    private void ServerSpawnDeathBarrier()
    {
        NetworkObject db = Instantiate(_onlineDeathBarrier);
        InstanceFinder.ServerManager.Spawn(db);
    }
    

    [ServerRpc]
    public void s_Accessed()
    {
        Debug.Log("SERVER: Accessed");
        c_Accessed();
    }
    [ObserversRpc]
    public void c_Accessed()
    {
        Debug.Log("CLIENT: Accessed");
    }
}

