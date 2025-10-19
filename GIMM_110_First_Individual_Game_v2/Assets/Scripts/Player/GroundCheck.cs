using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    #region Variables

    [Header("Ground Check Settings")]
    [SerializeField] private float checkRadius = 0.2f;        // Radius of the circle used to check for ground beneath the player
    [SerializeField] private float checkDistance = 0.5f;      // How far below the player to check for ground
    [SerializeField] private LayerMask groundLayers;          // Which layers count as ground (e.g. Ground, Platform, MovingPlatform)

    #endregion

    #region Custom Methods

    /// <summary>
    /// Checks if the player is currently grounded by using an OverlapCircle.
    /// Returns true if a ground layer is found within the check area.
    /// </summary>
    public bool IsGrounded()
    {
        // Calculate the position slightly below the player to perform the check
        Vector2 checkPosition = (Vector2)transform.position + Vector2.down * checkDistance;

        // Use OverlapCircle to detect any ground colliders within the radius at the check position
        return Physics2D.OverlapCircle(checkPosition, checkRadius, groundLayers);
    }

    /// <summary>
    /// Visualizes the ground check area in the Unity editor.
    /// Helpful for debugging the size and position of the ground detection.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        // Calculate the same position used by the OverlapCircle
        Vector2 checkPosition = (Vector2)transform.position + Vector2.down * checkDistance;

        // Draw a wireframe circle in the Scene view to show where the ground check happens
        Gizmos.DrawWireSphere(checkPosition, checkRadius);
    }

    public LayerMask GroundLayerMask => groundLayers;

    #endregion
}
