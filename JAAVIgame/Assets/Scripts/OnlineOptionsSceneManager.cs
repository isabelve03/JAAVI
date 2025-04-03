using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnlineOptionsSceneManager : MonoBehaviour
{
    private GameObject steamConnectingPanel;
    private GameObject mainMenuPanel;
    private GameObject lobbyConnectingPanel;
    private SteamLobbyManager _steamLobbyManager;

    // Start is called before the first frame update
    void Start()
    {
        _steamLobbyManager = GetComponent<SteamLobbyManager>();

        if(_steamLobbyManager == null)
        {
            Debug.Log("Could not get steam lobby manager... ");
            return;
        }

        GameObject uiRoot = GameObject.Find("UI");
        if(uiRoot == null)
        {
            Debug.Log("UI GameObject could not be found...");
            return;
        }

        steamConnectingPanel = uiRoot.transform.Find("SteamConnecting").gameObject;
        mainMenuPanel = uiRoot.transform.Find("MainMenu").gameObject;
        lobbyConnectingPanel = uiRoot.transform.Find("LobbyConnecting").gameObject;

        if (steamConnectingPanel == null || mainMenuPanel == null || lobbyConnectingPanel == null)
        {
            Debug.Log("Could not find one or more UI panels");
            return;
        }
        initializeOnlineOptionsScene();
    }

    private void initializeOnlineOptionsScene()
    {
        showSteamConnectingPanel();
        StartCoroutine(CheckSteamConnection());
    }
    private void showSteamConnectingPanel()
    {
        steamConnectingPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        lobbyConnectingPanel.SetActive(false);
    }

    private void showMainMenu()
    {
        steamConnectingPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        lobbyConnectingPanel.SetActive(false);
    }

    private void showLobbyConnecting()
    {
        steamConnectingPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        lobbyConnectingPanel.SetActive(true);
    }

    public void JoinGameButtonPressed()
    {
        showLobbyConnecting();
        StartCoroutine(CheckLobbyConnection());
    }

    private IEnumerator CheckSteamConnection()
    {
        while (!_steamLobbyManager.GetConnectionStatus())
        {
            Debug.Log("In while loop");
            yield return new WaitForSeconds(0.5f);
        }

        showMainMenu();
    }
    private IEnumerator CheckLobbyConnection()
    {
        while (!_steamLobbyManager.GetLobbyStatus())
        {
            yield return new WaitForSeconds(1.0f);
        }
            SceneManager.LoadScene("TEST_ONLINE_BATTLE"); // Load online mode
    }


}
