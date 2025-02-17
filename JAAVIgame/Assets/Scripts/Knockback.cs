using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    Rigidbody2D playerCharacter;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;
    public int hitPoints = 0; //hitpoints start at 0 and increment up until character dies, then they are reset
    public bool inLag = false;

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
                    TakeDamage(gameObject.transform.parent.GetComponent<AttackData>().jabDam);
                }
                else if(Input.GetButtonDown("Fire3")){ //neutral special

                }
            }

            else if(Math.Abs(Input.GetAxis("Horizontal")) <= Math.Abs(Input.GetAxis("Vertical")) && Input.GetAxis("Vertical") > 0){ //up attacks
                if(Input.GetButtonDown("Fire2")){ //up light
                    TakeDamage(gameObject.transform.parent.GetComponent<AttackData>().uLightDam);
                }
                else if(Input.GetButtonDown("Fire3")){ //up special
                    
                }
                else if(Input.GetButtonDown("Fire2") && Input.GetButtonDown("Fire3")){ //up strong
                    TakeDamage(gameObject.transform.parent.GetComponent<AttackData>().uStrongDam);
                }
            }

            else if(Math.Abs(Input.GetAxis("Horizontal")) <= Math.Abs(Input.GetAxis("Vertical")) && Input.GetAxis("Vertical") < 0){ //down attacks
                if(Input.GetButtonDown("Fire2")){ //down light
                    TakeDamage(gameObject.transform.parent.GetComponent<AttackData>().dLightDam);
                }
                else if(Input.GetButtonDown("Fire3")){ //down special
                    
                }
                else if(Input.GetButtonDown("Fire2") && Input.GetButtonDown("Fire3")){ //down strong
                    TakeDamage(gameObject.transform.parent.GetComponent<AttackData>().dStrongDam);
                }
            }

            else if(Math.Abs(Input.GetAxis("Horizontal")) > Math.Abs(Input.GetAxis("Vertical"))){ //forward attacks
                if(Input.GetButtonDown("Fire2")){ //forward light
                    TakeDamage(gameObject.transform.parent.GetComponent<AttackData>().fLightDam);
                }
                else if(Input.GetButtonDown("Fire3")){ //forward special
                    
                }
                else if(Input.GetButtonDown("Fire2") && Input.GetButtonDown("Fire3")){ //forward strong
                    TakeDamage(gameObject.transform.parent.GetComponent<AttackData>().fStrongDam);
                }
            }
        }

        else{ //air attacks
            if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0){ //neutral movement
                if(Input.GetButtonDown("Fire2")){ //neutral air
                    TakeDamage(gameObject.transform.parent.GetComponent<AttackData>().nAirDam);
                }
                else if(Input.GetButtonDown("Fire3")){ //neutral special

                }
            }

            else if(Math.Abs(Input.GetAxis("Horizontal")) <= Math.Abs(Input.GetAxis("Vertical")) && Input.GetAxis("Vertical") > 0){ //up attacks
                if(Input.GetButtonDown("Fire2")){ //up air
                    TakeDamage(gameObject.transform.parent.GetComponent<AttackData>().uAirDam);
                }
                else if(Input.GetButtonDown("Fire3")){ //up special
                    
                }
            }

            else if(Math.Abs(Input.GetAxis("Horizontal")) <= Math.Abs(Input.GetAxis("Vertical")) && Input.GetAxis("Vertical") < 0){ //down attacks
                if(Input.GetButtonDown("Fire2")){ //down air
                    TakeDamage(gameObject.transform.parent.GetComponent<AttackData>().dAirDam);
                }
                else if(Input.GetButtonDown("Fire3")){ //down special
                    
                }
            }

            else if(Math.Abs(Input.GetAxis("Horizontal")) > Math.Abs(Input.GetAxis("Vertical"))){ //forward attacks
                if(Input.GetButtonDown("Fire3")){ //forward special
                    
                }
/*                else if(Input.GetButtonDown("Fire2") && (faced direction = Input horizontal direction)){ //forward air
                    TakeDamage(gameObject.transform.parent.GetComponent<AttackData>().fAirDam);
                }
                else if(Input.GetButtonDown("Fire2") && (faced direction != Input horizontal direction)){ //back special
                    TakeDamage(gameObject.transform.parent.GetComponent<AttackData>().bAirDam);
                }*/
            }            
        }
    }
    public void TakeKnockback(float baseKnockback, float scaleKnockback, float attackAngle){ //need to add condition to check direction player is facing when using attack

    }
    //damage is added to the hitpoint total AFTER knockback is calculated
    public void TakeDamage(int attackVal){
        hitPoints += attackVal;
    }

    public void calculateLag(){

    }
        //sample damage calculator
        //knockbackUnits = hitpoints * baseDamage * scaleDamage
        //Vector 2 to launch fighter depending on what angle the hitbox sends at, what direction the opponent was facing, how much damage the player has currently
        //hitpoints = hitpoints + baseDamage + 
        //hitpoints will be reduced to 0 when the player dies
}
