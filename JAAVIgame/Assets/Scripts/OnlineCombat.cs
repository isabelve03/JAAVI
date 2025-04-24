// Online adaptation of the Combat.cs script
// Implements the same hit/miss/block logic with changes centering on how an opponent is decided (i.e. tags vs no tags)
// NOTE: Most comments (such as currently commented out but soon to be implemented code) are stripped
// When these changes are made this script should be edited to reflect it
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEditor.Build;

public class OnlineCombat : MonoBehaviour
{
    private Vector2 movementInput;
    private Vector2 direction;
    //public Transform attackZone; //circular hitbox for now
    public float attackRange = 0.5f; //will be set in AttackData once I implement more than one attack
    public LayerMask opponentLayers;
    private string playerTag;
    private GameObject attackZone;
    //public bool inLag = false;

    //public bool isDead =  false;
    private bool isGrounded;
    public AttackData attackData;

    //attack data definitions
    private int attackDamage;
    private Vector2 baseKnockback;
    private float scaledKnockback;

    // Start is called before the first frame update
    void Start()
    {
        attackZone = new GameObject("attackZone");
        attackZone.transform.parent = transform;
        attackZone.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }

    private void UpdateDirection(Vector2 dir)
    {
        movementInput = dir;
        isGrounded = GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("ground"));
    }

    public void GetAttack(string attackType)
    {
        attackZone.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f); // Sets the position relative to the parent

        if(attackType == "LightAttack")
        {
            attackZone.transform.localPosition = attackZone.transform.localPosition + GetComponent<AttackData>().jabHitbox;
            attackRange = GetComponent<AttackData>().jabRange;
            attackDamage = GetComponent<AttackData>().jabDam;
            baseKnockback = GetComponent<AttackData>().jabBaseK;
            scaledKnockback = GetComponent<AttackData>().jabScaleK;
            Attack();


            attackZone.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }
    
    // applies knockback and damage to opponent characters
    void Attack()
    {
        HashSet<GameObject> alreadyDamaged = new HashSet<GameObject>();
        Collider2D[] hitOpponent = Physics2D.OverlapCircleAll(attackZone.transform.position, attackRange, opponentLayers);
        playerTag = gameObject.tag;
    }
}
