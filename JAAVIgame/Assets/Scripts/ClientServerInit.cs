using FishNet.Managing;
using FishNet.Managing.Server;
using FishNet.Transporting;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ClientServerInit : MonoBehaviour
{
    private NetworkManager _networkManager;

    // Current state of client and server sockets
    private LocalConnectionState _clientState = LocalConnectionState.Stopped;
    private LocalConnectionState _serverState = LocalConnectionState.Stopped;

    private void Start()
    {
        _networkManager = FindObjectOfType<NetworkManager>();  
        if( _networkManager == null)
        {
            UnityEngine.Debug.LogError("Could not find NetworkManager...");
            return;
        }

        // subscribe to these events
        _networkManager.ServerManager.OnServerConnectionState += ServerManager_OnServerConnectionState;
        _networkManager.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;
        

    }

    private void OnDestroy()
    {
        if (_networkManager == null) return;

        // unsubscribe from these events
        _networkManager.ServerManager.OnServerConnectionState -= ServerManager_OnServerConnectionState;
        _networkManager.ClientManager.OnClientConnectionState -= ClientManager_OnClientConnectionState;
    }


    // Update client state when local client state changes
    private void ClientManager_OnClientConnectionState(ClientConnectionStateArgs obj)
    {
        _clientState = obj.ConnectionState;
    }


    // Update server state when local server state changes
    private void ServerManager_OnServerConnectionState(ServerConnectionStateArgs obj)
    {
        _serverState = obj.ConnectionState;
    }


    // Switches server state from connection stopped to connection started and vice versa
    public void ChangeServerState()
    {
        if (_networkManager == null) return;

        if (_serverState != LocalConnectionState.Stopped)
            _networkManager.ServerManager.StopConnection(true);
        else
            _networkManager.ServerManager.StartConnection();
    }

    // Switches client state from connection stopped to connection started and vice versa
    public void ChangeClientState()
    {
        if (_networkManager == null) return;

        if (_clientState != LocalConnectionState.Stopped)
            _networkManager.ClientManager.StopConnection();
        else
            _networkManager.ClientManager.StartConnection();
        UnityEngine.Debug.Log("Changed client state");
    }
}
