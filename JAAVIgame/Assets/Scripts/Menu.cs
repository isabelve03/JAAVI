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

        /*
        // following code fixes bug where entering online, quitting, and re-entering causes all options to be displayed instead of needed one
        GameObject _networkManager = GameObject.Find("NetworkManager");
        if(_networkManager == null)
        {
            // first entry into online mode (i.e. no network manager yet)
            return;
        }
        OnlineOptionsSceneManager _onlineOptionsSceneManager = _networkManager.GetComponent<OnlineOptionsSceneManager>();
        if(_onlineOptionsSceneManager == null)
        {
            return;
        }
        _onlineOptionsSceneManager.initializeOnlineOptionsScene();
        */
    }

    public void LocalMode()
    {
        GameModeManager.isLocalMode = true; // Remember the mode
        SceneManager.LoadScene("CharacterSelect");
    }

    // loads the start menu scene (i.e. for back button from different modes)
    public void StartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

}
