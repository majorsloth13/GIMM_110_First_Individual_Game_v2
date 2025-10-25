using UnityEngine;

public class DoubleJump : WallJump
{
    /*[Header("Double Jump Settings")]
    public int extraJumps = 1;

    private int jumpsLeft;

    protected override void Update()
    {
        base.Update(); // Movement2D + WallJump behavior

        // Reset jumps when grounded
        if (isGrounded)
            jumpsLeft = extraJumps;

        // Handle mid-air extra jump
        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && !isTouchingWall && jumpsLeft > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpsLeft--;
        }
    }*/
}
