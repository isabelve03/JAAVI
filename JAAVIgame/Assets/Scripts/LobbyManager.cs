using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Scened;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    private NetworkManager _networkManager;
    private int numConns = 0;
    public NetworkConnection _hostConnection { get; private set; }
    public NetworkConnection _clientConnection {  get; private set; }

    private void Awake()
    {
        _networkManager = GetComponent<NetworkManager>();

        // subscribe to callbacks
        _networkManager.SceneManager.OnClientLoadedStartScenes += SceneManager_OnClientLoadedScenes;
    }

    private void SceneManager_OnClientLoadedScenes(NetworkConnection conn, bool asServer)
    {
        if (!asServer)
            return;

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "OnlineOptions")
            return;

        numConns++;
        if(numConns == 1)
        {
            _hostConnection = conn;
            Debug.Log("Host connection set");
        }else if(numConns == 2)
        {
            _clientConnection = conn;
            Debug.Log("Client connection set");
            numConns = 0;
            LoadCharacterSelect();
        }
        else
        {
            Debug.LogError("WTF");
        }

    }

    private void LoadCharacterSelect()
    {
        SceneLoadData sld = new SceneLoadData("CharacterSelect");
        sld.Options.AllowStacking = false;
        _networkManager.SceneManager.LoadGlobalScenes(sld);

        SceneUnloadData sud = new SceneUnloadData("OnlineOptions");
        _networkManager.SceneManager.UnloadGlobalScenes(sud);
    }

    private void OnDestroy()
    {
        
        // unsubscribe from callbacks
        _networkManager.SceneManager.OnClientLoadedStartScenes -= SceneManager_OnClientLoadedScenes;
    }
}
