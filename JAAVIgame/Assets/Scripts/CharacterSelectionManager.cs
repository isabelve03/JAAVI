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
    private bool ready = false; // tracks if player has clicked start/ready (and is valid)

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
            Debug.LogError("Could not find OnlineManager");
            return;
        }
        _onlineGameManager = FindObjectOfType<OnlineManager>().GetComponent<OnlineGameManager>();
    }

    public void StartGame()
    {
        if(SelectedNetworkCharacter == null)
        {
            // TODO - Add something on the screen to let the user know to select a character
            Debug.Log("Please select a character");
            return;
        }

        //_onlineGameManager.ServerReadyToStartGame(ready);
        ready = true;
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
