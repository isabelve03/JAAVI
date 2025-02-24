using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class Test_Knockback : MonoBehaviour
{
    public Collider2D[] attackHitboxes;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
            LaunchAttack(attackHitboxes[0]);
        if(Input.GetKeyDown(KeyCode.K))
            LaunchAttack(attackHitboxes[1]);
        
    }
    private void LaunchAttack(Collider2D col){
        Collider2D cols = Physics2D.OverlapCapsule(col.bounds.min, col.bounds.max, CapsuleDirection2D.Horizontal, LayerMask.GetMask("Player"));
        Debug.Log(cols.name);
    }
}
