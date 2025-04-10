using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab; // Assign your player prefab in Unity
    public Transform playerListContainer; // UI container for player names
    public GameObject playerNamePrefab; // UI Text Prefab for player names
    public Text joinPromptText; // Legacy text prompt UI

    private List<int> assignedControllers = new List<int>(); // Track controllers that have joined
    private List<GameObject> players = new List<GameObject>(); // Track spawned players
    private bool keyboardPlayerJoined = false; // Track if keyboard player is joined

    private void Update()
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
            if (assignedControllers.Contains(i))
                continue; // Skip if already assigned

            // Check if joystick button 0 (A button) on the controller is pressed
            if (Input.GetKeyDown("joystick " + i + " button 0"))
            {
                Debug.Log("Controller " + i + " pressed button 0 (A/X). Spawning player.");
                AddPlayer(i);
            }
        }

        // Hide join prompt when 4 players join
        if (players.Count >= 4)
        {
            joinPromptText.gameObject.SetActive(false); // Hide the prompt
        }
    }

    void AddPlayer(int controllerID)
{
    // Only add the player if the controller is not already used
    if (assignedControllers.Contains(controllerID))
    {
        Debug.Log("Controller " + controllerID + " already assigned!");
        return;
    }

    Vector3 spawnPosition = new Vector3(players.Count * 2, 0, 0);
    GameObject newPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

    // Set the controllerID for the new player
    PlayerMovement playerMovement = newPlayer.GetComponent<PlayerMovement>();
    if (playerMovement != null)
    {
        playerMovement.SetControllerID(controllerID); // Correctly set the controller ID
    }
    else
    {
        Debug.LogError("PlayerMovement script not found on player prefab!");
    }

    assignedControllers.Add(controllerID); // Add controller to the list
    players.Add(newPlayer); // Add player to the list

    // Update UI
    AddPlayerToUI(controllerID);
}

    void AddPlayerToUI(int controllerID)
    {
        // Create the player name UI
        GameObject newName = Instantiate(playerNamePrefab, playerListContainer);
        string playerType = (controllerID == 0) ? "Keyboard" : "Controller " + controllerID;
        newName.GetComponent<Text>().text = "Player " + players.Count;
    }
}


