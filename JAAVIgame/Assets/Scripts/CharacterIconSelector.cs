using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CharacterIconSelector : MonoBehaviour
{
    public GameObject characterPrefab;
    public Image selectionIndicator;

    public void SelectCharacter(int controllerID)
    {
        Debug.Log("Character selected by Controller " + controllerID);
        PlayerJoinManager.Instance.SetSelectedCharacter(controllerID, characterPrefab);
        HighlightIcon();
    }

    private void HighlightIcon()
    {
        foreach (var selector in FindObjectsOfType<CharacterIconSelector>())
        {
            if (selector.selectionIndicator != null)
                selector.selectionIndicator.enabled = false;
        }

        if (selectionIndicator != null)
            selectionIndicator.enabled = true;
    }
}
