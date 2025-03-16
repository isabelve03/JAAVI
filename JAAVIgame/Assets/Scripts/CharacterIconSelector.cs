using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CharacterIconSelector : MonoBehaviour
{
    public GameObject characterPrefab; // Assign the character prefab in the Inspector
    public Image selectionIndicator;   // Assign a UI Image (e.g., a border or highlight) to show selection

    private void Start()
    {
        // Add a click listener to the button (assuming the icon is a button)
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnIconClicked);
    }

    private void OnIconClicked()
    {
        // Notify the CharacterSelectionManager that this character is selected
        CharacterSelectionManager.Instance.SelectCharacter(characterPrefab);

        // Highlight the selected icon
        HighlightIcon();
    }

    private void HighlightIcon()
    {
        // Disable all selection indicators first
        foreach (var selector in FindObjectsOfType<CharacterIconSelector>())
        {
            if (selector.selectionIndicator != null)
                selector.selectionIndicator.enabled = false;
        }

        // Enable the selection indicator for this icon
        if (selectionIndicator != null)
            selectionIndicator.enabled = true;
    }
}
