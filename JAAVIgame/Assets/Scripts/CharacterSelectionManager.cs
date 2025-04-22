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
        if(_networkManager == null)
        {
            Debug.LogError("Could not find Network Manager...");
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

        SetCharacterToSpawn();
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

    private void SetCharacterToSpawn()
    {
        PlayerSpawner _playerSpawner = _networkManager.transform.GetComponent<PlayerSpawner>();
        if(_playerSpawner == null)
        {
            Debug.LogError("Could not find Player Spawner...");
            return;
        }

        if(SelectedNetworkCharacter == null)
        {
            // keep spawned character the defualt
            return;
        }

        _playerSpawner.SetPlayerPrefab(SelectedNetworkCharacter);
    }
}
