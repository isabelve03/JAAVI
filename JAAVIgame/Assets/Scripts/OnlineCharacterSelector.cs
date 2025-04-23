using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Managing;

public class OnlineCharacterSelector : NetworkBehaviour
{
    private NetworkManager _networkManager;
    public override void OnStartClient()
    {
        base.OnStartClient();
        _networkManager = GetComponent<NetworkManager>();
    }

    [ServerRpc]
    public void ServerPlayerIsReady(bool isHost, bool ready)
    {
        Debug.Log("SERVER");
        ClientPlayerIsReady(isHost, ready);
    }


    [ObserversRpc]
    private void ClientPlayerIsReady(bool isHost, bool ready)
    {
        Debug.Log("CLIENT");
        _networkManager.GetComponent<LobbyManager>().PlayerReady(isHost, ready);
    }

}
