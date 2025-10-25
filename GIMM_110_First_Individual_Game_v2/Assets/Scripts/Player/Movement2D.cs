using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.LightAnchor;

public class Movement2D : MonoBehaviour
{
    private enum WallSide { None, Left, Right }
    [Header("Movement Variables")]
    public float moveSpeed = 10f;        // Base horizontal movement speed
    private float baseMoveSpeed;         // Stores original move speed (for resetting after dash)
    public float jumpForce = 18f;        // Upward force applied during jump

    [Header("Jump Timing")]
    public float coyoteTime = 0.2f;      // Grace period after leaving the ground where jump is still allowed
    public float jumpBufferTime = 0.2f;  // Allows jump input to be "buffered" slightly before landing

    [Header("Ground Check")]
    public GroundCheck groundCheck;     // Reference to custom ground check component

    [Header("Sprite")]
    public SpriteRenderer sprite;       // SpriteRenderer for flipping player based on direction

    public Rigidbody2D rb;             // Cached reference to Rigidbody2D
    private float movement;             // Horizontal input (-1, 0, 1)
    private float jumpBufferCounter;    // Timer to track buffered jump input
    private float lastTimeGrounded;     // Timestamp of the last time player was grounded
    private bool jumpRequested;         // Flag to trigger jump in FixedUpdate

    private MovingPlatforms currentPlatform; // Reference to current platform (if standing on one)
    public bool isGrounded { get; private set; } // Is the player currently grounded?


    [Header("Jump Settings")]
    public int extraJumps = 1;
    private int jumpsLeft;

    [Header("Wall Jump Settings")]
    public float wallCheckDistance = 1f;
    public float wallSlideSpeed = 2f;
    public float wallJumpForce = 15f;
    public float wallJumpPush = 10f;
    public float wallJumpLockTime = 0.15f; // Prevents instant re-stick

    [Header("References")]
    public Transform wallCheck;
    public LayerMask wallLayer;

    private bool isTouchingWall;
    private bool isTouchingLeftWall;
    private bool isTouchingRightWall;
    private bool isWallSliding;
    private bool wallJumping;
    private float wallJumpLockCounter;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        baseMoveSpeed = moveSpeed; // Store initial move speed for reset purposes

        // Improves physics smoothing and collision accuracy
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Automatically get the SpriteRenderer if not set
        sprite = GetComponent<SpriteRenderer>();
        jumpsLeft = extraJumps;
    }

    bool jumpBuffering;
    private void Update()
    {
        // Read horizontal input each frame (-1 for left, 1 for right)
        movement = Input.GetAxisRaw("Horizontal");

        // Flip sprite based on direction
        UpdateSpriteDirection();

        // Check whether player is on the ground
        isGrounded = groundCheck.IsGrounded();

        if (isGrounded)
            jumpsLeft = extraJumps;

        WallSide side = GetWallJump();
        if (side != WallSide.None && !isGrounded && !jumpBuffering)
        {
            print("wall sliding");
            moveSpeed = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlideSpeed, float.MaxValue));

            if (Input.GetKeyDown(KeyCode.Space))
            {
                int preDirection = 0;
                if (side == WallSide.Left)
                    preDirection = 1;
                else
                    preDirection = -1;

                if (movement == preDirection && movement != 0f)
                {
                    print("jump to other wall");
                    StartCoroutine(JumpBuffer());
                    rb.linearVelocityY = 0f;
                    rb.AddForce(wallJumpPush /** (side == WallSide.Left ? Vector2.right : Vector2.left)*/ * Vector2.up, ForceMode2D.Impulse);

                    // Face away from wall
                    sprite.flipX = preDirection != 1;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && jumpsLeft > 0 && !jumpBuffering)
        {
            StartCoroutine(JumpBuffer());
            print("jump reg");
            moveSpeed = baseMoveSpeed;
            rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
            jumpsLeft--;
        }
        else
            moveSpeed = baseMoveSpeed;
    }

    private void FixedUpdate()
    {
        //= Calculate platform's velocity if standing on one
        Vector2 platformVelocity = Vector2.zero;
        if (isGrounded && currentPlatform != null)
            platformVelocity = currentPlatform.CurrentVelocity;

        // Apply horizontal movement plus any platform motion
        float targetX = movement * moveSpeed + platformVelocity.x;
        rb.linearVelocity = new Vector2(targetX, rb.linearVelocity.y);

        /*7 Apply jump (only if jump was requested)
        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false; // Clear jump flag
        }*/
    }

    // Detect if player lands on a moving platform
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("MovingPlatform"))
            currentPlatform = collision.collider.GetComponent<MovingPlatforms>();
    }

    // Detect if player leaves a moving platform
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("MovingPlatform"))
            currentPlatform = null;
    }

    // Flip the sprite left or right based on movement direction
    private void UpdateSpriteDirection()
    {
        if (movement > 0f)
            sprite.flipX = true;
        else if (movement < 0f)
            sprite.flipX = false;
    }

    // Used by Dash script to apply temporary speed boost
    public void ApplyDash(float boost)
    {
        Debug.Log("Dashed");
        moveSpeed += boost;
    }

    // Called after dash ends to restore base move speed
    public void ResetSpeed()
    {
        moveSpeed = baseMoveSpeed;
    }

    WallSide GetWallJump()
    {
        WallSide side = WallSide.None;
        if (Physics2D.Raycast(wallCheck.position, Vector2.left, wallCheckDistance, wallLayer).collider != null)
           side = WallSide.Left;
        else if (Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, wallLayer).collider != null)
            side = WallSide.Right;
        else
            side = WallSide.None;

        Debug.DrawRay(wallCheck.position, Vector2.left * wallCheckDistance, side == WallSide.Left ? Color.green : Color.red);
        Debug.DrawRay(wallCheck.position, Vector2.right * wallCheckDistance, side == WallSide.Right ? Color.green : Color.red);

        return side;
    }

    IEnumerator JumpBuffer()
    {
        jumpBuffering = true;
        yield return new WaitForSeconds(jumpBufferTime);
        jumpBuffering = false;
    }
}
