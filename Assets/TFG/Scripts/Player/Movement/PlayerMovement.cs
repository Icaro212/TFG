using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

public class PlayerMovement : MonoBehaviour
{

    #region Variables
    //General Movement parameters
    public PlayerData data;
    public Transform playerTransform;
    private Rigidbody2D rb;
    private float movementInputDirection;
    private bool isFacingRight = true;  

    //Animation
    private enum MovementState { iddle, running, jumping, falling, wallsliding, holding, climbing }
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip[] walkClips;
    private float timerSound = 0.0f;
    private float clipPlayingDuration;

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
    public TrailRenderer trailRenderer { set; get; }
    public bool isImpulsePointAct { set; get; }
    public bool isSpringActive { set; get; }
    public bool isWallClimbingActive { set; get; }
    public bool isWallClimbingHoriActive { set; get; }
    public bool isFanActivated { set; get; }
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
        data.floorInXAixs = true;
        data.JumpPositive = true;
        data.orientation = new Vector2(1, 0);
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
        movementInputDirection =  data.floorInXAixs ?  Input.GetAxisRaw("Horizontal") : Input.GetAxisRaw("Vertical") ;
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
            int signWallJumpY = data.JumpPositive ? 1 : -1;
            Vector2 force = data.floorInXAixs ? new Vector2(data.wallJumpVector.x * currentWallJumpDirection, data.wallJumpVector.y * signWallJumpY) : new Vector2(data.wallJumpVector.y * signWallJumpY, data.wallJumpVector.x * currentWallJumpDirection);
            rb.AddForce(force, ForceMode2D.Impulse);
            SoundFXManager.instance.PlaySoundFXClip(jumpClip, transform, 1f);
            Flip();
        }
        else if ((isGrounded || data.LastOnGroundTime > 0) && !isFanActivated)
        {
            int signJump = data.JumpPositive ? 1 : -1;
            Vector2 jumpVector = data.floorInXAixs ? Vector2.up * signJump : Vector2.right * signJump;
            rb.AddForce(jumpVector * data.jumpForce, ForceMode2D.Impulse);
            SoundFXManager.instance.PlaySoundFXClip(jumpClip, transform, 1f);
        }
    }

    private void Run(float interpolVal)
    {

        float targetSpeed = isGrounded ? movementInputDirection * data.maxSpeed : movementInputDirection * data.maxSpeedAir;
        if (targetSpeed.Equals(0) && !isWallJumping && !isSpringActive && !isFanActivated)
        {
            Vector2 auxVector = data.floorInXAixs ? new Vector2(0, rb.velocity.y) : new Vector2(rb.velocity.x, 0);
            rb.velocity = auxVector;
        }
        else
        {
            targetSpeed = data.floorInXAixs ? Mathf.Lerp(rb.velocity.x, targetSpeed, interpolVal) : Mathf.Lerp(rb.velocity.y, targetSpeed, interpolVal);
            float accelVal;
            if (data.LastOnGroundTime > 0)
            {
                accelVal = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccelAmount : data.runDeccelAmount;
            }
            else
            {
                accelVal = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccelAmount * data.accelInAir : data.runDeccelAmount * data.deccelInAir;
            }
            float speedDif = data.floorInXAixs ? targetSpeed - rb.velocity.x : targetSpeed - rb.velocity.y;
            float movement = speedDif * accelVal;
            Vector2 forceMovement = data.floorInXAixs ? movement * Vector2.right : movement * Vector2.up;
            rb.AddForce(forceMovement, ForceMode2D.Force);
        }

        if (interpolVal == 1 && targetSpeed != 0 && timerSound > clipPlayingDuration && isGrounded) //Wallking Sound
        {
            int rand = Random.Range(0, walkClips.Length);
            AudioClip clipAux = walkClips[rand];
            clipPlayingDuration = clipAux.length;
            SoundFXManager.instance.PlaySoundFXClip(clipAux, transform, 1f);
            timerSound = 0.0f;
        }

    }

    private void WallSlide()
    {
        Vector2 wallDirection = data.JumpPositive ? (data.floorInXAixs ? Vector2.down : Vector2.left) : (data.floorInXAixs ? Vector2.up : Vector2.right);
        Vector2 currentVelocity = rb.velocity;
        float speedDiff = Vector2.Dot(currentVelocity, wallDirection) - data.wallSlideSpeed;
        float movement = speedDiff * data.wallSlideAccel;
        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDiff) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDiff) * (1 / Time.fixedDeltaTime));
        rb.AddForce(movement * wallDirection);
    }

    private void ApplyMovement()
    {
        if (isWallJumping)
        {
            Run(0.01f);
        }
        else if(isSpringActive || isFanActivated)
        {
            Run(0.045f);
        }
        else
        {
            Run(1);
        }


        int jumpingDirection = data.JumpPositive ? 1 : -1;
        bool velocityWallActive = data.floorInXAixs ? rb.velocity.y * jumpingDirection < 0 && Mathf.Abs(rb.velocity.y) <= data.wallSlideSpeed : rb.velocity.x * jumpingDirection < 0 && Mathf.Abs(rb.velocity.x) <= data.wallSlideSpeed;
        if (isTouchingWall && !isGrounded && velocityWallActive) //Wallslide
        {
            WallSlide();
        }
    }
    #endregion

    //** Checks **
    #region Checks
    private void CheckSurrondings()
    {
        // Returns whether or not the player is grouned or it´s touching the wall
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
        int direction = isFacingRight ? 1 : -1;
        Vector2 directionVector = data.floorInXAixs ? Vector2.right * direction : Vector2.up * direction;
        isTouchingWall = Physics2D.Raycast(wallCheck.position, directionVector, data.wallCheckDistance, data.layerMask);

        //Updates Player Orientation
        data.orientation = directionVector;

    }

    private void GravityConditions()
    {
        int directionFalling = data.JumpPositive ? 1 : -1;
        float velocityFall = data.floorInXAixs ? rb.velocity.y * directionFalling : rb.velocity.x * directionFalling;

        if (isTouchingWall && !isGrounded && velocityFall < 0) //If it´s wallSliding 
        {
            SetGravity(0);
        }
        else if (isImpulsePointAct) //Impulse  
        {
            SetGravity(0);
        }
        else if (isFanActivated)
        {
            SetGravity(data.gravityScale);
        }
        else if (isSpringActive)
        {
            SetGravity(data.gravityScale * data.jumpHangGravityMul);
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
        }
        else if ((!isGrounded || isWallJumping || velocityFall < 0) && Mathf.Abs(velocityFall) < data.jumpHangTimeThreshold) //Jump is reaching its apex and we want to stay a little there
        {
            SetGravity(data.gravityScale * data.jumpHangGravityMul);
        }
        else if (velocityFall < 0) //Normal Fall
        {
            SetGravity(data.gravityScale * data.fallGravityMult);
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
        MovementState state;
        float jumpingVelocitySign = data.JumpPositive ? 1 : -1;
        float jumpingVelocity = data.floorInXAixs ? rb.velocity.y * jumpingVelocitySign : rb.velocity.x * jumpingVelocitySign;

        if(movementInputDirection != 0)
        {
            state = MovementState.running;
        }
        else
        {
            state = MovementState.iddle;
        }


        if(jumpingVelocity > .1f)
        {
            state = MovementState.jumping;
        }
        else if(jumpingVelocity < -.1f)
        {
            state = MovementState.falling;
        }

        if(isTouchingWall && jumpingVelocity < -.1f && !isGrounded)
        {
            state = MovementState.wallsliding;
        }

        if((isWallClimbingActive || isWallClimbingHoriActive) && jumpingVelocity != 0)
        {
            state = MovementState.climbing;
        }
        else if (isWallClimbingActive || isWallClimbingHoriActive)
        {
            state = MovementState.holding;
        }
        data.anim.SetInteger("state", (int) state);
    }
    public void Flip()
    {
        if (!isWallClimbingHoriActive)
        {
            isFacingRight = !isFacingRight;
            data.wallJumpDirection = -data.wallJumpDirection;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }

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
        timerSound += Time.deltaTime;
    }
    #endregion
}