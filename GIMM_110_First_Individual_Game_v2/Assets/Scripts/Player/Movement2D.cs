using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Movement2D : MonoBehaviour
{
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

    protected Rigidbody2D rb;             // Cached reference to Rigidbody2D
    private float movement;             // Horizontal input (-1, 0, 1)
    private float jumpBufferCounter;    // Timer to track buffered jump input
    private float lastTimeGrounded;     // Timestamp of the last time player was grounded
    private bool jumpRequested;         // Flag to trigger jump in FixedUpdate

    private MovingPlatforms currentPlatform; // Reference to current platform (if standing on one)
    public bool isGrounded { get; private set; } // Is the player currently grounded?

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        baseMoveSpeed = moveSpeed; // Store initial move speed for reset purposes

        // Improves physics smoothing and collision accuracy
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Automatically get the SpriteRenderer if not set
        sprite = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        // Read horizontal input each frame (-1 for left, 1 for right)
        movement = Input.GetAxisRaw("Horizontal");

        // Flip sprite based on direction
        UpdateSpriteDirection();

        // Check whether player is on the ground
        isGrounded = groundCheck.IsGrounded();

        // Handle jump input buffering
        if (Input.GetKeyDown(KeyCode.Space))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        // Track when the player was last grounded
        if (isGrounded)
            lastTimeGrounded = Time.time;

        // Allow jump if buffer is active and within coyote time
        if (jumpBufferCounter > 0f && Time.time - lastTimeGrounded <= coyoteTime)
        {
            jumpRequested = true;      // Set flag for FixedUpdate to process
            jumpBufferCounter = 0f;    // Reset jump buffer
        }
    }

    private void FixedUpdate()
    {
        // Calculate platform's velocity if standing on one
        Vector2 platformVelocity = Vector2.zero;
        if (isGrounded && currentPlatform != null)
            platformVelocity = currentPlatform.CurrentVelocity;

        // Apply horizontal movement plus any platform motion
        float targetX = movement * moveSpeed + platformVelocity.x;
        rb.linearVelocity = new Vector2(targetX, rb.linearVelocity.y);

        // Apply jump (only if jump was requested)
        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false; // Clear jump flag
        }
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
}
