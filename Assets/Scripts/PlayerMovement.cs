using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //General Movement parameters
    private Rigidbody2D rb;
    private float movementInputDirection;
    private bool isFacingRight = true;
    private bool isWalking;
    public float movementSpeed = 9f;

    //Jump parameters
    private bool isGrounded;
    private bool canJump;
    public float jumpForce = 14.0f;
    public float groundCheckRadius;
    public Transform groundCheck;

    //Wall Sliding parameters
    private bool isTouchingWall;
    private bool isWallSliding;
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public Transform wallCheck;

    //Wall Jump parameters
    private bool canWallJump;
    private bool isWallJumping = false;
    private float wallJumpTime = 1.0f;
    private float lastWallJump;
    private float wallJumpDirection = -1;
    private float currentWallJumpDirection;
    [Range(0f, 1)] private float deccelAirJump;

    //Dash parameters
    private bool canDash;
    private bool isDashing;
    public float fullDashForce = 14f;
    public float partialDashForce = 9.89f;
    public int numberOfDashes = 1;
    public float DashTime;

    //Assits parameters
    public float coyoteTime = 0.15f; // Grace period after falling off a platform, where you can still jump
    public float LastOnGroundTime;

    //Other
    private Animator anim;    
    public LayerMask layerMask;


    //** Default Methods Unity **

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame. We will used to control all the checks necesary for the enviroment.
    private void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckMovement();
        TimeCounter();
    }
    //This calcultaes before the frame happends meaning that the physics of the in bluild engine will be more precise
    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurrondings();
    }


    // ** Controls **
    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            Dash();
        }
    }
    private void Jump()
    {
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else if (canWallJump)
        {
            currentWallJumpDirection = wallJumpDirection;
            isWallJumping = true;
            lastWallJump = wallJumpTime;
            deccelAirJump = 1;
            rb.velocity = new Vector2(movementSpeed * currentWallJumpDirection * deccelAirJump, jumpForce);
            Flip();
        }
        
    }

    private void Dash()
    {
        if (canDash)
        {
            float dashInputDirectionVertical = Input.GetAxisRaw("Vertical");
            float dashInputDirrHorizontalManual = movementInputDirection;
            isDashing = true;
            if(dashInputDirectionVertical != 0)
            {
                if(dashInputDirrHorizontalManual != 0)
                {
                    rb.velocity = new Vector2(partialDashForce * dashInputDirrHorizontalManual, partialDashForce * dashInputDirectionVertical);
                    DashTime = 0.5f;
                }
                else
                {
                    rb.velocity = new Vector2(0, fullDashForce * dashInputDirectionVertical);
                    DashTime = 0.5f;
                }

            }
            else
            {

                float dashInputDirrHorizontalAuto = -1;
                if (isFacingRight) { dashInputDirrHorizontalAuto = 1; }
                rb.velocity = new Vector2(fullDashForce * dashInputDirrHorizontalAuto, 0);
                DashTime = 0.5f;

            }

        }
    }

    private void CheckMovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }
        isWalking = rb.velocity.x != 0;

    }
    private void ApplyMovement()
    {
        if (isWallJumping && lastWallJump > 0)
        {
            rb.velocity = new Vector2(movementSpeed * currentWallJumpDirection *deccelAirJump + movementSpeed * movementInputDirection, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(movementInputDirection * movementSpeed, rb.velocity.y);
        }
        
        if (isWallSliding)
        {
            if (rb.velocity.y <= wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }

        if (isDashing)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
    }

    //** Checks **
    private void CheckSurrondings()
    {
        // Returns whether or not the player is grouned or it´s touching the wall
        bool auxGroundCollision = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, layerMask);
        if (auxGroundCollision)
        {
            isGrounded = true;
            LastOnGroundTime = coyoteTime;
        }
        else
        {
            isGrounded = false;
        }
        
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, layerMask);
        if(isTouchingWall && auxGroundCollision)
        {
            isWallJumping = false;
        }
    }

    private void CheckMovement()
    {
        canJump = isGrounded || (LastOnGroundTime > 0);
        canWallJump = isTouchingWall && !isGrounded;
        isWallSliding = isTouchingWall && !isGrounded && rb.velocity.y < 0;
        canDash = numberOfDashes > 0;
        isDashing = isDashing && (DashTime > 0);

    }

    //** Other **
    private void UpdateAnimations()
    {
        anim.SetBool("isWalking", isWalking);
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
        wallJumpDirection = -wallJumpDirection;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }

    private void TimeCounter()
    {
        LastOnGroundTime -= Time.deltaTime;
        lastWallJump -= Time.deltaTime;
        DashTime -= Time.deltaTime;
        if(deccelAirJump > 0)
        {
            deccelAirJump = deccelAirJump - 0.004f;
        }
        
    }
}
