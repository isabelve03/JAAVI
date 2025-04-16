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
        Debug.Log("In initializeOnlineOptionsScene()");
    }

    private void GetUIElements()
    {
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
    }
    private void showSteamConnectingPanel()
    {
        if (steamConnectingPanel == null || mainMenuPanel == null || lobbyConnectingPanel == null) return;
        steamConnectingPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        lobbyConnectingPanel.SetActive(false);
        Debug.Log("In showSteamConnectingPanel()");
    }

    private void showMainMenu()
    {
        if (steamConnectingPanel == null || mainMenuPanel == null || lobbyConnectingPanel == null) return;
        steamConnectingPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        lobbyConnectingPanel.SetActive(false);
        Debug.Log("In showMainMenu()");
    }

    private void showLobbyConnecting()
    {
        if (steamConnectingPanel == null || mainMenuPanel == null || lobbyConnectingPanel == null) return;
        steamConnectingPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        lobbyConnectingPanel.SetActive(true);
        Debug.Log("In showLobbyConnecting()");
    }

    public void JoinGameButtonPressed()
    {
        showLobbyConnecting();
        StartCoroutine(CheckLobbyConnection());
        Debug.Log("In JoinGameButtonPressed()");
    }

    private IEnumerator CheckSteamConnection()
    {
        while (!_steamLobbyManager.GetConnectionStatus())
        {
            Debug.Log("In while loop");
            yield return new WaitForSeconds(0.5f);
        }

        showMainMenu();
        Debug.Log("In CheckSteamConnection()");
    }
    private IEnumerator CheckLobbyConnection()
    {
        while (!_steamLobbyManager.GetLobbyStatus())
        {
            yield return new WaitForSeconds(1.0f);
        }
        SceneManager.LoadScene("TEST_ONLINE_BATTLE"); // Load online mode

        _steamLobbyManager.addUserAsClient();
        Debug.Log("In CheckLobbyConnection()");
    }


}
