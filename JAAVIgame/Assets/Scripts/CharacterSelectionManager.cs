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
using FishNet.Managing.Server;

public class CharacterSelectionManager : NetworkBehaviour
{
    private NetworkManager _networkManager;
    private LobbyManager _lobbyManager;
    public static CharacterSelectionManager Instance;
    public GameObject SelectedCharacter { get; private set; }
    public NetworkObject SelectedNetworkCharacter { get; private set; }
    private bool ready = false; // tracks if player has clicked start/ready (and is valid)


    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("On start client");
    }
    private void Start()
    {
        _networkManager = FindObjectOfType<NetworkManager>();
        if (_networkManager == null)
        {
            Debug.LogError("Could not find Network Manager...");
        }

        _lobbyManager = _networkManager.GetComponent<LobbyManager>();
        if (_lobbyManager == null)
        {
            Debug.LogError("Could not find Lobby Manager");
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
        bool isHost = InstanceFinder.IsServerStarted;
        ServerPlayerReady(isHost, ready);
        _lobbyManager.PlayerReady(isHost, ready);
        ready = true;
    }

    [ServerRpc]
    private void ServerPlayerReady(bool isHost, bool ready)
    {
        Debug.Log($"SERVER: isHost: {isHost}, first time? {!ready}");
    }

    [ObserversRpc]
    private void ClientPlayerReady(bool isHost, bool ready)
    {

        Debug.Log($"CLIENT: isHost: {isHost}, first time? {!ready}");
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

}
