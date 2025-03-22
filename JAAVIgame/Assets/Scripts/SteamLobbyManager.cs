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
using UnityEngine.Assertions;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using Unity.VisualScripting;

// TODO - Make a LeaveLobby Function
// TODO - look up how to properly use async method (I think we should never use void and instead return a task so that we can wait for this to finish)
public class SteamLobbyManager : MonoBehaviour
{
    #region Definitions

    [SerializeField] private uint appId;
    private const string lobbyTypeKey = "LobbyType";
    private NetworkManager _networkManager;
    private FishyFacepunch.FishyFacepunch _fishyFacepunch;
    private ClientServerInit _clientServerInit;
    private Lobby currLobby; // the curr lobby the user is in

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
            Debug.Log("Steam ID: " + SteamClient.SteamId);
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

        // TODO - If needed, remove the callback sub, implementation, and unsub
        // subscribe to events
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
    // TODO - maybe change this return type to bool or something so if we fail to join lobby we can do something (i.e. retry or tell user that there was an error)
    private async void JoinLobby(string lobbyTypeValue)
    {
        Lobby[] lobbyList = await FetchLobbies(lobbyTypeValue);
        Lobby? nLobby = null;
        if (lobbyList == null) 
        {
            // no lobbies exist, create lobby
            nLobby = await CreateLobby(lobbyTypeValue);
        }else 
        {
            // At least one lobby exist, select best lobby
            nLobby = await SelectAndJoinLobby(lobbyList);
        }



        if(nLobby == null)
        {
            Debug.Log("Failed to find, join, or create lobby...");
            return;
        }

        currLobby = (Lobby)nLobby;
        Debug.Log($"Lobby of type {currLobby.GetData(lobbyTypeKey)}");
        _fishyFacepunch.SetClientAddress(currLobby.Owner.Id.ToString());


        // start FishNet server/client

        // if user is host we need to initialize the server for them
        if (currLobby.IsOwnedBy(SteamClient.SteamId)) 
            _clientServerInit.ChangeServerState();


        // NOTE: We want to add even the host as a client to the server
        _clientServerInit.ChangeClientState();
        Debug.Log(currLobby.Members);
        Debug.Log($"To string: {currLobby.Members}");
    }

    // Finds list of joinable lobbies of type lobbyType
    // Returns a null list if none exist
    private async Task<Lobby[]> FetchLobbies(string lobbyType)
    {
        // the paramater lobbyType is either "Competitive" or "Casual"


        // TODO ON_APP_ID - delete '.WithKeyValue(OurAppKey, OurAppValue)' as this will be unnecessary with our own appID
        // filters lobbies based on lobby type
        Lobby[] lobbyList = await SteamMatchmaking.LobbyList.WithKeyValue(OurAppKey, OurAppValue).WithKeyValue(lobbyTypeKey, lobbyType).RequestAsync();
        
        // Comp MMR filtering. If no lobbies then it does not enter
        if(lobbyType == "Competitive" && lobbyList != null)
        {
            /*
             * mmrRange is the acceptable range for lobbies to join
             * Example: mmrRange is 50 and the player's mmr is 500
             * any lobby +- 50 (450-550 mmr) is considered acceptable and added to the list of lobbies
             * the lobbylist should also be sorted by range so that the closest in mmr val is first
             * the select and join function can also take into accound this and networking things (like ping) when deciding a lobby to join
             */
            const int mmrRange = 50;
            // fetch player mmr
            int? nPlayerMMR = await FetchMMR(SteamClient.SteamId);
            if (!nPlayerMMR.HasValue)
            {
                Debug.Log("Could not fetch mmr for matchmaking...");
                return null;
            }
            int playerMMR = (int)nPlayerMMR;
            int lowerMMRbound = playerMMR - mmrRange;
            int upperMMRbound = playerMMR + mmrRange;

            // filters list of lobbies
            Lobby[] filteredLobbyArray = lobbyList.Where(
                lobby => !((Int32.Parse(lobby.GetData("mmr")) < lowerMMRbound) || // less than lower bound 
                (Int32.Parse(lobby.GetData("mmr")) > upperMMRbound))).ToArray(); // less than upper bound
            if (filteredLobbyArray.Length == 0) return null;
            return filteredLobbyArray;
        }
        return lobbyList;
    }

    
    // returns null if something goes wrong
    private async Task<Lobby?> SelectAndJoinLobby(Lobby[] lobbyList)
    {

        // TODO - Implement better selection criteria
        // TODO - Something to think about: if none fit criteria from above then we will need to create a new lobby anyways...
        // How to refactor so we are not repeating the above code ^

        // lobbies exist
        return await SteamMatchmaking.JoinLobbyAsync(lobbyList[0].Id);
    }

