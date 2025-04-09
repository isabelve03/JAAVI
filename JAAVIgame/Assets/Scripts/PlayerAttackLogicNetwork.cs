using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;

using FishNet;

public class PlayerAttackLogicNetwork : NetworkBehaviour
{
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner) // disable if not owner of character and return
        {
            GetComponent<PlayerAttackLogicNetwork>().enabled = false;
            return;
        }
    }

    [ServerRpc]
    public void ServerBlock(int x) // Block is pressed
    {
        Debug.Log($"SERVER: Block received on server from player: {x}");
        ClientBlock(x);
    }

    [ObserversRpc]
    public void ClientBlock(int x)
    {
        int y;
        if (InstanceFinder.IsServerStarted)
        {
            y = 0;
        }
        else
        {
            y = 1;
        }
        Debug.Log($"CLIENT: Block transmitted to player {y}. Initiated from player {x}");
    }
}
