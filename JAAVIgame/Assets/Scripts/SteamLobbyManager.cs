using Steamworks;
using Steamworks.Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SteamLobbyManager : MonoBehaviour
{
    [SerializeField] private uint appId;

    // while using test id for appId (480) the below strings differentiate lobbies created by us
    // TODO - delete this and all things associated with (labeled as ON_APP_ID) when we receive our unique app id
    private const string OurAppKey = "JAAVI";
    private const string OurAppValue = "JAAVI";

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

    // Finds casual lobby or creates new one if none exist
    public async void JoinCasualLobby()
    {
        const string lobbyTypeKey = "Lobby Type";
        const string lobbyType = "Casual";
        Lobby? nLobby;
        Lobby lobby;
        const int MAX_PLAYERS = 2;

        Lobby[] lobbyList = await FindLobbies(lobbyType);
        if (lobbyList == null) // no lobbies exist
        {
            Debug.Log($"No lobbies with type {lobbyType} exist... \n Creating new lobby");
            nLobby = await Steamworks.SteamMatchmaking.CreateLobbyAsync(MAX_PLAYERS);
            if (nLobby == null)
            {
                Debug.Log("Could not create lobby...");
                return;
            }
            lobby = (Lobby)nLobby;
            lobby.SetData(OurAppKey, OurAppValue); // TODO ON_APP_ID - unnecessary set when we have our own appId
            lobby.SetData(lobbyTypeKey, lobbyType);
            // TODO - Set Data for Casual, if any 
            Debug.Log($"Lobby of type {lobby.GetData(lobbyTypeKey)} created... \n" +
                $"Lobby ID: {lobby.Id} owned by: {lobby.Owner}");
            Debug.Log($"Number of current players: {lobby.MemberCount}");
            return;
        } // end of no lobbies exist

        // lobbyList is populated with at least one possible lobby
        // TODO: Implement way of selecting a lobby (i.e. distance, closest ranking, longest lobby wait time, etc...)
        nLobby = await SteamMatchmaking.JoinLobbyAsync(lobbyList[0].Id);
        if (nLobby == null)
        {
            Debug.Log("Could not join lobby...");
            return;
        }
        lobby = (Lobby)nLobby;
        Debug.Log($"Succesfully joined lobby of type {lobby.GetData(lobbyTypeKey)}\n" +
            $"Lobby ID: {lobby.Id}... Owned by: {lobby.Owner}");
        Debug.Log($"Number of current players: {lobby.MemberCount}");
    }

    // Finds competitive lobby or creates new one if none exist
    public async void JoinCompLobby()
    {
        const string lobbyTypeKey = "Lobby Type";
        const string lobbyType = "Competitive";
        Lobby? nLobby;
        Lobby lobby;
        const int MAX_PLAYERS = 2;

        Lobby[] lobbyList = await FindLobbies(lobbyType);
        if (lobbyList == null) // no lobbies exist
        {
            Debug.Log($"No lobbies with type {lobbyType} exist... \n Creating new lobby");
            nLobby = await Steamworks.SteamMatchmaking.CreateLobbyAsync(MAX_PLAYERS);
            if (nLobby == null)
            {
                Debug.Log("Could not create lobby...");
                return;
            }
            lobby = (Lobby)nLobby;
            lobby.SetData(lobbyTypeKey, lobbyType);
            lobby.SetData(OurAppKey, OurAppValue); // TODO ON_APP_ID - unnecessary set when we have our own appId

            // TODO - Set Data for Comp (i.e. what rankings to get in this lobby)
            Debug.Log($"Lobby of type {lobby.GetData(lobbyTypeKey)} created... \n" +
                $"Lobby ID: {lobby.Id} owned by: {lobby.Owner}");
            return;
        } // end of no lobbies exist

        // lobbyList is populated with at least one possible lobby
        // TODO: Implement way of selecting a lobby (i.e. distance, closest ranking, longest lobby wait time, etc...)
        nLobby = await SteamMatchmaking.JoinLobbyAsync(lobbyList[0].Id);
        if (nLobby == null)
        {
            Debug.Log("Could not join lobby...");
            return;
        }
        lobby = (Lobby)nLobby;
        Debug.Log($"Succesfully joined lobby of type {lobby.GetData(lobbyTypeKey)}\n" +
            $"Lobby ID: {lobby.Id}... Owned by: {lobby.Owner}");
    }

    // Finds list of joinable lobbies of type lobbyType
    // Returns null if none exist
    private async Task<Lobby[]> FindLobbies(string lobbyType)
    {
        // the paramater lobbyType is either "Competitive" or "Casual"
        string key = "Lobby Type"; // key for lobby data dictionary

        // returns list of existing lobbies which matches lobbyType
        // TODO ON_APP_ID - delete '.WithKeyValue(OurAppKey, OurAppValue)' as this unnecessary and will not be set when we get our own appId
        Lobby[] FilteredLobbyList = await SteamMatchmaking.LobbyList.WithKeyValue(OurAppKey, OurAppValue).WithKeyValue(key, lobbyType).RequestAsync();
        if (FilteredLobbyList == null) return null; // no lobbies exist with our criteria
        // lobbies exist
        return FilteredLobbyList;
    }


    private void OnApplicationQuit()
    {
        Steamworks.SteamClient.Shutdown();
    }
}
