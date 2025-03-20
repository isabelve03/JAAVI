using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartGameButton : MonoBehaviour
{
    public void StartGame()
    {
        if (CharacterSelectionManager.Instance.SelectedCharacter != null)
        {
            SceneManager.LoadScene("Battle"); // Replace with your game scene name
        }
        else
        {
            Debug.LogWarning("No character selected!");
        }
    }
}


