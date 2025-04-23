using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Scened;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    private NetworkManager _networkManager;
    private int numReady = 0;
    public NetworkConnection _hostConnection { get; private set; }
    public NetworkConnection _clientConnection {  get; private set; }
    public NetworkObject _hostCharacter {  get; private set; }
    public NetworkObject _clientCharacter {  get; private set; }

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

        numReady++;
        if(numReady == 1)
        {
            _hostConnection = conn;
            Debug.Log("Host connection set");
        }else if(numReady == 2)
        {
            _clientConnection = conn;
            Debug.Log("Client connection set");
            numReady = 0;
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

    public void PlayerReady(bool isHost, bool ready)
    {
        // ready will be false on first ready up, but true for all after
        if (!ready)
            numReady++;

        if (isHost)
        {
            _hostCharacter = GetComponent<CharacterSelectionManager>().SelectedNetworkCharacter;
        }
        else
        {
            _clientCharacter = GetComponent<CharacterSelectionManager>().SelectedNetworkCharacter;
        }

        if(numReady == 2)
        {
            Debug.Log($"Host Character: {_hostCharacter.name}");
            Debug.Log($"Client Character: {_clientCharacter.name}");
        }
    }

    private void OnDestroy()
    {
        
        // unsubscribe from callbacks
        _networkManager.SceneManager.OnClientLoadedStartScenes -= SceneManager_OnClientLoadedScenes;
    }
}
