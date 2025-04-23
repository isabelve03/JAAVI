using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class OnlineLobbyHelper : NetworkBehaviour
{
    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("On Start Client");
    }
/*
    [ServerRpc]
    public void ServerSpawn
*/
}
