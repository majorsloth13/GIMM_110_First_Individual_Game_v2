using UnityEngine;

public class DoubleJump : Movement2D
{
    [Header("Double Jump Settings")]
    public int extraJumps = 1;      // Number of extra jumps available (1 = double jump)

    [Header("Wall Check")]
    public WallCheck wallCheck;

    private int jumpsRemaining;

    protected override void Update()
    {
        // Run base movement and jump input logic
        base.Update();

        // Initialize references if needed
        if (wallCheck == null)
            wallCheck = GetComponent<WallCheck>();

        // Reset extra jumps when grounded or touching wall
        if (isGrounded || (wallCheck != null && wallCheck.IsTouchingWall))
        {
            jumpsRemaining = extraJumps;
        }

        // Handle second jump input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryDoubleJump();
        }
    }

    private void TryDoubleJump()
    {
        // Allow extra jump only if not grounded
        if (!isGrounded && jumpsRemaining > 0)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpsRemaining--;

            // Optional feedback
            Debug.Log("Double Jump! Jumps left: " + jumpsRemaining);
        }
    }
}
