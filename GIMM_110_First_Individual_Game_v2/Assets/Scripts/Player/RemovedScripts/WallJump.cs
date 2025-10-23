using UnityEngine;

public class WallJump : Movement2D
{
    [Header("Wall Jump Settings")]
    public Transform wallCheck;           // position to check for walls
    public LayerMask wallLayer;
    public float checkRadius = 0.2f;

    public float wallJumpHorizontalForce = 6f;   // horizontal push away from wall
    public float wallJumpVerticalForce = 16f;    // vertical jump height

    [Header("Wall Slide Settings")]
    public float wallSlideSpeed = 2f;    // sliding down the wall
    public float wallStickTime = 0.2f;   // how long the player sticks before sliding
    private float wallStickCounter;

    protected bool isTouchingWall;
    protected bool isWallSliding;
    private bool wallJumped;

    protected override void Update()
    {
        base.Update(); // keeps Movement2D functionality

        // Check wall contact
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, checkRadius, wallLayer);

        HandleWallSlide();
        HandleWallJump();
    }

    private void HandleWallSlide()
    {
        // Start sliding if touching wall and not grounded
        if (isTouchingWall && !isGrounded && rb.linearVelocity.y < 0)
        {
            isWallSliding = true;
            wallStickCounter = wallStickTime;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }

        // Reduce stick timer if holding against wall
        if (isWallSliding && wallStickCounter > 0)
        {
            wallStickCounter -= Time.deltaTime;
        }
    }

    private void HandleWallJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isWallSliding && wallStickCounter <= 0)
        {
            wallJumped = true;

            // Determine direction away from wall
            float direction = -Mathf.Sign(transform.localScale.x);

            // Apply horizontal and vertical force
            rb.linearVelocity = new Vector2(direction * wallJumpHorizontalForce, wallJumpVerticalForce);

            isWallSliding = false; // stop sliding after jump
        }

        // Reset wall jump flag when leaving wall
        if (!isTouchingWall)
            wallJumped = false;
    }

    public bool IsTouchingWall => isTouchingWall;
}
