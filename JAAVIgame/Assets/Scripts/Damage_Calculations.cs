using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Damage_Calculations : MonoBehaviour
{
    public int currentHealth;
    void Start(){
        currentHealth = 0;
    }
    public void TakeDamage(int damage){
        currentHealth += damage;
    }

    public void TakeKnockback(){
        //should run knockback calculations
    }
}
