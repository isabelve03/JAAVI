using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Scened;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LobbyManager : MonoBehaviour
{
    private NetworkManager _networkManager;
    private int numReady = 0;
    public NetworkConnection _hostConnection { get; private set; }
    public NetworkConnection _clientConnection {  get; private set; }
    public NetworkObject _hostCharacter {  get; private set; }
    public NetworkObject _clientCharacter {  get; private set; }
    [SerializeField] NetworkObject _OnlineCharacterSelector;
    [SerializeField] NetworkObject _OnlineLobbyHelper;
    [SerializeField] NetworkObject _OnlineGameManager;

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
        sld.ReplaceScenes = ReplaceOption.All;
        _networkManager.SceneManager.LoadGlobalScenes(sld);

        bool asServer = false;
        NetworkConnection conn = _clientConnection;
        if (InstanceFinder.IsServerStarted)
        {
            asServer = true;
            conn = _hostConnection;
        }
        NetworkObject nob = _networkManager.GetPooledInstantiated(_OnlineCharacterSelector, asServer);
        _networkManager.ServerManager.Spawn(nob, conn);

    }


    public void PlayerReady(bool isHost, bool ready, NetworkObject player)
    {
        // ready will be false on first ready up, but true for all after
        if (!ready)
            numReady++;

        if (isHost)
        {
            _hostCharacter = player;
        }
        else
        {
            _clientCharacter = player;
        }

        if(numReady == 2)
        {
            Debug.Log($"Host Character: {_hostCharacter.name}");
            Debug.Log($"Client Character: {_clientCharacter.name}");
            LoadBattleScene();
        }
    }
    private void LoadBattleScene()
    {
        SceneLoadData sld = new SceneLoadData("TEST_ONLINE_BATTLE");
        sld.ReplaceScenes = ReplaceOption.All;
        _networkManager.SceneManager.LoadGlobalScenes(sld);

        bool asServer = false;
        NetworkConnection conn = _clientConnection;
        if (InstanceFinder.IsServerStarted)
        {
            asServer = true;
            conn = _hostConnection;
        }
        NetworkObject nob = _networkManager.GetPooledInstantiated(_OnlineGameManager, asServer);
        _networkManager.ServerManager.Spawn(nob, conn);
    }

    private void OnDestroy()
    {
        
        // unsubscribe from callbacks
        _networkManager.SceneManager.OnClientLoadedStartScenes -= SceneManager_OnClientLoadedScenes;
    }
}
