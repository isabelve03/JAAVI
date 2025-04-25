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
    // Start is called before the first frame update
    void Start()
    {
        playerCharacter = GetComponent<Rigidbody2D>();
        attackZone = new GameObject("attackZone");
        attackZone.transform.parent = transform;
        attackZone.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f); // sets pos relative to parent
    }

    private void UpdateDirection(Vector2 dir)
    {
        movementInput = dir;
        isGrounded = GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    public void GetAttack(string attackType)
    {
        attackZone.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f); // sets pos rel to parent
        //if(attackType == "StrongAttack")
        //{
            //put in the attackdata or you can check if it was a strong aerial and stuff like that inside
            //Debug.Log("Did strong attack");
        //}

        if(attackType == "LightAttack") // Only one attack for MVP
        {
            // NOTE: there is other code commented on in normal Combat script. 
            // This is not brought over so as to not clutter this script.
            // When that is implemented add here w/ necessary network stuff

            attackZone.transform.localPosition += GetComponent<AttackData>().jabHitbox;
            attackRange = GetComponent<AttackData>().jabRange;
            attackDamage = GetComponent<AttackData>().jabDam;
            baseKnockback = GetComponent<AttackData>().jabBaseK;
            scaledKnockback = GetComponent<AttackData>().jabScaleK;
        }

        // LOTS of other commented out code which needs to be added and networked here when implemented

    }

    [ServerRpc]
    public void s_Attack(NetworkConnection conn)
    {
        foreach (var item in ServerManager.Clients)
        {
            if(conn != item.Value)
            {
                t_Attack(item.Value, attackDamage);
                break;
            }
        }

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
}
