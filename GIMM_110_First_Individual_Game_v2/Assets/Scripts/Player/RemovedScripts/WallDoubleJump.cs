using UnityEngine;

/// <summary>
/// Adds double jump and wall jump functionality to Movement2D.
/// Uses a single raycast-based WallCheck.
/// </summary>
public class WallDoubleJump : Movement2D
{
    /*[Header("Jump Settings")]
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
        //prevSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();

        if (wallJumping)
        {
            wallJumpLockCounter -= Time.deltaTime;
            if (wallJumpLockCounter <= 0)
                wallJumping = false;
        }

        CheckWall();
        HandleWallSlide();
        HandleExtraJump();
    }

    /// <summary>
    /// Checks both sides for walls using raycasts.
    /// </summary>
    private void CheckWall()
    {
        if (wallJumping)
        {
            // Don't check during lock
            isTouchingWall = isTouchingLeftWall = isTouchingRightWall = false;
            return;
        }

        // Check left and right separately
        RaycastHit2D leftHit = Physics2D.Raycast(wallCheck.position, Vector2.left, wallCheckDistance, wallLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, wallLayer);

        isTouchingLeftWall = leftHit.collider != null;
        isTouchingRightWall = rightHit.collider != null;
        isTouchingWall = isTouchingLeftWall || isTouchingRightWall;

        Debug.DrawRay(wallCheck.position, Vector2.left * wallCheckDistance, isTouchingLeftWall ? Color.green : Color.red);
        Debug.DrawRay(wallCheck.position, Vector2.right * wallCheckDistance, isTouchingRightWall ? Color.green : Color.red);
    }

    float prevSpeed;

    /// <summary>
    /// Handles wall sliding and jumping away from walls.
    /// </summary>
    private void HandleWallSlide()
    {
        if (isTouchingWall && !isGrounded && rb.linearVelocity.y < 0f)
        {
            print("wall sliding");
            isWallSliding = true;
            //prevSpeed = movement;
            //moveSpeed = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            print(rb);
            isWallSliding = false;
            //moveSpeed = prevSpeed;
        }

        // Jump away from whichever side we're touching
        if (isWallSliding && Input.GetKeyDown(KeyCode.Space))
        {
            float jumpDirection = 0f;

            if (isTouchingLeftWall)
                jumpDirection = 1f; // Jump right
            else if (isTouchingRightWall)
                jumpDirection = -1f; // Jump left

            // Apply jump velocity away from wall
            //rb.linearVelocity = new Vector2(wallJumpPush * jumpDirection, wallJumpForce);

            //Adds instant force in the direction of the opposite wall
            rb.AddForce(wallJumpPush * (isTouchingLeftWall ? Vector2.right : Vector2.left), ForceMode2D.Impulse);

            // Face away from wall
            if (jumpDirection != 0)
                sprite.flipX = jumpDirection < 0;

            // Lock re-sticking for a short time
            wallJumping = true;
            wallJumpLockCounter = wallJumpLockTime;

            // Reset double jumps
            jumpsLeft = extraJumps;
        }
    }

    /// <summary>
    /// Handles ground and double jump behavior.
    /// </summary>
    private void HandleExtraJump()
    {
        if (isGrounded)
            jumpsLeft = extraJumps;

        if (Input.GetKeyDown(KeyCode.Space) && jumpsLeft > 0 && !isGrounded && !isWallSliding)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpsLeft--;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (wallCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * wallCheckDistance);
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.left * wallCheckDistance);
    }*/
}
