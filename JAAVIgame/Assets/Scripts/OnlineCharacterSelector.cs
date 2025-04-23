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
        _networkManager = FindObjectOfType<NetworkManager>();
    }

    [ServerRpc(RequireOwnership =false)]
    public void ServerPlayerIsReady(bool isHost, bool ready)
    {
        Debug.Log("SERVER");
        _networkManager.GetComponent<LobbyManager>().PlayerReady(isHost, ready);
    }

}
