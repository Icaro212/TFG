using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    #region Variables
    //General Movement parameters
    public PlayerData data;
    public Transform playerTransform;
    private Rigidbody2D rb;
    private float movementInputDirection;
    private bool isFacingRight = true;
    private bool isWalking;

    //Jump parameters
    public bool isGrounded { set; get; }
    private bool isJumpCut;
    public Transform groundCheck;

    //Wall Sliding parameters
    private bool isTouchingWall;
    public Transform wallCheck;

    //Wall Jump parameters
    private bool isWallJumping = false;
    private float currentWallJumpDirection;

    //Magic parameters
    public bool isImpulsePointAct { set; get; }
    public TrailRenderer trailRenderer { set; get; }
    public bool isSpringActive { set; get; }
    public bool isWallClimbingActive { set; get; }
    public bool isWallClimbingHoriActive { set; get; }
    #endregion

    private void Awake()
    {
        GameManager.instance.SearchForPlayer();
    }

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        data.anim = GetComponent<Animator>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame. We will used to control all the checks necesary for the enviroment.
    private void Update()
    {
        CheckInputs();
        CheckMomentDirectionAnimation();
        UpdateAnimations();
        TimeCounter();
    }
    //This calculates before the frame happends meaning that the physics of the in bluild engine will be more precise
    private void FixedUpdate()
    {
        GravityConditions();
        CheckSurrondings();
        if (!isImpulsePointAct)
        {
            ApplyMovement();
        }
    }

    //** Controls **
    #region Controls
    private void CheckInputs()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
            Jump();
        if (Input.GetButtonUp("Jump"))
            isJumpCut = true;

    }
    private void Jump()
    {
        if (isTouchingWall && !isGrounded)
        {
            currentWallJumpDirection = data.wallJumpDirection;
            isWallJumping = true;
            Vector2 force = new Vector2(data.wallJumpVector.x, data.wallJumpVector.y);
            force.x *= currentWallJumpDirection;
            rb.AddForce(force, ForceMode2D.Impulse);
            Flip();
        }else if (isGrounded || (data.LastOnGroundTime > 0))
        {
            rb.AddForce(Vector2.up * data.jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Run(float interpolVal)
    {
        
        float targetSpeed = isGrounded ?  movementInputDirection * data.maxSpeed : movementInputDirection * data.maxSpeedAir;
        if (targetSpeed.Equals(0) && !isWallJumping && !isSpringActive)
        {
            Vector2 auxVector = new Vector2(0, rb.velocity.y);
            rb.velocity = auxVector;
        }
        else
        {
            targetSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, interpolVal);

            float accelVal;
            if (data.LastOnGroundTime > 0)
            {
                accelVal = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccelAmount : data.runDeccelAmount;
            }
            else
            {
                accelVal = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccelAmount * data.accelInAir : data.runDeccelAmount * data.deccelInAir;
            }
            float speedDif = targetSpeed - rb.velocity.x;
            float movement = speedDif * accelVal;
            rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
        }

    }

    private void WallSlide()
    {
        float speedDiff = data.wallSlideSpeed - rb.velocity.y;
        float movement = speedDiff * data.wallSlideAccel;
        movement = Mathf.Clamp(movement, - Mathf.Abs(speedDiff) * (1/Time.fixedDeltaTime),  Mathf.Abs(speedDiff) * (1/Time.fixedDeltaTime));
        rb.AddForce(movement * Vector2.up);
    }

    private void ApplyMovement()
    {
        if (isWallJumping)
        {
            Run(0.01f);
        }
        else if(isSpringActive)
        {
            Run(0.025f);
        }
        else
        {
            Run(1);
        }

        if(isTouchingWall && !isGrounded && rb.velocity.y < 0 && rb.velocity.y <= data.wallSlideSpeed) //Wallslide
        {
            WallSlide();
        }
    }
    #endregion

    //** Checks **
    #region Checks
    private void CheckSurrondings()
    {
        // Returns whether or not the player is grouned or it�s touching the wall
        bool auxGroundCollision = Physics2D.OverlapCircle(groundCheck.position, data.groundCheckRadius, data.layerMask);
        if (auxGroundCollision)
        {
            isGrounded = true;
            isJumpCut = false;
            isWallJumping = false;
            isSpringActive = false;
            data.LastOnGroundTime = data.coyoteTime;
        }
        else
            isGrounded = false;

        // Updates the Wall Jump direction 
        data.wallJumpDirection = isFacingRight ? -1 : 1;

        // Returns whether or not the player is touching the wall
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
        isTouchingWall = Physics2D.Raycast(wallCheck.position, direction, data.wallCheckDistance, data.layerMask);       

    }

    private void GravityConditions()
    {
        if (isTouchingWall && !isGrounded && rb.velocity.y < 0 && rb.velocity.y <= data.wallSlideSpeed) //If it�s wallSliding 
        {
            SetGravity(0);
        }
        else if(isImpulsePointAct) //Impulse  
        {
            SetGravity(0);
        }
        else if (isSpringActive)
        {
            SetGravity(data.gravityScale);
        }
        else if (isWallClimbingActive)
        {
            SetGravity(0);
        }
        else if (isWallClimbingHoriActive)
        {
            SetGravity(0);
        }
        else if (isJumpCut) //Jump Button is release and thus the jump is cut
        {
            SetGravity(data.gravityScale * data.fallGravityMult);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -data.maxFallSpeed));
        }
        else if((!isGrounded || isWallJumping || rb.velocity.y < 0) && Mathf.Abs(rb.velocity.y) < data.jumpHangTimeThreshold) //Jump is reaching its apex and we want to stay a little there
        {
            SetGravity(data.gravityScale * data.jumpHangGravityMul);
        }
        else if (rb.velocity.y < 0) //Normal Fall
        {
            SetGravity(data.gravityScale * data.fallGravityMult);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -data.maxFallSpeed));
        }
        else //Default Case
        {
            SetGravity(data.gravityScale);
        }

    }

    private void SetGravity(float gravityScale)
    {
        rb.gravityScale = gravityScale;
    }
    #endregion

    //** Animation Relate Stuff **
    #region Animation Stuff
    private void UpdateAnimations()
    {
        data.anim.SetBool("isWalking", isWalking);
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        if (isWallClimbingHoriActive)
        {
            transform.Rotate(180.0f, 0.0f, 0.0f); 
        }
        else
        {
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
        data.wallJumpDirection = -data.wallJumpDirection;
    }
    private void CheckMomentDirectionAnimation()
    {
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }
        isWalking = movementInputDirection != 0;
    }

    private void Respawn()
    {
        GameManager.instance.Respawn();
    }
    #endregion


    //** Other **
    #region Other
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, data.groundCheckRadius);
        float direction = isFacingRight ? wallCheck.position.x + data.wallCheckDistance : wallCheck.position.x - data.wallCheckDistance;
        Gizmos.DrawLine(wallCheck.position, new Vector3(direction, wallCheck.position.y, wallCheck.position.z));
    }

    private void TimeCounter()
    {
        data.LastOnGroundTime -= Time.deltaTime;       
    }
    #endregion
}