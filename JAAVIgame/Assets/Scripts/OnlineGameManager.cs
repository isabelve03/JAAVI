using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet;

public class OnlineGameManager : NetworkBehaviour
{
    public static OnlineGameManager Instance { get; private set; }
    public NetworkObject Player1 { get; private set; }
    public NetworkObject Player2 { get; private set; }
    private int numPlayers = 0;

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
    }

    // client calls this to notify server it is in battle scene and ready to be spawned
    [ServerRpc]
    public void RegisterPlayer(NetworkObject player, int playerIndex)
    {
        Debug.Log($"In Register Player: {playerIndex}");
        if (!InstanceFinder.IsServerStarted) return; // Only allow the server to register players

        if (playerIndex == 1)
        {
            Player1 = player;
            Debug.Log("Registerd Player 1");
        }
        else
        {
            Player2 = player;
            Debug.Log("Registered player 2");
        }
    }


}
