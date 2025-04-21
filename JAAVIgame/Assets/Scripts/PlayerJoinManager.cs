using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerJoinManager : MonoBehaviour
{

    [Header("Player Setup")]
    public static PlayerJoinManager Instance;
    public GameObject playerCursorPrefab; //assign in inspector
    public Transform cursorParent; // UI canvas parent for cursors
    public int maxPlayers = 6;

    [Header("UI")]
    public Transform playerListContainer; // UI container for player names
    public GameObject playerNamePrefab; // UI Text Prefab for player names
    public Text joinPromptText; // Legacy text prompt UI

    [HideInInspector]
    public List<PlayerData> joinedPlayers = new List<PlayerData>();
    private List<int> assignedControllerIDs = new List<int>();
    private bool keyboardPlayerJoined = false; // Track if keyboard player is joined

    //public GameObject playerPrefab; // Assign your player prefab in Unity
    //private List<int> assignedControllers = new List<int>(); // Track controllers that have joined
    //private List<GameObject> players = new List<GameObject>(); // Track spawned players

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Hide the default mouse pointer
        Cursor.visible = false;
    }

    void Update()
    {
        // Keyboard player joins first
        if (!keyboardPlayerJoined && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Keyboard player joining");
            AddPlayer(0); // 0 represents keyboard
            keyboardPlayerJoined = true;
        }

        // Controllers join with button press
        for (int i = 1; i <= 4; i++) // Supports up to 4 controllers
        {
            if (assignedControllerIDs.Contains(i))
                continue; // Skip if already assigned

            // Check if joystick button 0 (A button) on the controller is pressed
            if (Input.GetKeyDown("joystick " + i + " button 0"))
            {
                Debug.Log("Controller " + i + " joining.");
                AddPlayer(i);
            }
        }

        // Hide join prompt when 4 players join
        if (joinedPlayers.Count >= 4)
        {
            joinPromptText.gameObject.SetActive(false); // Hide the prompt
        }
    }

    void AddPlayer(int controllerID)
    {
        // returns after reaching max joined players which is 6 as default
        if(joinedPlayers.Count >= maxPlayers) return;

        // Only add the player if the controller is not already used
        if (assignedControllerIDs.Contains(controllerID))
        {
            Debug.Log("Controller " + controllerID + " already assigned!");
            return;
        }

        // creates new cursor for added player
        GameObject cursor = Instantiate(playerCursorPrefab, cursorParent);
        cursor.GetComponent<PlayerCursor>().SetControllerID(controllerID);

        PlayerData data = new PlayerData
        {
            controllerID = controllerID,
            cursor = cursor,
            selectedCharacter = null
        };

        assignedControllerIDs.Add(controllerID); // Add controller to the list
        joinedPlayers.Add(data); // Add player to the list

        // Update UI
        AddPlayerToUI(controllerID);
    }

    void AddPlayerToUI(int controllerID)
    {
        // Create the player name UI
        GameObject newName = Instantiate(playerNamePrefab, playerListContainer);
        string playerType = (controllerID == 0) ? "Keyboard" : "Controller " + controllerID;
        newName.GetComponent<Text>().text = "Player " + joinedPlayers.Count;
    }

    // Called by CharacterIconSelector
    public void SetSelectedCharacter(int controllerID, GameObject character)
    {
        foreach (var player in joinedPlayers)
        {
            if (player.controllerID == controllerID)
            {
                player.selectedCharacter = character;
                Debug.Log(player.selectedCharacter + " selected for Player " + controllerID);
                return;
            }
        }
        Debug.LogWarning("No player found for Controller ID: " + controllerID);
    }

    void HandleCharacterSelection(int controllerID, GameObject characterPrefab)
    {
        // Update the selected character based on the controller ID
        if (controllerID >= 0 && controllerID < joinedPlayers.Count)
        {
            joinedPlayers[controllerID].selectedCharacter = characterPrefab;
            Debug.Log("Player " + controllerID + " selected: " + characterPrefab.name);
        }
    }


    public GameObject GetSelectedCharacter(int controllerID)
    {
        foreach (PlayerData player in joinedPlayers)
        {
            if (player.controllerID == controllerID)
                return player.selectedCharacter;
        }
        return null;
    }
}


