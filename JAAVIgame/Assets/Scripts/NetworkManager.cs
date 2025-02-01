using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Steamworks;
using Steamworks.Data;
using Netcode.Transports.Facepunch;
using System.Threading.Tasks;
using UnityEditor;

public class NetworkManager : MonoBehaviour
{
    [SerializeField] private uint appId;

    private void Awake()
    {
        DontDestroyOnLoad(this); // ensures NetwrokManager script exists past new scenes
        
        try // Initialize steamclient and ensure it exists
        {
            if (Steamworks.SteamClient.RestartAppIfNecessary(appId)) return;
            Steamworks.SteamClient.Init(appId, true);
            Debug.Log($"User's Steam Name: {SteamClient.Name}");
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    public async void JoinLobby()
    {
        Lobby? nLobby;
        Lobby lobby;
        // Steamworks Documentation requires any filters to be set prior to requestlobbylist, however
        // with Facepunch, I am not sure if it is done differently

        Steamworks.Data.LobbyQuery LobbyQueryResults = SteamMatchmaking.LobbyList; // Query for list of lobbies
        // set filters here maybe?
        // Ex: LobbyQueryResults.FilterDistanceClose()
        // LobbyQueryResults.ApplyFilters()
        string key = "Ranking";
        string value = "Bronze";
        LobbyQueryResults.WithKeyValue(key, value);
        Lobby[] FilteredLobbyList = await LobbyQueryResults.RequestAsync();
        if(FilteredLobbyList == null) // no lobbies fit our criteria
        {
            Debug.Log("No Lobbies Found, Creating New Lobby");
            nLobby = await Steamworks.SteamMatchmaking.CreateLobbyAsync(2); // param (in this case 2) is max number of players in lobby
            if (nLobby == null)
            {
                Debug.Log("Could Not Create Lobby");
                return;
            }
            lobby = (Lobby)nLobby;
            lobby.SetData(key, value);
            lobby.SetPublic(); // TODO - Decide if we need this to be public or private
            Debug.Log($"Lobby created with ID: {lobby.Id}, ranking of {lobby.GetData(key)}, and owned by {lobby.Owner}");
            return;
        }


        // Have valid lobby in list
        
        Debug.Log("There exists a lobby");
        nLobby = await Steamworks.SteamMatchmaking.JoinLobbyAsync(FilteredLobbyList[0].Id);
        if (nLobby == null)
        {
            Debug.Log("Error Joining Lobby");
            return;
        }
        lobby = (Lobby)nLobby;
        Debug.Log($"Joined lobby: {lobby.Id} owned by {lobby.Owner}");

    }


    private void OnApplicationQuit()
    {
        Steamworks.SteamClient.Shutdown();
    }


}
