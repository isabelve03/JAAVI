using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;

using FishNet;
using UnityEditor;
using System.Drawing.Text;

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

    #region BLOCK
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
    #endregion BLOCK

    #region ATTACK1

    [ServerRpc]
    public void ServerAttack1(Transform attackerTransform, Vector3 hitBox, float range, int damage, Vector2 baseKnockback, float scaledKnockback)
    {
        // NOTE: This is for the light attack of a character

        // attackZone init
        GameObject attackZone = new GameObject("attackZone");
        attackZone.transform.parent = attackerTransform;
        attackZone.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f) + hitBox;

        // CURRENT STATE: The combat's hit detection uses stuff like tags and layer masks to determine
        // hits and what not. Austin and Isabel mentioned that they are looking at changing this
    }

    [ObserversRpc]
    public void ClientAttack1()
    {

    }
    #endregion ATTACK1
}
