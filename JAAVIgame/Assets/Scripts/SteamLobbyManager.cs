using FishNet.Managing;
using Steamworks;
using Steamworks.Data;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

// TODO - Make a LeaveLobby Function

public class SteamLobbyManager : MonoBehaviour
{
    #region Definitions

    [SerializeField] private uint appId;
    private const string lobbyTypeKey = "LobbyType";
    private NetworkManager _networkManager;
    private FishyFacepunch.FishyFacepunch _fishyFacepunch;
    private ClientServerInit _clientServerInit;

    // while using test id for appId (480) the below strings ensure we find only our lobbies
    // TODO - delete this and all things asociated with (labeled as ON_APP_ID) when we receive our unique app id
    private const string OurAppKey = "JAAVI";
    private const string OurAppValue = "JAAVI";

    #endregion Definitions

    private void Awake()
    {
        DontDestroyOnLoad(this); // ensures this script persists past current scene

        try // Initialize steamclienet and ensure it exists
        {
            if (Steamworks.SteamClient.RestartAppIfNecessary(appId)) return;
            if (!SteamClient.IsValid) Steamworks.SteamClient.Init(appId, true);
            Debug.Log($"Successfully logged in through steam... \n" +
                $"Users Steam name: {SteamClient.Name}");
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    private void Start()
    {
        _networkManager = FindObjectOfType<NetworkManager>();
        if(_networkManager == null)
        {
            Debug.LogError("NetworkManager not found...");
            return;
        }

        _fishyFacepunch = FindObjectOfType<FishyFacepunch.FishyFacepunch>();
        if(_fishyFacepunch == null)
        {
            Debug.LogError("FishyFacepunch not found...");
            return;
        }

        _clientServerInit = FindObjectOfType<ClientServerInit>();
        if(_clientServerInit == null)
        {
            Debug.LogError("ClientServerInit not found...");
            return;
        }

        // subscribe to events
        SteamMatchmaking.OnLobbyMemberLeave += SteamMatchmaking_OnLobbyMemberLeave;
    }

    // Callbacks 
    private void SteamMatchmaking_OnLobbyMemberLeave(Lobby lobby, Friend friend)
    {
        Debug.Log("Friend has left the lobby"); 
    }

    private void OnDestroy()
    {
        //unsub from callbacks
        SteamMatchmaking.OnLobbyMemberLeave -= SteamMatchmaking_OnLobbyMemberLeave;
    }


    // Calls general JoinLobby function for type casual
    public void JoinCasualLobby()
    {
        const string lobbyTypeValue = "Casual";
        JoinLobby(lobbyTypeValue);
    }
    

    // Calls general JoinLobby function for type competitve
    public void JoinCompetitveLobby()
    {
        const string lobbyTypeValue = "Competitive";
        JoinLobby(lobbyTypeValue);
    }

    
    // General JoinLobby function. Joins or creates a lobby for casual or competitive
    // based on the lobbyTypeValue supplied
    private async void JoinLobby(string lobbyTypeValue)
    {
        Lobby[] lobbyList = await FetchLobbies(lobbyTypeValue);
        Lobby? nLobby = await SelectAndJoinLobby(lobbyList);

        if(nLobby == null)
        {
            Debug.Log("Failed to find, join, or create lobby...");
            return;
        }

        Lobby lobby = (Lobby)nLobby;
        //TODO - Set Data For Lobby (should it be its own function so as to not repeat code?)
        lobby.SetData(OurAppKey, OurAppValue); // TODO ON_APP_ID - remove this line... unnecessary set when we have our own appId
        lobby.SetData(lobbyTypeKey, lobbyTypeValue);
        Debug.Log($"Lobby of type {lobby.GetData(lobbyTypeKey)}");
        _fishyFacepunch.SetClientAddress(lobby.Owner.Id.ToString());


        // start FishNet server/client

        // if user is host we need to initialize the server for them
        if (lobby.IsOwnedBy(SteamClient.SteamId)) 
            _clientServerInit.ChangeServerState();


        // NOTE: We want to add even the host as a client to the server
        _clientServerInit.ChangeClientState();
        
            
    }

    // Finds list of joinable lobbies of type lobbyType
    // Returns null if none exist
    private async Task<Lobby[]> FetchLobbies(string lobbyType)
    {
        // the paramater lobbyType is either "Competitive" or "Casual"

        // TODO ON_APP_ID - delete '.WithKeyValue(OurAppKey, OurAppValue)' as this will be unnecessary with our own appID
        Lobby[] FilteredLobbyList = await SteamMatchmaking.LobbyList.WithKeyValue(OurAppKey, OurAppValue).WithKeyValue(lobbyTypeKey, lobbyType).RequestAsync();
        return FilteredLobbyList;
    }

    
    // returns null if something goes wrong
    private async Task<Lobby?> SelectAndJoinLobby(Lobby[] lobbyList)
    {
        if(lobbyList == null) // no existing lobbies, create new one
        {
            const int MAX_PLAYERS = 2;
            Debug.Log("No lobbies exist... \n Creating new lobby...");
            return await Steamworks.SteamMatchmaking.CreateLobbyAsync(MAX_PLAYERS);
        }

        // TODO - Implement better selection criteria
        // TODO - Something to think about: if none fit criteria from above then we will need to create a new lobby anyways...
        // How to refactor so we are not repeating the above code ^

        // lobbies exist
        return await SteamMatchmaking.JoinLobbyAsync(lobbyList[0].Id);
    }

    // callbacks
    
    private void OnApplicationQuit()
    {
        Debug.Log("App Quit");
        Steamworks.SteamClient.Shutdown();
    }
}
