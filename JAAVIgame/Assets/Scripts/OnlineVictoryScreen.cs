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
}
