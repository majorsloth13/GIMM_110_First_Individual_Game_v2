using UnityEngine;

public class WallJump : Movement2D
{
    [Header("Wall Jump Settings")]
    public WallCheck wallCheck;         // Reference to your WallCheck component
    public float wallSlideSpeed = 2f;   // How fast the player slides down a wall
    public Vector2 wallJumpForce = new Vector2(14f, 18f); // Force applied when wall-jumping

    private bool isWallSliding;
    private bool isTouchingWall;
    private float lastTimeOnWall;        // For wall coyote time
    private bool wallJumped;

    protected override void Update()
    {
        base.Update(); // Runs the base Movement2D Update (handles input, ground check, coyote time, etc.)

        isTouchingWall = wallCheck != null && wallCheck.IsTouchingWall();

        HandleWallSlide();
        HandleWallJump();
    }

    private void HandleWallSlide()
    {
        // Detect wall + downward motion
        if (isTouchingWall && !isGrounded && rb.linearVelocity.y < 0)
        {
            isWallSliding = true;
            lastTimeOnWall = Time.time; // Refresh wall coyote time
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
        }
        else
        {
            // Stop sliding if off wall and coyote time has expired
            if (Time.time - lastTimeOnWall > coyoteTime)
                isWallSliding = false;
        }
    }

    private void HandleWallJump()
    {
        // Only allow wall jump if recently on a wall (within coyote time)
        if ((isWallSliding || Time.time - lastTimeOnWall <= coyoteTime) && Input.GetKeyDown(KeyCode.Space))
        {
            wallJumped = true;

            // Flip horizontal direction away from wall
            int direction = wallCheck.IsTouchingRightWall ? -1 : 1;

            // Apply the jump force away from the wall
            rb.linearVelocity = new Vector2(wallJumpForce.x * direction, wallJumpForce.y);
        }

        // Optional — stop wall sliding after jump
        if (wallJumped && rb.linearVelocity.y > 0)
            isWallSliding = false;
    }
}
x