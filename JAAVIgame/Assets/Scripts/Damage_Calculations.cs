using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Damage_Calculations : MonoBehaviour
{
    Rigidbody2D playerCharacter;
    public int currentHealth;
    void Start()
    {
        playerCharacter = GetComponent<Rigidbody2D>();
        currentHealth = 0;
    }

    //would like to refine both knockback and hitstun formulas eventually
    public void TakeKnockback(bool isFacingRight, Vector2 attackAngle, float scaledKB)
    {
        scaledKB *= currentHealth * 0.12f; //Adds a knockback multiplier based on damage before attack hit
        attackAngle.x += scaledKB; //adds knockback multiplier to base knockback
        attackAngle.y += scaledKB; //adds knockback multiplier to base knockback
        //GetComponent<PlayerMovement>().hitStun = (int)Math.Round(Math.Max(attackAngle.x, attackAngle.y), 0, MidpointRounding.AwayFromZero) - 1; //adds hitstun to knockback victim
        GetComponent<PlayerMovement>().hitStun = 30; //janky fix because low on time
        if(!isFacingRight){ //switches direction of x knockback if player is facing left
            attackAngle.x = -attackAngle.x;
        }
        playerCharacter.AddForce(attackAngle, ForceMode2D.Impulse);
    }

    public void TakeDamage(int damage)
    {
        if(GetComponent<PlayerMovement>().isBlocking == true){
            damage = damage / 2; //reduces damage by half from a successful block
        }
        currentHealth += damage;
    }
}

