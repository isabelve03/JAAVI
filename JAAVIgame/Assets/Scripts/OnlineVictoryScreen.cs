using FishNet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineVictoryScreen : MonoBehaviour
{
    [SerializeField] private GameObject _victoryScreen;
    [SerializeField] private Button _quitButton;

    private void Awake()
    {
        // Initially make sure the victory screen is hidden
        _victoryScreen.SetActive(false);
        _quitButton.gameObject.SetActive(false);
    }

    public void ShowVictoryScreen()
    {
        _victoryScreen.SetActive(true);
        _quitButton.gameObject.SetActive(true); 
    }

    public void QuitGame()
    {
        if (!InstanceFinder.IsClientStarted)
            return;


        OnlineGameManager _onlineGameManager = FindObjectOfType<OnlineGameManager>();
        if(_onlineGameManager == null)
        {
            Debug.LogWarning("Could not get OnlineGameManager");
        }

        _onlineGameManager.s_QuitGame();
    }
}
