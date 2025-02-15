using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public int hitPoints = 0; //hitpoints start at 0 and increment up until character dies, then they are reset


    public void TakeKnockback(float attackAngle, bool faceRight){

    }
    //damage is added to the hitpoint total AFTER knockback is calculated
    public void TakeDamage(int attackVal){
        hitPoints += attackVal;
    }    
        //sample damage calculator
        //knockbackUnits = hitpoints * baseDamage * scaleDamage
        //Vector 2 to launch fighter depending on what angle the hitbox sends at, what direction the opponent was facing, how much damage the player has currently
        //hitpoints = hitpoints + baseDamage + 
        //hitpoints will be reduced to 0 when the player dies
}
