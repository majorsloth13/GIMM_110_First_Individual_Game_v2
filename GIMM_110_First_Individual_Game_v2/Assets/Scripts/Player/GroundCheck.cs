using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    #region Variables

    [Header("Ground Check Settings")]
    [SerializeField] private float checkRadius = 0.2f;        // Radius of the circle used to check for ground beneath the player
    [SerializeField] private Vector2 checkDistance;      // How far below the player to check for ground
    [SerializeField] private LayerMask groundLayers;          // Which layers count as ground (e.g. Ground, Platform, MovingPlatform)
    GameObject player;
    #endregion

    /* #region Custom Methods

     /// <summary>
     /// Checks if the player is currently grounded by using an OverlapCircle.
     /// Returns true if a ground layer is found within the check area.
     /// </summary>
     public bool IsGrounded()
     {
         // Calculate the position slightly below the player to perform the check
         Vector2 checkPosition = (Vector2)transform.position + checkDistance;

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

     #endregion*/

    #region Unity Methods
    private void Start()
    {
        player = gameObject.transform.parent.gameObject; // Gets the parent object of the ground check object.
    }

    private void Update()
    {
        IsGrounded(); // Calls the CheckGround method every frame to constantly check if the player is grounded.
    }
#endregion

    #region Custom Methods
    /// <summary>
    /// Checks if the player is grounded, and sets the isGrounded variable in the Movement2D script.
    /// </summary>
    public bool IsGrounded() //Changed name from CheckGround to IsGrounded because it makes more sense to me
    {
        float radius = 0.2f; // The radius of the circle cast
        float distance = 0.2f; // The distance of the circle cast
        Vector2 origin = transform.position; // The position of the ground check object
        Vector2 direction = Vector2.down; // The direction of the raycast

        /* CircleCast is a 2D sphere cast that checks for colliders in a circular area.
            RaycastHit2D is a struct that stores information about the raycast hit. 
            Raycasts are basically invisible rays that are used to detect objects in the scene. */
        RaycastHit2D hit = Physics2D.CircleCast(origin, radius, direction, distance, groundLayers);

        // If the raycast hits a collider, the player is grounded. Otherwise, the player is not grounded.
        if (hit.collider != null)
        {
           return true;
        }
        else
        {
           return false;
        }
    }
    #endregion
}

