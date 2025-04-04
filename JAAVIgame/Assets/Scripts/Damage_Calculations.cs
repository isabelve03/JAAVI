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

    public void TakeKnockback(bool isFacingRight, Vector2 attackAngle, float scaledKB)
    {
        scaledKB *= currentHealth * 0.12f; //Adds a knockback multiplier based on damage before attack hit
        attackAngle.x += scaledKB; //adds knockback multiplier to base knockback
        attackAngle.y += scaledKB; //adds knockback multiplier to base knockback
        if(!isFacingRight){ //switches direction of x knockback if player is facing left
            attackAngle.x = -attackAngle.x;
        }
        playerCharacter.AddForce(attackAngle, ForceMode2D.Impulse);
    }

    public void TakeDamage(int damage)
    {
        currentHealth += damage;
    }
}

