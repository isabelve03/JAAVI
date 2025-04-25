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
        GetLightAttack(currPlayer);
        Collider2D[] hitOpponnet = Physics2D.OverlapCircleAll(attackZone.transform.position, attackRange);
        int cnt = 0;
        foreach(Collider2D collider in hitOpponnet)
        {
            cnt++;
            Debug.Log($"Name: {collider.gameObject.name}");
            Debug.Log($"If equal no .gameObject: {collider.gameObject.Equals(oppPlayer)}");
            Debug.Log($"If equal with .gameObject: {collider.gameObject.Equals(oppPlayer.gameObject)}");
            Debug.Log($"Collider {cnt}");
            if(collider.gameObject == oppPlayer.gameObject)
            {
                t_Attack(oppConn, attackDamage);
            }
        }

        Debug.Log($"[SERVER] Damage from this player: {attackDamage}");

        // check opp character
        // see if hit
        // if hit, send to player

    }

    [TargetRpc]
    private void t_Attack(NetworkConnection conn, int Damage)
    {
        Debug.Log($"[TARGET] Hit with {Damage} damage");
    }

    [ServerRpc]
    public void s_Accessed(NetworkConnection conn)
    {
        Debug.Log($"SERVER: Received network connection: {conn}");
        foreach (var item in ClientManager.Clients)
        {
            Debug.Log($"[SERVER] Client #{item.Key}: connection = {item.Value}");
        }
        c_Accessed(conn);
    }

    [ObserversRpc]
    private void c_Accessed(NetworkConnection conn)
    {
        Debug.Log($"CLIENT: Received network connection: {conn}");
        if (conn == ClientManager.Connection)
        {
            Debug.Log("I sent this message...");
        }
        if(conn != ClientManager.Connection)
        {
            Debug.Log($"CLIENT: I am the opponent and my player is {gameObject.name}");
        }
    }

    #endregion RPC
}
