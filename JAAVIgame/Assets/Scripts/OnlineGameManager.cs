using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet;
using UnityEngine.SceneManagement;
using FishNet.Component.Spawning;
using FishNet.Managing;
using FishNet.Connection;

public class OnlineGameManager : NetworkBehaviour
{
    public static OnlineGameManager Instance { get; private set; }
    private SteamLobbyManager _steamLobbyManager;
    private NetworkManager _networkManager;
    public NetworkObject Player1 { get; private set; }
    public NetworkObject Player2 { get; private set; }
    private NetworkConnection _playerConnection1;
    private NetworkConnection _playerConnection2;
    private int numPlayers = 0;
    private int numConns = 0;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        _steamLobbyManager = FindObjectOfType<NetworkManager>().GetComponent<SteamLobbyManager>();
        if(_steamLobbyManager == null)
        {
            Debug.LogError("Could not find steam lobby manager...");
            return;
        }
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

        _networkManager = FindObjectOfType<NetworkManager>();
        if(_networkManager == null)
        {
            Debug.LogError("Could not find network manager...");
            return;
        }
        _networkManager.SceneManager.OnClientLoadedStartScenes += SceneManager_OnClientLoadedStartScenes;
    }

    // DELETE WHEN FINISHED
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "TEST_ONLINE_BATTLE")
        {
            NetworkObject player = FindObjectOfType<CharacterSelectionManager>().SelectedNetworkCharacter;
            if(player == null)
            {
                Debug.LogError("Could not get player...");
                return;
            }
            if (InstanceFinder.IsServerStarted)
            {
                // is host player p_index = 1
                Debug.Log("Going to register player host");
                ServerRegisterPlayer(player, 1);
                Debug.Log("After");
            }
            else
            {
                // is only client p_index = 2
                Debug.Log("Going to register player client");
                ServerRegisterPlayer(player, 2);
            }
        }

    }
    private void SceneManager_OnClientLoadedStartScenes(NetworkConnection conn, bool asServer)
    {
        if (!asServer) return;

        numConns++;
        if (numConns == 1)
        {
            _playerConnection1 = conn;
        }
        else if (numConns == 2)
        {
            _playerConnection2 = conn;
        }
        else
        {
            Debug.LogError("WTFFF");
        }
            
    }
    // client calls this to notify server it is in battle scene and ready to be spawned
    // NOTE: This cannot be an RPC because client is not initiated yet (or else they would be spawned)
    [ServerRpc]
    public void ServerRegisterPlayer(NetworkObject player, int playerIndex)
    {
        if (!InstanceFinder.IsServerStarted) return; // Only allow the server to register players

        if (playerIndex == 1)
        {
            Player1 = player;
            numPlayers++; 
            Debug.Log("Registerd Player 1");
        }
        else
        {
            Player2 = player;
            numPlayers++; 
            Debug.Log("Registered player 2");
        }
        Debug.Log($"With Character: {player.name}");
    }

    private void OnDestroy()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        if (_networkManager != null)
            _networkManager.SceneManager.OnClientLoadedStartScenes -= SceneManager_OnClientLoadedStartScenes;
    }


}
