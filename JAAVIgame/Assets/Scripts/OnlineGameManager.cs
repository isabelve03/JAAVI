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
    private NetworkManager _networkManager;
    private OnlinePlayerSpawner _playerSpawner;
    private LobbyManager _lobbyManager;
    public override void OnStartClient()
    {
        base.OnStartClient();
        ServerSpawnCharacters();
    }

    [ServerRpc]
    private void ServerSpawnCharacters()
    {
        _networkManager = FindAnyObjectByType<NetworkManager>();
        _playerSpawner = _networkManager.GetComponent<OnlinePlayerSpawner>();
        _lobbyManager = _networkManager.GetComponent<LobbyManager>();
        Debug.Log("SERVER: Spawning characters");
        NetworkObject hostCharacter = _lobbyManager._hostCharacter;
        NetworkObject clientCharacter = _lobbyManager._clientCharacter;
        NetworkConnection hostConn = _lobbyManager._hostConnection;
        NetworkConnection clientConn = _lobbyManager._clientConnection;

        _playerSpawner.Spawn(hostCharacter, hostConn);
        _playerSpawner.Spawn(clientCharacter, clientConn);
    }

    [ServerRpc]
    public void ServerAccessed()
    {
        Debug.Log("SERVER: Accessed");
        ClientAccessed();
    }
    [ObserversRpc]
    public void ClientAccessed()
    {
        Debug.Log("CLIENT: Accessed");
    }
}

