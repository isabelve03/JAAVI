// This script is temporary and meant to allow functionality to buttons for showing off matchmaking
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempOnlineButtonManager : MonoBehaviour
{
    [SerializeField] private Button WinGame;
    [SerializeField] private Button DrawGame;
    [SerializeField] private Button LoseGame;
    private SteamLobbyManager steamLobbyManager;
    // Start is called before the first frame update
    void Start()
    {
        steamLobbyManager = FindAnyObjectByType<SteamLobbyManager>();

        if(steamLobbyManager == null)
        {
            Debug.Log("Steam Lobby Manager could not be found");
            return;
        }

        WinGame.onClick.AddListener(steamLobbyManager.UpdateMMRWin);
        DrawGame.onClick.AddListener(steamLobbyManager.UpdateMMRDraw);
        LoseGame.onClick.AddListener(steamLobbyManager.UpdateMMRLoss);

    }

}
