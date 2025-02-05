using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(2);
    }

    public void OnlineMode()
    {
        SceneManager.LoadScene(0);
    }

    public void CharacterSelect()
    {
        SceneManager.LoadScene(3);
    }
}
