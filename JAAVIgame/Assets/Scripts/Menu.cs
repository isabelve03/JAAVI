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
        GameModeManager.isLocalMode = false; // Ensure that we know we are not going to local mode
        SceneManager.LoadScene("OnlineOptions");
    }

    public void LocalMode()
    {
        GameModeManager.isLocalMode = true; // Remember the mode
        SceneManager.LoadScene("CharacterSelectLocal");
    }

    // loads the start menu scene (i.e. for back button from different modes)
    public void StartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

}
