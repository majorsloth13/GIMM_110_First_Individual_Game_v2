using System.Collections;
using UnityEngine;

public class DashController : MonoBehaviour
{
    private Movement2D move;            // Reference to the Movement2D script
    private bool isDashing = false;     // Tracks whether the player is currently dashing

    [SerializeField] private float dashSpeedBoost = 30f;   // Amount of speed added during dash
    [SerializeField] private float dashDuration = 0.1f;    // Time the dash effect lasts
    [SerializeField] private float dashCooldown = 3f;      // Time before another dash can be triggered

    void Start()
    {
        // Get the Movement2D component attached to the same GameObject
        move = GetComponent<Movement2D>();
    }

    void Update()
    {
        // Listen for right mouse button press
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            // Only allow dash if not currently dashing or in cooldown
            if (!isDashing)
            {
                isDashing = true;
                StartCoroutine(StartDash()); // Begin dash coroutine
            }
        }
    }

    IEnumerator StartDash()
    {
        // Apply the speed boost from the dash
        move.ApplyDash(dashSpeedBoost);
        Debug.Log("Dashing...");

        // Wait for the duration of the dash
        yield return new WaitForSeconds(dashDuration);

        // Reset speed back to base value
        move.ResetSpeed();
        Debug.Log("Dash ended");

        // Wait for cooldown before allowing another dash
        yield return new WaitForSeconds(dashCooldown);

        isDashing = false;
        Debug.Log("Dash cooldown over");
    }
}
