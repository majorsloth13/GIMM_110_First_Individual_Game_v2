using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Movement2D : MonoBehaviour
{
    [Header("Movement Variables")]
    public float moveSpeed = 10f;
    public float jumpForce = 18f;

    [Header("Jump Timing")]
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;

    [Header("Ground Check")]
    public GroundCheck groundCheck;

    [Header("Sprite")]
    public SpriteRenderer sprite;

    private Rigidbody2D rb;
    private float movement;
    private float jumpBufferCounter;
    private float lastTimeGrounded;
    private bool jumpRequested;

    private MovingPlatforms currentPlatform;
    public bool isGrounded { get; private set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        sprite = GetComponent<SpriteRenderer>();

    }

    private void Update()
    {
        movement = Input.GetAxisRaw("Horizontal");

        // Update sprite direction
        UpdateSpriteDirection();

        // Ground check
        isGrounded = groundCheck.IsGrounded();

        // Jump buffer
        if (Input.GetKeyDown(KeyCode.Space))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        // Coyote time
        if (isGrounded)
            lastTimeGrounded = Time.time;

        if (jumpBufferCounter > 0f && Time.time - lastTimeGrounded <= coyoteTime)
        {
            jumpRequested = true;
            jumpBufferCounter = 0f;
        }
    }

    private void FixedUpdate()
    {
        // Platform velocity
        Vector2 platformVelocity = Vector2.zero;
        if (isGrounded && currentPlatform != null)
            platformVelocity = currentPlatform.CurrentVelocity;

        // Apply horizontal movement + platform velocity
        float targetX = movement * moveSpeed + platformVelocity.x;
        rb.linearVelocity = new Vector2(targetX, rb.linearVelocity.y);

        // Apply jump
        if (jumpRequested)
        {//while fixing a platforming issue Chat changed and simplified my jump from the jump method to internally of the if statement using rb velocity instead of linearvelocity
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("MovingPlatform"))
            currentPlatform = collision.collider.GetComponent<MovingPlatforms>();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("MovingPlatform"))
            currentPlatform = null;
    }

    // -------------------------
    // Custom Methods
    // -------------------------
    private void UpdateSpriteDirection()
    {
        if (movement > 0f)
            sprite.flipX = true;
        else if (movement < 0f)
            sprite.flipX = false;
    }
}