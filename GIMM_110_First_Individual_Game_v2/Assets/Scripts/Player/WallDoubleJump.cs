using UnityEngine;

public class WallDoubleJump : Movement2D
{
    [Header("Double Jump Settings")]
    public int extraJumps = 1;
    private int jumpsLeft;

    [Header("Wall Jump Settings")]
    public Transform wallCheck;
    public LayerMask wallLayer;
    public float wallJumpHorizontalForce = 6f;
    public float wallJumpVerticalForce = 16f;
    public float checkRadius = 0.2f;

    [Header("Wall Slide Settings")]
    public float wallSlideSpeed = 2f;

    private bool isTouchingWall;
    private bool isWallSliding;

    protected override void Update()
    {
        base.Update();

        // Grounded resets jumps
        if (isGrounded)
            jumpsLeft = extraJumps;

        // Check for wall contact
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, checkRadius, wallLayer);

        HandleWallSlide();
        HandleWallJump();
        HandleDoubleJump();
    }

    private void HandleWallSlide()
    {
        // Apply wall slide if touching wall and not grounded
        if (isTouchingWall && !isGrounded)
        {
            isWallSliding = true;

            // Only clamp downward speed
            if (rb.linearVelocity.y < -wallSlideSpeed)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
        }
        else
        {
            isWallSliding = false;
        }
    }


    private void HandleWallJump()
    {
        // Wall jump occurs when touching wall and pressing jump
        if (Input.GetKeyDown(KeyCode.Space) && isWallSliding)
        {
            float direction = -Mathf.Sign(transform.localScale.x); // push away from wall
            rb.linearVelocity = new Vector2(direction * wallJumpHorizontalForce, wallJumpVerticalForce);
            isWallSliding = false;
            jumpsLeft = extraJumps; // reset double jump after wall jump
        }
    }

    private void HandleDoubleJump()
    {
        // Only allow double jump if in air and not touching wall
        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && !isWallSliding && jumpsLeft > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // reset vertical velocity
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpsLeft--;
        }
    }
}
