using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnlineOptionsSceneManager : MonoBehaviour
{
    public static OnlineOptionsSceneManager Instance { get; private set; }
    private GameObject steamConnectingPanel;
    private GameObject mainMenuPanel;
    private GameObject lobbyConnectingPanel;
    private SteamLobbyManager _steamLobbyManager;
    private Scene scene;
    public Action<bool> LobbyJoined;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // remove just in case one exists already (fine to do if none exist)
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        scene = SceneManager.GetActiveScene(); // store which scene this was spawned in (should be OnlineOptions)
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    // Start is called before the first frame update
    void Start()
    {
        _steamLobbyManager = GetComponent<SteamLobbyManager>();

        if(_steamLobbyManager == null)
        {
            Debug.Log("Could not get steam lobby manager... ");
            return;
        }
        GetUIElements();
        initializeOnlineOptionsScene();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (string.Equals(scene.path, this.scene.path)) return; // did not load into OnlineOptions

        GetUIElements();
        initializeOnlineOptionsScene();
    }

    public void initializeOnlineOptionsScene()
    {
        showSteamConnectingPanel();
        StartCoroutine(CheckSteamConnection());
    }

    private void GetUIElements()
    {
        GameObject uiRoot = GameObject.Find("UI");
        if(uiRoot == null)
        {
            Debug.Log("UI GameObject could not be found...", this);
            return;
        }


        if (uiRoot.transform.Find("SteamConnecting") == null || uiRoot.transform.Find("MainMenu") == null || uiRoot.transform.Find("LobbyConnecting") == null)
        {
            Debug.Log("Could not find one or more UI panels");
            return;
        }
        steamConnectingPanel = uiRoot.transform.Find("SteamConnecting").gameObject;
        mainMenuPanel = uiRoot.transform.Find("MainMenu").gameObject;
        lobbyConnectingPanel = uiRoot.transform.Find("LobbyConnecting").gameObject;
    }
    private void showSteamConnectingPanel()
    {
        if (steamConnectingPanel == null || mainMenuPanel == null || lobbyConnectingPanel == null) return;
        steamConnectingPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        lobbyConnectingPanel.SetActive(false);
    }

    private void showMainMenu()
    {
        if (steamConnectingPanel == null || mainMenuPanel == null || lobbyConnectingPanel == null) return;
        steamConnectingPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        lobbyConnectingPanel.SetActive(false);
    }

    private void showLobbyConnecting()
    {
        if (steamConnectingPanel == null || mainMenuPanel == null || lobbyConnectingPanel == null) return;
        steamConnectingPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        lobbyConnectingPanel.SetActive(true);
    }

    public void JoinGameButtonPressed()
    {
        showLobbyConnecting();
        //StartCoroutine(CheckLobbyConnection());
    }

    private IEnumerator CheckSteamConnection()
    {
        while (!_steamLobbyManager.GetConnectionStatus())
        {
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
        //LobbyJoined.Invoke(true);
    }


}
