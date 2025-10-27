using System.Collections;
using UnityEngine;

public class DashController : MonoBehaviour
{
    private Movement2D move;            // Reference to Movement2D
    private Rigidbody2D rb;             // Reference to Rigidbody2D
    private bool isDashing = false;     // True while player is mid-dash
    private bool dashOnCooldown = false;// For ground dash cooldown
    private bool dashUsedInAir = false; // Prevents multiple air dashes until grounded

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeedBoost = 30f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 2f;

    private void Start()
    {
        move = GetComponent<Movement2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Input for dash (Right Mouse Button)
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            bool canDash =
                !isDashing &&                           // not currently dashing
                (!dashOnCooldown ||                     // not cooling down
                (!move.isGrounded && !dashUsedInAir));  // OR in air but hasn’t dashed yet

            if (canDash)
                StartCoroutine(StartDash());
        }

        // Reset air dash when touching ground
        if (move.isGrounded && dashUsedInAir)
        {
            dashUsedInAir = false;
            Debug.Log("Air dash reset on landing");
        }
    }

    private IEnumerator StartDash()
    {
        isDashing = true;

        // Determine dash direction (based on facing direction)
        float dashDir = move.sprite.flipX ? 1f : -1f;

        // Store original gravity
        float originalGravity = rb.gravityScale;

        // Temporarily disable gravity during dash
        rb.gravityScale = 0f;

        // Apply linear velocity for instant dash
        rb.linearVelocity = new Vector2(dashDir * dashSpeedBoost, 0f);
        Debug.Log("Dash started (" + (move.isGrounded ? "ground" : "air") + ")");

        // Optional: Dash animation
        Animator anim = move.GetComponent<Animator>();
        if (anim != null)
            anim.SetTrigger("dash");

        // Wait for dash duration
        yield return new WaitForSeconds(dashDuration);

        // Stop dash movement but keep Y velocity consistent
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        rb.gravityScale = originalGravity;

        // Handle dash restrictions
        if (move.isGrounded)
        {
            dashOnCooldown = true;
            Debug.Log("Ground dash cooldown started");
            yield return new WaitForSeconds(dashCooldown);
            dashOnCooldown = false;
            Debug.Log("Dash cooldown over");
        }
        else
        {
            dashUsedInAir = true;
            Debug.Log("Air dash used — must land to reset");
        }

        isDashing = false;
    }
}
