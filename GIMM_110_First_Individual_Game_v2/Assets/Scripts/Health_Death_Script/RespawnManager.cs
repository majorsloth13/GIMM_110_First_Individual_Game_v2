using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public Transform player;                     // Reference to the player's transform
    private Vector3 spawnPoint;                  // Position to respawn the player at
    private Health playerHealth;                 // Reference to the player's Health component

    [SerializeField] private float respawnDelay = 1f; // Time to wait before respawning

    private void Start()
    {
        if (player != null)
        {
            spawnPoint = player.position;                // Set initial spawn point to player's starting position
            playerHealth = player.GetComponent<Health>(); // Cache the Health component for later use
        }
    }

    /// <summary>
    /// Updates the player's spawn point to a new position.
    /// </summary>
    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }

    /// <summary>
    /// Public method to initiate the respawn sequence.
    /// </summary>
    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCoroutine());  // Start the coroutine to handle delayed respawn
    }

    /// <summary>
    /// Handles the delay, repositioning, and reactivation logic for the player's respawn.
    /// </summary>
    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay); // Wait before respawning

        // Reset player position to saved spawn point
        player.position = spawnPoint;

        // Reset health and death status
        if (playerHealth != null)
        {
            playerHealth.ResetHealth();
        }

        // Re-enable movement if it was disabled on death
        Movement2D movement = player.GetComponent<Movement2D>();
        if (movement != null)
        {
            movement.enabled = true;
        }

        // Trigger respawn animation if Animator is available
        Animator anim = player.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("respawn");
        }
    }
}
