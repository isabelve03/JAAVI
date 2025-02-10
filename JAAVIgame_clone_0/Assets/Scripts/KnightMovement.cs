using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMovement : MonoBehaviour 
{
//All three variables below are placeholders.  They will be replaced by values from each character's personal attributes.
    [SerializeField] private float runSpeed = 5.0f;
    [SerializeField] private float jumpSpeed = 5.0f;
    [SerializeField] private int airJumpVal = 1;
    [SerializeField] private float airDashSpeed2 = 15.0f;
    [SerializeField] private float airDashTimer = 0;
    [SerializeField] private float airDashDuration = .15f;   
    [SerializeField] private string airDashDirection = "none";
    [SerializeField] private int airDashVal = 1;
    [SerializeField] private int airJump;

    float gravityScaleAtStart;

    bool isAlive = true; //Starts true because the player is alive

    Rigidbody2D playerCharacter;
    CapsuleCollider2D playerBodyCollider;
    Animator playerAnimator;
    BoxCollider2D playerFeetCollider;

    // Start is called before the first frame update
    void Start()
    {
        //we grab from the component 
        playerCharacter = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();

        gravityScaleAtStart = playerCharacter.gravityScale;
    }

    // Update is called once per frame
    void Update()
        {
        if(!isAlive)
        {
            return;
        }
        Health();
        Run();
        FlipSprite();
        Jump();
        AirDash();
       // Climb();
       // Die();
    }

    private void Health()
    {

    }

    private void Run()
    {
        // Value between -1 to +1
        float hMovement = Input.GetAxis("Horizontal");
        Vector3 runVelocity = new Vector2(hMovement*runSpeed, playerCharacter.velocity.y);

        bool hSpeed = Mathf.Abs(playerCharacter.velocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("run", hSpeed);
        
        playerCharacter.velocity = runVelocity;

    }

    private void FlipSprite()
    {
        // Lets characters use back aerial attacks
        // Want to add a certain amount of frames after a jump where character can reverse direction even in the air
        if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        // If the player is moving horizontally
        bool hMovement = Mathf.Abs(playerCharacter.velocity.x) > Mathf.Epsilon;

        if (hMovement)
        {
            // xScale multiplies against the sign of x to ensure that the scale within transform of the sprite is upheld
            float xScale = Mathf.Abs(playerCharacter.transform.localScale.x);
            
            // Reverse the current direction (scale) of the X-Axis
            transform.localScale = new Vector2(Mathf.Sign(playerCharacter.velocity.x)*xScale, playerCharacter.transform.localScale.y);
        }
    }

    private void Jump()
    {
        bool isGrounded = playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if(isGrounded){
            airJump = airJumpVal;
        }
        if(Input.GetButtonDown("Jump"))
        {
            if (!isGrounded)
            {
                if(airJump == 0)
                {
                    // Will stop this function unless true
                    return;
                }
                else
                {
                    airJump--;
                }
            }    
            // Get new Y velocity based on a controllable variable
            Vector2 jumpVelocity = new Vector2(0.0f, jumpSpeed);
            playerCharacter.velocity = jumpVelocity;
            //playerAnimator.SetTrigger("jump");
            //AudioSource.PlayClipAtPoint(jumpSFX, Camera.main.transform.position);
        }
        return;
    }

    private void AirDash() {
        bool isGrounded = playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if(airDashDirection == "none" && isGrounded == false && airDashVal > 0) {
            //picks direction to air dash in
            if(Input.GetButtonDown("Fire1") && Input.GetAxis("Horizontal") > 0) {
                airDashDirection = "right";
                airDashVal--;
            }
            else if(Input.GetButtonDown("Fire1") && Input.GetAxis("Horizontal") < 0) {
                airDashDirection = "left";
                airDashVal--;
            }
            else {
                airDashDirection = "none";
            }            
        }

        if(airDashDirection != "none") {
            //ends air dash
            if (airDashTimer >= airDashDuration){
                playerCharacter.velocity = Vector2.zero;
                airDashDirection = "none";
                airDashTimer = 0;
            }
            //increases horizontal dash movement
            else{
                airDashTimer += Time.deltaTime;
                if (airDashDirection == "right") {
                    playerCharacter.velocity = Vector2.right * airDashSpeed2;
                }
                else if(airDashDirection == "left") {
                    playerCharacter.velocity = Vector2.left * airDashSpeed2;
                }
            }
        }
        if(playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            airDashVal = 1;
        }
        return;
    }



    //this is to climb up ladders for example which i dont think we need
    // private void Climb()
    // {
    //     if(!playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
    //     {
    //         // Will stop this function unless true
    //         playerAnimator.SetBool("climb", false);
    //         playerCharacter.gravityScale = gravityScaleAtStart;
    //         return;
    //     }

    //     //"Vertical from Input Axis"
    //     float vMovement = Input.GetAxis("Vertical");

    //     // x needs to remain the same and we need to change y
    //     Vector2 climbVelocity = new Vector2(playerCharacter.velocity.x, vMovement * climbSpeed);
    //     playerCharacter.velocity = climbVelocity;

    //     playerCharacter.gravityScale = 0.0f;

    //     bool vSpeed = Mathf.Abs(playerCharacter.velocity.y) > Mathf.Epsilon;
    //     playerAnimator.SetBool("climb", vSpeed);

    // }

    // check for collision layer for jump / wall climb
    //this was from my old code but we dont need this yet.

    // private void Die()
    // {
    //     if(playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
    //     {
    //         isAlive = false;
    //         playerAnimator.SetTrigger("die");
    //         GetComponent<Rigidbody2D>().velocity = deathSeq;

    //         FindAnyObjectByType<GameSession>().ProcessPlayerDeath();

    //        AudioSource.PlayClipAtPoint(dieSFX, Camera.main.transform.position);

    //     }
    // }

}