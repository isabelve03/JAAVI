using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Managing;

public class OnlineCharacterSelector : NetworkBehaviour
{
    // TODO - Possible reason for not workking to spawn: 
    // this netwrok manager (and where the prefabs are saved to) is found on start client
    // try change this to find network manager within the server RPC to see if this ensure network manager/lobby manager are same for the spawn script
    private NetworkManager _networkManager;
    public override void OnStartClient()
    {
        base.OnStartClient();
        _networkManager = FindObjectOfType<NetworkManager>();
    }

    [ServerRpc(RequireOwnership =false)]
    public void ServerPlayerIsReady(bool isHost, bool ready, NetworkObject player)
    {
        Debug.Log("SERVER");
        _networkManager.GetComponent<LobbyManager>().PlayerReady(isHost, ready, player);
    }

}
