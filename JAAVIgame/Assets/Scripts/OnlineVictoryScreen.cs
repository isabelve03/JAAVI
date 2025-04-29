using FishNet;
using FishNet.Managing;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
        ClientServerInit _clientServerInit = InstanceFinder.NetworkManager.GetComponent<ClientServerInit>();

        if (InstanceFinder.IsClientStarted)
        {
            _clientServerInit.ChangeClientState();
        }
        if (InstanceFinder.IsServerStarted)
        {
            _clientServerInit.ChangeServerState();
        }
        InstanceFinder.NetworkManager.GetComponent<SteamLobbyManager>().ShutDownSteam();
        Destroy(FindObjectOfType<NetworkManager>());
        Thread.Sleep(100);
    }
}
