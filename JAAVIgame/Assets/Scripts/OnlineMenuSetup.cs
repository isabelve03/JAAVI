// used to ensure comp and casual buttons still work after re-entering OnlineOptions scene
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineMenuSetup : MonoBehaviour
{
    [SerializeField] private Button joinCasualButton;
    [SerializeField] private Button joinCompetitiveButton;

    private void Awake()
    {
        GameObject _networkManager = GameObject.Find("NetworkManager");
        if(_networkManager == null)
        {
            Debug.LogError("Network manager not found...", this);
            return;
        }

        SteamLobbyManager _steamLobbyManager = _networkManager.GetComponent<SteamLobbyManager>();
        OnlineOptionsSceneManager _onlineOptionsSceneManager = _networkManager.GetComponent<OnlineOptionsSceneManager>();

        if (_steamLobbyManager == null ||  _onlineOptionsSceneManager == null)
        {
            Debug.LogError("Required Components not found...", this);
            return;
        }

        // avoid possiblity of duplicates
        joinCasualButton.onClick.RemoveAllListeners();
        joinCompetitiveButton.onClick.RemoveAllListeners();

        // casual button
        joinCasualButton.onClick.AddListener(() => _steamLobbyManager.JoinCasualLobby());
        joinCasualButton.onClick.AddListener(() => _onlineOptionsSceneManager.JoinGameButtonPressed());

        // comp button
        joinCompetitiveButton.onClick.AddListener(() => _steamLobbyManager.JoinCompetitveLobby());
        joinCompetitiveButton.onClick.AddListener(() => _onlineOptionsSceneManager.JoinGameButtonPressed());
    }
}
