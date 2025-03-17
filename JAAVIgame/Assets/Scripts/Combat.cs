using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class Combat : MonoBehaviour
{
    Rigidbody2D playerCharacter;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;
    //public Animator animator;
    public Transform attackZone; //circular hitbox for now
    public float attackRange = 0.5f; //will be set in AttackData once I implement more than one attack
    public LayerMask opponentLayers;
    private string playerTag;
    //public bool inLag = false;


    //public bool isDead =  false;
    private static bool isGrounded;
    void Start()
    {
        //we grab from the component
        playerCharacter = GetComponent<Rigidbody2D>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();
    }

    void Update(){
        GetAttack(); //reads attack


    }


    public void GetAttack(){
        isGrounded = playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));


        if(isGrounded){ //grounded attacks
            if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0){ //stand still
                if(Input.GetButtonDown("Fire2")){ //jab
                    //only attack I am working on rn
                    Attack(gameObject.transform.GetComponent<AttackData>().jabDam);
                }
                else if(Input.GetButtonDown("Fire3")){ //neutral special


                }
            }


            else if(Math.Abs(Input.GetAxis("Horizontal")) <= Math.Abs(Input.GetAxis("Vertical")) && Input.GetAxis("Vertical") > 0){ //up attacks
                if(Input.GetButtonDown("Fire2")){ //up light
                    //Attack(gameObject.transform.parent.GetComponent<AttackData>().uLightDam);
                }
                else if(Input.GetButtonDown("Fire3")){ //up special
                   
                }
                else if(Input.GetButtonDown("Fire2") && Input.GetButtonDown("Fire3")){ //up strong
                    //Attack(gameObject.transform.parent.GetComponent<AttackData>().uStrongDam);
                }
            }


            else if(Math.Abs(Input.GetAxis("Horizontal")) <= Math.Abs(Input.GetAxis("Vertical")) && Input.GetAxis("Vertical") < 0){ //down attacks
                if(Input.GetButtonDown("Fire2")){ //down light
                    //Attack(gameObject.transform.parent.GetComponent<AttackData>().dLightDam);
                }
                else if(Input.GetButtonDown("Fire3")){ //down special
                   
                }
                else if(Input.GetButtonDown("Fire2") && Input.GetButtonDown("Fire3")){ //down strong
                    //Attack(gameObject.transform.parent.GetComponent<AttackData>().dStrongDam);
                }
            }


            else if(Math.Abs(Input.GetAxis("Horizontal")) > Math.Abs(Input.GetAxis("Vertical"))){ //forward attacks
                if(Input.GetButtonDown("Fire2")){ //forward light
                    //Attack(gameObject.transform.parent.GetComponent<AttackData>().fLightDam);
                }
                else if(Input.GetButtonDown("Fire3")){ //forward special
                   
                }
                else if(Input.GetButtonDown("Fire2") && Input.GetButtonDown("Fire3")){ //forward strong
                    //Attack(gameObject.transform.parent.GetComponent<AttackData>().fStrongDam);
                }
            }
        }


        else{ //air attacks
            if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0){ //neutral movement
                if(Input.GetButtonDown("Fire2")){ //neutral air
                    //Attack(gameObject.transform.parent.GetComponent<AttackData>().nAirDam);
                }
                else if(Input.GetButtonDown("Fire3")){ //neutral special


                }
            }


            else if(Math.Abs(Input.GetAxis("Horizontal")) <= Math.Abs(Input.GetAxis("Vertical")) && Input.GetAxis("Vertical") > 0){ //up attacks
                if(Input.GetButtonDown("Fire2")){ //up air
                    //Attack(gameObject.transform.parent.GetComponent<AttackData>().uAirDam);
                }
                else if(Input.GetButtonDown("Fire3")){ //up special
                   
                }
            }


            else if(Math.Abs(Input.GetAxis("Horizontal")) <= Math.Abs(Input.GetAxis("Vertical")) && Input.GetAxis("Vertical") < 0){ //down attacks
                if(Input.GetButtonDown("Fire2")){ //down air
                    //Attack(gameObject.transform.parent.GetComponent<AttackData>().dAirDam);
                }
                else if(Input.GetButtonDown("Fire3")){ //down special
                   
                }
            }


            else if(Math.Abs(Input.GetAxis("Horizontal")) > Math.Abs(Input.GetAxis("Vertical"))){ //forward attacks
                if(Input.GetButtonDown("Fire3")){ //forward special
                   
                }
/*                else if(Input.GetButtonDown("Fire2") && (faced direction = Input horizontal direction)){ //forward air
                    Attack(gameObject.transform.parent.GetComponent<AttackData>().fAirDam);
                }
                else if(Input.GetButtonDown("Fire2") && (faced direction != Input horizontal direction)){ //back special
                    Attack(gameObject.transform.parent.GetComponent<AttackData>().bAirDam);
                }*/
            }            
        }
    }

    //calls to apply knockback and damage to opponent characters
    void Attack(int damage){
        //animator.SetTrigger("Attack_Name");

        //need to transition from layers to tags I think
        //need to figure out how to not deal damage to each collider individually (double damage)
        //need to figure out how to reverse hitbox when character turns around (position and launch angle)
        //need to figure out how to resize the hitbox for each different attack (probably not hard tbh)
        //would be nice to write a script that allows ne to drag and drop the hitbox for convenience
        //will have different types of hitboxes for different attacks
        //can get cute with it if I have enough time and have sweetspot and sourspot hitboxes for different attacks

        Collider2D[] hitOpponent = Physics2D.OverlapCircleAll(attackZone.position, attackRange, opponentLayers);
        playerTag = gameObject.tag;
        foreach(Collider2D opponent in hitOpponent){
            if(opponent.tag != playerTag){ //keeps attacking player from taking damage/knockback
                opponent.GetComponent<Damage_Calculations>().TakeKnockback(); //still need to work on
                opponent.GetComponent<Damage_Calculations>().TakeDamage(damage);
            }
        }
    }

    //draws a hitbox visualizer in scene mode
    void OnDrawGizmosSelected()
    {
        if(attackZone != null){
            Gizmos.DrawWireSphere(attackZone.position, attackRange);
        }
    }

    public void calculateLag(){


    }
        //sample damage calculator
        //knockbackUnits = hitpoints * baseDamage * scaleDamage
        //Vector 2 to launch fighter depending on what angle the hitbox sends at, what direction the opponent was facing, how much damage the player has currently
        //hitpoints will be reduced to 0 when the player dies
}

