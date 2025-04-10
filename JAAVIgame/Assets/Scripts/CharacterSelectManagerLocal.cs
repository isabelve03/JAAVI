using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectManagerLocal : MonoBehaviour
{
 public static CharacterSelectManagerLocal Instance;

    // Track each controller's selected character
    private Dictionary<int, GameObject> controllerSelections = new Dictionary<int, GameObject>();

    public GameObject[] availableCharacters; // Assign your available character prefabs (e.g., Knight, Necromancer)

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

    // Select character for a given controller
    public void SelectCharacter(int controllerId, GameObject character)
    {
        controllerSelections[controllerId] = character;
        Debug.Log("Player " + controllerId + " selected: " + character.name);
    }

    // Get the character selected by a controller
    public GameObject GetSelectedCharacter(int controllerId)
    {
        if (controllerSelections.ContainsKey(controllerId))
        {
            return controllerSelections[controllerId];
        }
        return null;
    }

    // Set available characters (to assign the correct ones in PlayerManager)
    public void SetAvailableCharacters(GameObject[] characters)
    {
        availableCharacters = characters;
    }
}