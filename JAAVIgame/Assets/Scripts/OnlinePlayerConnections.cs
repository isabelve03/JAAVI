using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Managing;
using FishNet.Connection;

public class OnlinePlayerConnections : MonoBehaviour
{
    private NetworkManager _networkManager;

    private void Awake()
    {
        _networkManager = GetComponent<NetworkManager>();
        if(_networkManager == null)
        {
            Debug.Log("Could not find network manager....");
        }

        _networkManager.SceneManager.OnClientLoadedStartScenes += SceneManager_OnClientLoadedStartScenes;
    }

    private void SceneManager_OnClientLoadedStartScenes(NetworkConnection conn, bool asServer)
    {
        Debug.Log($"As server? {asServer}");
        Debug.Log($"Conn: {conn}");

        // NOTE: If we sue this we probably (think about it first) want to return instantly if not server
    }
    private void OnDestroy()
    {
        _networkManager.SceneManager.OnClientLoadedStartScenes += SceneManager_OnClientLoadedStartScenes;
    }
}
