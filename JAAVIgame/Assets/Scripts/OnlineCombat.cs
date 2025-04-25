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
        Debug.Log("[SERVER] In light attack");
        NetworkObject oppPlayer = null;
        NetworkObject currPlayer = null;
        NetworkConnection oppConn = null;


        foreach (var item in ServerManager.Clients)
        {
            if(item.Value == conn)
            {
                foreach (var Object in item.Value.Objects)
                {
                    if(Object.GetComponent<AttackData>() != null)
                    {
                        currPlayer = Object;
                    }
                }
            }
            else
            {
                oppConn = item.Value;
                foreach (var Object in item.Value.Objects)
                {
                    if(Object.GetComponent<AttackData>() != null)
                    {
                        oppPlayer = Object;
                    }
                }

            }
        }

        if(oppConn == null ||  oppPlayer == null || currPlayer == null)
        {
            Debug.Log("Could not find either the opponents player/connection, or the current player's connection");
            return;
        }

        GetLightAttack(currPlayer);
        Collider2D[] hitOpponnet = Physics2D.OverlapCircleAll(attackZone.transform.position, attackRange);
        foreach(Collider2D collider in hitOpponnet)
        {
            if(collider.gameObject == oppPlayer.gameObject)
            {
                t_Attack(oppConn, conn, attackDamage, oppPlayer);
                break; // should be a max of 1 colliders in hitOpponent (hopefully), but if there isn't at least they only take dam once
            }
        }
    }

    [TargetRpc]
    private void t_AttackBlocked(NetworkConnection conn)
    {
        Debug.Log("[TARGET] Your attack was blocked");
    }

    [TargetRpc]
    private void t_Attack(NetworkConnection conn, NetworkConnection attackerConn, int dam, NetworkObject player)
    {
        Debug.Log("[TARGET] Func with network object");
        if (player.GetComponent<TestOnlinePlayerMovementNew>().isBlocking) 
        {
            t_AttackBlocked(attackerConn);
            dam = dam / 2;
        }
        if(GetComponent<Damage_Calculations>() == null)
        {
            Debug.Log("[TARGET] Could not find damage calculations...");
        }
        player.GetComponent<Damage_Calculations>().currentHealth += dam;
        Debug.Log($"[TARGET] Hit with {dam} damage");
    }



    #endregion RPC
}
