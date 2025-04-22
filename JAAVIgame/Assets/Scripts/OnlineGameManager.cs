using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet;
using UnityEngine.SceneManagement;
using FishNet.Component.Spawning;
using FishNet.Managing;

public class OnlineGameManager : NetworkBehaviour
{
    public static OnlineGameManager Instance { get; private set; }
    private SteamLobbyManager _steamLobbyManager;
    public NetworkObject Player1 { get; private set; }
    public NetworkObject Player2 { get; private set; }
    private int numPlayers = 0;

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
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

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

        // spawn in players
        _steamLobbyManager.addUserAsClient();
    }

    // client calls this to notify server it is in battle scene and ready to be spawned
    // NOTE: This cannot be an RPC because client is not initiated yet (or else they would be spawned)
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

    }
    private void OnDestroy()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }


}
