using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using FishNet.Object;
using FishNet.Component.Transforming;
using FishNet.Component.Animating;
using FishNet.Managing;
using FishNet.Component.Spawning;
using FishNet;
using System.CodeDom;
using Unity.VisualScripting;
using System.Drawing;

public class CharacterSelectionManager : MonoBehaviour
{
    private NetworkManager _networkManager;
    public static CharacterSelectionManager Instance;
    public GameObject SelectedCharacter { get; private set; }
    public NetworkObject SelectedNetworkCharacter { get; private set; }
    private OnlineGameManager _onlineGameManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    private void Start()
    {
        if(!string.Equals(SceneManager.GetActiveScene().name, "CharacterSelect"))
        {
            Destroy(this);
        }

        _networkManager = FindObjectOfType<NetworkManager>();
        if (_networkManager == null)
        {
            Debug.LogError("Could not find Network Manager...");
        }
        if(FindObjectOfType<OnlineManager>() == null)
        {
            Debug.LogWarning("Could not find Online Manager");
        }
        if(FishNet.InstanceFinder.ClientManager == null)
        {
            Debug.Log("Client Manager = null");
        }
        else
        {
            FishNet.InstanceFinder.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;
        }
    }

    private void ClientManager_OnClientConnectionState(FishNet.Transporting.ClientConnectionStateArgs args)
    {
        if(args.ConnectionState == FishNet.Transporting.LocalConnectionState.Started)
        {
            if(OnlineManager.Instance != null)
            {
                _onlineGameManager = OnlineManager.FindObjectOfType<OnlineGameManager>();
                if( _onlineGameManager == null)
                {
                    Debug.Log("Error finding online game manager on OnlineManager...");
                }
                Debug.Log("Successfully found Online Game Manager");
            }
            else
            {
                Debug.Log("Could not find Online Manager...");
            }
        }
        else
        {
            Debug.Log("Local Connection not started");
        }
    }

    public void StartGame()
    {
        if(SelectedNetworkCharacter == null)
        {
            // TODO - Add something on the screen to let the user know to select a character
            Debug.Log("Please select a character");
            return;
        }

        GameObject.Find("UI").transform.Find("Clickables").GetComponent<ScreenManagerNavigator>().StartGame();
    }

    // button is the button which calls this function
    public void SelectCharacter(GameObject button)
    {
        SelectedNetworkCharacter = button.GetComponent<OnlineCharacterIconSelector>().networkCharacterPrefab;
        if(SelectedNetworkCharacter == null)
        {
            Debug.LogError("Error getting selected network character prefab...");
            return;
        }
        Debug.Log("Character selected: " + SelectedNetworkCharacter.name);
    }

    public void SetCharacterToSpawn()
    {
        if(_onlineGameManager == null)
        {
            if(FindObjectOfType<OnlineManager>() == null)
            {
                Debug.LogError("Could not find Online Manager...");
            }
            _onlineGameManager = FindObjectOfType<OnlineManager>().GetComponent<OnlineGameManager>();
            if (_onlineGameManager == null)
            {
                Debug.LogError("Could not find Online Game Manager...");
            }
        }

        int p_index;
        if (InstanceFinder.IsServerStarted)
        {
            p_index = 1;
        }
        else
        {
            p_index = 2;
        }
        _onlineGameManager.ServerRegisterPlayer(SelectedNetworkCharacter, p_index);
    }

    private void OnDestroy()
    {
        FishNet.InstanceFinder.ClientManager.OnClientConnectionState -= ClientManager_OnClientConnectionState;

    }
}
