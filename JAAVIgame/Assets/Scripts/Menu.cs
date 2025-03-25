using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Battle");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("The Game is Quitting...");
    }

    public void OnlineMode()
    {
        SceneManager.LoadScene("CharacterSelect");
    }

    public void LocalMode()
    {
        GameModeManager.isLocalMode = true; // Remember the mode
        SceneManager.LoadScene("CharacterSelect");
    }
}
