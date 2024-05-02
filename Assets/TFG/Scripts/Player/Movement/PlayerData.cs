using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName ="Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("General")]
    public float movementSpeed;
    public float maxSpeed;
    public float maxFallSpeed;
    public float runAccelAmount;
    public float runDeccelAmount;
    public float gravityScale;
    public float fallGravityMult;
    public float jumpHangGravityMul;

    [Space(5)]
    [Header("Jump")]
    public float jumpForce = 14.0f;
    public float groundCheckRadius;
    public float jumpHangTimeThreshold;
    public float maxSpeedAir;
    public float accelInAir;
    public float deccelInAir;

    [Space(5)]
    [Header("WallSlide")]
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float wallSlideAccel;

    [Space(5)]
    [Header("WallJump")]
    public float wallJumpDirection = 0;
    public Vector2 wallJumpVector;

    [Space(5)]
    [Header("Assits Parameters")]
    [Range(0.01f, 0.5f)] public float coyoteTime = 0.15f; // Grace period after falling off a platform, where you can still jump
    public float LastOnGroundTime;

    [Space(5)]
    [Header("Animation and Layer Mask")]
    public Animator anim;
    public LayerMask layerMask;

    [Space(5)]
    [Header("Rotation")]
    public bool floorInXAixs = true;
    public bool JumpPositive = true;
    public Vector2 orientation = new Vector2(1, 0);

}
