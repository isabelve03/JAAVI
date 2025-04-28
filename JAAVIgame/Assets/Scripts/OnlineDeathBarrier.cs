using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

public class OnlineDeathBarrier : NetworkBehaviour
{
    private bool canKill = false;

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


        foreach (var item in ServerManager.Clients)
        {
            foreach (var Object in item.Value.Objects)
            {
                if(Object.gameObject == collision.gameObject)
                {
                    TargetLog(item.Value);
                }
            }
        }
        ServerLog();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ServerLog()
    {
        Debug.Log("[SERVER] Death Barrier Collision registered..");
    }

    [TargetRpc]
    private void TargetLog(NetworkConnection conn)
    {
        Debug.Log("[TARGET] This target collided with the death barrier");
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