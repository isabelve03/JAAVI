using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using UnityEngine;

public class OnlineDeathBarrier : NetworkBehaviour
{
    private bool canKill = false;
    private GameObject _victoryScreen;
    private bool triggered = false;

    private void Start()
    {
        Invoke("EnableKill", 0.5f); // Adjust delay if needed
    }

    void EnableKill()
    {
        canKill = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canKill)
            return;
        if (triggered)
        {
            return;
        }

        triggered = true;
        NetworkConnection winner, loser;
        getConnections(out winner, out loser, collision.gameObject);
        if(winner == null || loser == null)
        {
            Debug.LogWarning("sumn null");
            return;
        }


        s_LockPlayers();
        t_ShowWinScreen(winner);
        t_ShowWinScreen(loser); // TODO - Temp show win screen to give user an opportunity to return to home (fix with a loss screen)
    }







    private void getConnections(out NetworkConnection winner, out NetworkConnection loser, GameObject collision)
    {
        winner = null;
        loser = null;
        foreach (var currClient in ServerManager.Clients)
        {
            foreach (var Object in currClient.Value.Objects)
            {
                if (Object.gameObject == collision.gameObject)
                {
                    loser = currClient.Value;
                    break;
                }
                winner = currClient.Value;
            }
        }
    }

    //[ServerRpc]
    private void s_LockPlayers()
    {
        foreach (var Client in ServerManager.Clients)
        {
            t_LockPlayer(Client.Value);
        }
    }

    [TargetRpc]
    private void t_LockPlayer(NetworkConnection conn)
    {
        foreach(var Object in conn.Objects)
        {
            TestOnlinePlayerMovementNew pm = Object.GetComponent<TestOnlinePlayerMovementNew>();
            if(pm != null)
            {
                pm.GameOver();
            }
        }
    }


    [TargetRpc]
    private void t_ShowWinScreen(NetworkConnection conn)
    {
        Debug.Log("[TARGET] Showing win screen");
        OnlineVictoryScreen ovs = FindObjectOfType<OnlineVictoryScreen>();
        if(ovs == null)
        {
            Debug.LogWarning("Could not find OnlineVictoryScreen...");
            return;
        }

        ovs.ShowVictoryScreen();
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canKill) return;
        
        Debug.Log("Triggered by: " + collision.gameObject.name);
        
        foreach (PlayerData player in PlayerJoinManager.Instance.joinedPlayers)
        {
            if (player.spawnedPlayer == collision.gameObject)
            {
                PlayerMovement pm = collision.gameObject.GetComponent<PlayerMovement>();
                if (pm != null)
                {
                    pm.Die(); // <- add more to this method later
                }

                Destroy(collision.gameObject);
                player.spawnedPlayer = null;
                CheckWinCondition();
                break;
            }
        }
    }

    private void CheckWinCondition()
    {
        int aliveCount = 0;
        PlayerData lastStanding = null;

        foreach (PlayerData player in PlayerJoinManager.Instance.joinedPlayers)
        {
            if (player.spawnedPlayer != null)
            {
                aliveCount++;
                lastStanding = player;
            }
        }

        if (aliveCount == 1)
        {
            Debug.Log("Player " + (lastStanding.controllerID+1) + " wins!");
            FindObjectOfType<VictoryManager>().ShowVictoryScreen("Player " + (lastStanding.controllerID+1));
        }
        else if (aliveCount == 0)
        {
            Debug.Log("Everyone died! It’s a draw!");
            FindObjectOfType<VictoryManager>().ShowVictoryScreen("No one");
        }
    }
    */
}