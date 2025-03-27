using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ScreenManagerNavigator : MonoBehaviour
{
    public void StartGame()
    {
        if (GameModeManager.isLocalMode)
        {
            SceneManager.LoadScene("LocalMode"); // Load local mode
        }
        else
        {
            SceneManager.LoadScene("OnlineOptions"); // Load online mode
        }
    }

}
