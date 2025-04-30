using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Managing;

public class OnlineLobbyQuit : MonoBehaviour
{
    public void QuitToMain()
    {
        ClientServerInit _clientServerInit = InstanceFinder.NetworkManager.GetComponent<ClientServerInit>();

        if (InstanceFinder.IsClientStarted)
        {
            _clientServerInit.ChangeClientState();
        }
        if (InstanceFinder.IsServerStarted)
        {
            _clientServerInit.ChangeServerState();
        }
        InstanceFinder.NetworkManager.GetComponent<SteamLobbyManager>().ShutDownSteam();
        Destroy(FindObjectOfType<NetworkManager>().gameObject);

    }
}
