using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
    private int controllerID; // 0 = Keyboard, 1+ = Controllers

    [SerializeField] private float runSpeed = 5.0f;
    [SerializeField] private float jumpSpeed = 5.0f;
    [SerializeField] private int airJumpVal = 1;
    [SerializeField] private float airDashSpeed2 = 15.0f;
    [SerializeField] private float airDashTimer = 0;
    [SerializeField] private float airDashDuration = .15f;   
    [SerializeField] private string airDashDirection = "none";
    [SerializeField] private int airJump;
    [SerializeField] private int airDashVal = 1;
    private bool isBlocking = false;
    private bool isAttacking = false;

    float gravityScaleAtStart;

    bool isAlive = true; // Starts true because the player is alive

    Rigidbody2D playerCharacter;
    CapsuleCollider2D playerBodyCollider;
    Animator playerAnimator;
    BoxCollider2D playerFeetCollider;

    public void SetControllerID(int id)
    {
        controllerID = id;
    }

    // Start is called before the first frame update
    void Start()
    {
        // We grab from the component 
        playerCharacter = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();

        gravityScaleAtStart = playerCharacter.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;

        Run();
        FlipSprite();
        Jump();
        AirDash();
        Block();
        Attack1();
    }

    private void Run()
    {
        float hMovement = 0;
        if(!isBlocking)
        {
            if (controllerID == 0) // Keyboard Controls
            {
                hMovement = Input.GetAxisRaw("Horizontal"); // Default Unity Input
            }
            else // Controller Movement
            {
                hMovement = Input.GetAxisRaw("Joystick " + controllerID + " Horizontal");
            }

            Vector2 runVelocity = new Vector2(hMovement * runSpeed, playerCharacter.velocity.y);
            playerCharacter.velocity = runVelocity;

            bool isMoving = Mathf.Abs(playerCharacter.velocity.x) > Mathf.Epsilon;
            playerAnimator.SetBool("run", isMoving);  
        }

    }

     private void FlipSprite()
    {
        if (!isAttacking)
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
    }

    private void Jump()
    {
        bool isGrounded = playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (isGrounded) airJump = airJumpVal;

        bool jumpPressed = false;
        
        if (controllerID == 0) // Keyboard Jump
        {
            jumpPressed = Input.GetButtonDown("KeyboardJump");
        }
        else // Controller Jump
        {
            jumpPressed = Input.GetKeyDown("joystick " + controllerID + " button 0"); // A / X button
        }

        if (jumpPressed)
        {
            if (!isGrounded && airJump == 0) return;

            if (!isGrounded) airJump--;

            Vector2 jumpVelocity = new Vector2(0.0f, jumpSpeed);
            playerCharacter.velocity = jumpVelocity;
        }
    }

        private void Block()
    {
        bool blockPressed = false;
        bool blockLetgo = false;

        if (controllerID == 0) // Keyboard Block
        {
            blockPressed = Input.GetButtonDown("KeyBlock");
            blockLetgo = Input.GetButtonUp("KeyBlock");
        }
        else // Controller Block
        {
            blockPressed = Input.GetKeyDown("joystick " + controllerID + " button 1"); // A / X button
            blockLetgo = Input.GetKeyUp("joystick " + controllerID + " button 1");
        }

        if (blockPressed)
        {
            playerAnimator.SetTrigger("block");
            isBlocking = true;
        }
        if (blockLetgo)
        {
            playerAnimator.SetTrigger("blockDone");
            isBlocking = false;
        }
    }

    private void Attack1() 
    {
        bool attackPressed = false;
        bool attackLetgo = false;

        if (controllerID == 0) // Keyboard attack
        {
            attackPressed = Input.GetButtonDown("KeyAttack1");
            attackLetgo = Input.GetButtonUp("KeyAttack1");
        }
        else
        {
            attackPressed = Input.GetKeyDown("joystick " + controllerID + " button 2");
            attackLetgo = Input.GetKeyUp("joystick " + controllerID + " button 2");
        }

        if (attackPressed)
        {
            playerAnimator.SetTrigger("attack1");
            isAttacking = true;
        }
        if (attackLetgo)
        {
            isAttacking = false;
        }
        
    }

    private void AirDash()
    {
        bool isGrounded = playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (isGrounded) airDashVal = 1;

        bool airDashPressed = false;
        
        if (controllerID == 0) // Keyboard Air Dash
        {
            airDashPressed = Input.GetButtonDown("Fire1");
        }
        else // Controller Air Dash (Right Bumper)
        {
            airDashPressed = Input.GetKeyDown("joystick " + controllerID + " button 5"); // Right Trigger
        }

        if (airDashDirection == "none" && !isGrounded && airDashVal > 0)
        {
            float hInput = (controllerID == 0) ? Input.GetAxisRaw("Horizontal") : Input.GetAxisRaw("Joystick " + controllerID + " Horizontal");

            if (airDashPressed)
            {
                if (hInput > 0)
                {
                    airDashDirection = "right";
                }
                else if (hInput < 0)
                {
                    airDashDirection = "left";
                }

                if (airDashDirection != "none")
                {
                    airDashVal--;
                }
            }
        }

        if (airDashDirection != "none")
        {
            if (airDashTimer >= airDashDuration)
            {
                playerCharacter.velocity = Vector2.zero;
                airDashDirection = "none";
                airDashTimer = 0;
            }
            else
            {
                airDashTimer += Time.deltaTime;
                if (airDashDirection == "right")
                    playerCharacter.velocity = Vector2.right * airDashSpeed2;
                else if (airDashDirection == "left")
                    playerCharacter.velocity = Vector2.left * airDashSpeed2;
            }
        }
    }
}
