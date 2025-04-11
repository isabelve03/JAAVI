using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Combat : MonoBehaviour
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
    private bool blocked = false;

    //attack data definitions
    private int attackDamage;
    private Vector2 baseKnockback;
    private float scaledKnockback;

    
    void Start(){
        attackZone = new GameObject("attackZone");
        attackZone.transform.parent = transform;
        attackZone.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f); // Sets the position relative to the parent
    }

    private void UpdateDirection(Vector2 dir)
    {
        movementInput = dir;
        isGrounded = GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    public void GetAttack(string attackType)
    {
        attackZone.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f); // Sets the position relative to the parent
        //if(attackType == "StrongAttack")
        //{
            //put in the attackdata or you can check if it was a strong aerial and stuff like that inside
            //Debug.Log("Did strong attack");
        //}

        if(attackType == "LightAttack") //we are only going to develop for one kind of attack for the MVP
        //we will add more attacks WHEN we continue working on this after the class is over :)
        {
            //put in the attackdata or you can check if it was a light aerial and stuff like that inside
            //you can use movementInput.y to check if its not zero to add in aerial elements
            // then you can use the attack function to use the data from attack data
            //Debug.Log("Did light attack");
            //example

           // if(isGrounded) //grounded attacks
           // {
                //if(movementInput.y == 0 && movementInput.x == 0) //stand still
                //{
                    attackZone.transform.localPosition = attackZone.transform.localPosition + GetComponent<AttackData>().jabHitbox; //sets attack position
                    attackRange = GetComponent<AttackData>().jabRange; //sets attack range
                    attackDamage = GetComponent<AttackData>().jabDam; //
                    baseKnockback = GetComponent<AttackData>().jabBaseK;
                    scaledKnockback = GetComponent<AttackData>().jabScaleK;
                    Attack();
                //}
                //else if((movementInput.x <= Math.Abs(movementInput.y)) && movementInput.y > 0)
                //{ 
                    //Attack(GetComponent<AttackData>().uLightDam);
                    //actually this is a bad example cause this one is if its in the air and not grounded
                //}
            //}
            //else
            //{
                //all the nongrounded attacks here
            //}


        }


//         if(isGrounded)
//         { //grounded attacks
//             if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
//             { //stand still
//                 if(Input.GetButtonDown("Fire2"))
//                 { //jab
//                     //only attack I am working on rn
//                     Attack(gameObject.transform.GetComponent<AttackData>().jabDam);
//                 }
//                 else if(Input.GetButtonDown("Fire3"))
//                 { 
//                     //neutral special

//                 }
//             }

//             else if(Math.Abs(Input.GetAxis("Horizontal")) <= Math.Abs(Input.GetAxis("Vertical")) && Input.GetAxis("Vertical") > 0){ //up attacks
//                 if(Input.GetButtonDown("Fire2")){ //up light
//                     //Attack(gameObject.transform.parent.GetComponent<AttackData>().uLightDam);
//                 }
//                 else if(Input.GetButtonDown("Fire3")){ //up special
                   
//                 }
//                 else if(Input.GetButtonDown("Fire2") && Input.GetButtonDown("Fire3")){ //up strong
//                     //Attack(gameObject.transform.parent.GetComponent<AttackData>().uStrongDam);
//                 }
//             }




//             else if(Math.Abs(Input.GetAxis("Horizontal")) <= Math.Abs(Input.GetAxis("Vertical")) && Input.GetAxis("Vertical") < 0){ //down attacks
//                 if(Input.GetButtonDown("Fire2")){ //down light
//                     //Attack(gameObject.transform.parent.GetComponent<AttackData>().dLightDam);
//                 }
//                 else if(Input.GetButtonDown("Fire3")){ //down special
                   
//                 }
//                 else if(Input.GetButtonDown("Fire2") && Input.GetButtonDown("Fire3")){ //down strong
//                     //Attack(gameObject.transform.parent.GetComponent<AttackData>().dStrongDam);
//                 }
//             }




//             else if(Math.Abs(Input.GetAxis("Horizontal")) > Math.Abs(Input.GetAxis("Vertical"))){ //forward attacks
//                 if(Input.GetButtonDown("Fire2")){ //forward light
//                     //Attack(gameObject.transform.parent.GetComponent<AttackData>().fLightDam);
//                 }
//                 else if(Input.GetButtonDown("Fire3")){ //forward special
                   
//                 }
//                 else if(Input.GetButtonDown("Fire2") && Input.GetButtonDown("Fire3")){ //forward strong
//                     //Attack(gameObject.transform.parent.GetComponent<AttackData>().fStrongDam);
//                 }
//             }
//         }




//         else{ //air attacks
//             if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0){ //neutral movement
//                 if(Input.GetButtonDown("Fire2")){ //neutral air
//                     //Attack(gameObject.transform.parent.GetComponent<AttackData>().nAirDam);
//                 }
//                 else if(Input.GetButtonDown("Fire3")){ //neutral special




//                 }
//             }




//             else if(Math.Abs(Input.GetAxis("Horizontal")) <= Math.Abs(Input.GetAxis("Vertical")) && Input.GetAxis("Vertical") > 0){ //up attacks
//                 if(Input.GetButtonDown("Fire2")){ //up air
//                     //Attack(gameObject.transform.parent.GetComponent<AttackData>().uAirDam);
//                 }
//                 else if(Input.GetButtonDown("Fire3")){ //up special
                   
//                 }
//             }




//             else if(Math.Abs(Input.GetAxis("Horizontal")) <= Math.Abs(Input.GetAxis("Vertical")) && Input.GetAxis("Vertical") < 0){ //down attacks
//                 if(Input.GetButtonDown("Fire2")){ //down air
//                     //Attack(gameObject.transform.parent.GetComponent<AttackData>().dAirDam);
//                 }
//                 else if(Input.GetButtonDown("Fire3")){ //down special
                   
//                 }
//             }




//             else if(Math.Abs(Input.GetAxis("Horizontal")) > Math.Abs(Input.GetAxis("Vertical"))){ //forward attacks
//                 if(Input.GetButtonDown("Fire3")){ //forward special
                   
//                 }
// /*                else if(Input.GetButtonDown("Fire2") && (faced direction = Input horizontal direction)){ //forward air
//                     Attack(gameObject.transform.parent.GetComponent<AttackData>().fAirDam);
//                 }
//                 else if(Input.GetButtonDown("Fire2") && (faced direction != Input horizontal direction)){ //back special
//                     Attack(gameObject.transform.parent.GetComponent<AttackData>().bAirDam);
//                 }*/
//             }      
//         }
    }


    //calls to apply knockback and damage to opponent characters
    void Attack(){
        //direction = transform.position;
        //direction.Normalize();
        //Debug.Log(direction);
        //animator.SetTrigger("Attack_Name");

        //am using both layers and tags.  can I do it with just one???

        //need to figure out how to reverse hitbox when character turns around (position and launch angle)
        //need to figure out how to resize the hitbox for each different attack (probably not hard tbh)
        //would be nice to write a script that allows me to drag and drop the hitbox for convenience
        //will have different types of hitboxes for different attacks
        //can get cute with it if I have enough time and have sweetspot and sourspot hitboxes for different attacks
        HashSet<GameObject> alreadyDamaged = new HashSet<GameObject>();
        Collider2D[] hitOpponent = Physics2D.OverlapCircleAll(attackZone.transform.position, attackRange, opponentLayers);
        playerTag = gameObject.tag;
        foreach(Collider2D opponent in hitOpponent){
            if (alreadyDamaged.Contains(opponent.gameObject)) continue; //makes sure attack only damages opponent once
            //if(opponent.tag != playerTag){ //keeps attacking player from taking damage/knockback
                bool isFacingRight = GetComponent<PlayerMovement>().isFacingRight;
                if(opponent.GetComponent<PlayerMovement>().isBlocking == true){ //does not do knockback to opponent if they are blocking
                    blocked = true; //this will knock back attacker after all damage calculations are run
                    opponent.GetComponent<Damage_Calculations>().TakeKnockback(isFacingRight, baseKnockback, scaledKnockback);
                }
                opponent.GetComponent<Damage_Calculations>().TakeDamage(attackDamage);
                alreadyDamaged.Add(opponent.gameObject);
            //}
        }
        if(blocked){ //knocks back user slightly after attacking blocked opponent
            
        }
        blocked = false;
    }

    //draws a hitbox visualizer in scene mode
    void OnDrawGizmosSelected()
    {
        if(attackZone != null){
            Gizmos.DrawWireSphere(attackZone.transform.position, attackRange);
        }
    }
        //sample damage calculator
        //knockbackUnits = hitpoints * baseDamage * scaleDamage
        //Vector 2 to launch fighter depending on what angle the hitbox sends at, what direction the opponent was facing, how much damage the player has currently
        //hitpoints will be reduced to 0 when the player dies
}