using FishNet;
using FishNet.Connection;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineCombat : NetworkBehaviour
{
    private Vector2 movementInput;
    //public Transform attackZone; //circular hitbox for now
    public float attackRange = 0.5f; //will be set in AttackData once I implement more than one attack
    public LayerMask opponentLayers;
    private string playerTag;
    Rigidbody2D playerCharacter;
    private GameObject attackZone;
    //public bool inLag = false;

    //public bool isDead =  false;
    private bool isGrounded;
    private bool blocked = false;
    private Vector2 pushBack;
    private float pushDam;

    //attack data definitions
    private int attackDamage;
    private Vector2 baseKnockback;
    private float scaledKnockback;
    private void UpdateDirection(Vector2 dir)
    {
        movementInput = dir;
        isGrounded = GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    #region GETATTACKS

    private void GetLightAttack(NetworkObject player)
    {
        AttackData _attackData = player.GetComponent<AttackData>();
        attackZone = new GameObject("attackZone");
        attackZone.transform.parent = transform;
        attackZone.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f); // sets pos rel to parent

            // NOTE: there is other code commented on in normal Combat script. 
            // This is not brought over so as to not clutter this script.
            // When that is implemented add here w/ necessary network stuff

        attackZone.transform.localPosition += _attackData.jabHitbox;
        attackRange = _attackData.jabRange;
        attackDamage = _attackData.jabDam;
        baseKnockback = _attackData.jabBaseK;
        scaledKnockback = _attackData.jabScaleK;

        // LOTS of other commented out code which needs to be added and networked here when implemented

    }
    #endregion GETATTACKS

    #region RPC

    // In future when more attacks, change this to just attack and pass a string to identify which get func to call
    [ServerRpc]
    public void s_LightAttack(NetworkConnection conn)
    {
        OnlineGameManager _onlineGameManager = FindObjectOfType<OnlineGameManager>();
        NetworkObject oppPlayer;
        NetworkObject currPlayer;
        NetworkConnection oppConn;

        if (conn == _onlineGameManager._hostConn)
        {
            currPlayer = _onlineGameManager._hostCharacter;
            oppPlayer = _onlineGameManager._clientCharacter;
            oppConn = _onlineGameManager._clientConn;
        }
        else
        {
            currPlayer = _onlineGameManager._clientCharacter;
            oppPlayer = _onlineGameManager._hostCharacter;
            oppConn = _onlineGameManager._hostConn;
        }
        // NOTE: THIS IS JANK
        // only sees if there is something in the defualt layer colliding, and then sends it if there is
        // Only works if there are no other colliders on default layer other than the 2 players
        // Current issue: the collider game object is the server's 'clone' while the oppPlayer is the local player for that machine and they are technically different
        GetLightAttack(currPlayer);
        int layerMask = LayerMask.GetMask("Default");
        Collider2D[] hitOpponnet = Physics2D.OverlapCircleAll(attackZone.transform.position, attackRange, layerMask);
        foreach(Collider2D collider in hitOpponnet)
        {
            t_Attack(oppConn, attackDamage);
            Debug.Log($"[SERVER] Damage from this player: {attackDamage}");
            break; // should be a max of 1 colliders in hitOpponent (hopefully), but if there isn't at least they only take dam once
        }
    }

    [TargetRpc]
    private void t_Attack(NetworkConnection conn, int dam)
    {
        if(GetComponent<TestOnlinePlayerMovementNew>().isBlocking)
        {
            Debug.Log("Blocking");
            dam = dam / 2;
        }
        if(GetComponent<Damage_Calculations>() == null)
        {
            Debug.Log("[TARGET] Could not find damage calculations...");
        }
        GetComponent<Damage_Calculations>().currentHealth += dam;
        Debug.Log($"[TARGET] Hit with {dam} damage");
    }

    [ServerRpc]
    public void s_BlockCheck()
    {
        int cnt = 0;
        Debug.Log("[SERVER] In block check");
        foreach (var item in ServerManager.Clients)
        {
            Debug.Log("[SERVER] In clients loop");
            cnt++;
            foreach (var Object in item.Value.Objects)
            {
                Debug.Log("[SERVER] In objects loop");
                Debug.Log($"[SERVER] Object {Object.name} for client # {cnt}");
            }
        }
    }


    #endregion RPC
}