    // creates a lobby of type lobbyType (casual or competitive)
    // returns created lobby, or null if an issue happened
    private async Task<Lobby?> CreateLobby(string lobbyType)
    {
        const int MAX_PLAYERS = 2;
        Lobby? nLobby = await Steamworks.SteamMatchmaking.CreateLobbyAsync(MAX_PLAYERS);
        if (!nLobby.HasValue)
        {
            Debug.Log("Could not create lobby...");
            return null;
        }
        
        Lobby lobby = (Lobby)nLobby;
        // data for both comp and casual games
        lobby.SetData(OurAppKey, OurAppValue); // TODO ON_APP_ID - remove this line... unnecessary set when we have our own appId
        lobby.SetData(lobbyTypeKey, lobbyType);

        // matchmaking data for comp games
        if( lobbyType == "Competitive")
        {
            int? mmr = await FetchMMR(SteamClient.SteamId);
            if (!mmr.HasValue)
            {
                Debug.Log("Error fetching mmr value.. \n");
                return null;
            }
            string mmrString = mmr.ToString();
            lobby.SetData("mmr", mmrString);
        }
        return lobby;
    }

    // result param must be -1, 0, or 1 corresponding to loss, tie, or win respectively
    // opponentSteamId is the steamId of the opponent you are playing
    // returns new mmr for the player. returns null if something went wrong
    private async Task<int?> CalcMMR(int result, SteamId opponentSteamId)
    {
        Debug.Assert((result >= -1) && (result <= 1)); // ensure result is input correct

        // TODO - Update this to better reflect the flow of the game with the game loop
        // i.e. not just the interface to show off that it works
        int? nCurrMMR = await FetchMMR(SteamClient.SteamId);
        if (!nCurrMMR.HasValue)
        {
            Debug.Log("Error fetching mmr while calculating new mmr...");
            return null;
        }
        int? mmr = (int)nCurrMMR;
        return mmr;

        // Use ELO to calculate the value of the new mmr

        // return new mmr 
    }
    // fetch's player's mmr from the database
    // retruns either an int or null which reflects their mmr
    private async Task<int?> FetchMMR(SteamId steamID)
    {
        string url = "http://129.146.86.26:18080/mmr/" + steamID;
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.timeout = 10;

        var operation = request.SendWebRequest();
        while(!operation.isDone) // wait for web request to finish
        {
            await Task.Yield();
        }

        if(request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Error sending web request to fetch mmr: {request.error}");
            return null;
        }

        JObject json = JObject.Parse(request.downloadHandler.text);
        int mmr = (int)json["mmr"];
        return mmr;
    }

    private IEnumerator SendMMR(int updatedMMR)
    {
        string url = "http://129.146.86.26:18080/updateMMR";
        // create json
        string json = "{" +
            "\"steamID\":" + SteamClient.SteamId + "," + 
            "\"mmr\":" + updatedMMR +
            "}";
        UnityWebRequest request = UnityWebRequest.Put(url, json);
        request.SetRequestHeader("Content-Type", "application/json");
        Debug.Log("Sending request...");
        yield return request.SendWebRequest();
        Debug.Log("Request sent, checkign result ...");

        if(request.result != UnityWebRequest.Result.Success )
        {
            Debug.Log($"Error sending web request to updata mmr: {request.error}");
            yield break;
        }
        Debug.Log("Successfully updated MMR");
    }

    
    private void OnApplicationQuit()
    {
        Steamworks.SteamClient.Shutdown();
    }
}
