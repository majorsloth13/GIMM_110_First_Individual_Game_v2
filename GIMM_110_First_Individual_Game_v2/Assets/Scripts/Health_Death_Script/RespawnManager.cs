using System.Collections;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [Header("Player References")]
    public Transform player;                     // Reference to the player
    private Vector3 spawnPoint;                  // Player's respawn position
    private Transform activeCheckpoint; // Tracks the checkpoint's Transform if it moves
    private Health playerHealth;                 // Reference to player's Health component

    [Header("Settings")]
    [SerializeField] private float respawnDelay = 1f; // Time before player respawns
    [SerializeField] private float idleTransitionDelay = 1.5f; // How long to wait before switching to idle
    [SerializeField] private string checkpointTag = "Checkpoint"; // Tag for checkpoint objects
    [SerializeField] private string deathBoxTag = "Deathbox";   // Tag for death zones

    private void Start()
    {
        if (player != null)
        {
            spawnPoint = player.position;                 // Initial spawn point
            playerHealth = player.GetComponent<Health>(); // Cache Health component
        }
        else
        {
            Debug.LogWarning("Player not assigned to RespawnManager!");
        }
    }

    /// <summary>
    /// Called by checkpoint objects to update the player's respawn point.
    /// </summary>
    /* public void SetSpawnPoint(Vector3 newSpawnPoint)
     {
         spawnPoint = newSpawnPoint;
         Debug.Log("New checkpoint set at: " + spawnPoint);
     }*/
    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
        activeCheckpoint = null; // clear dynamic reference
        Debug.Log($"[RespawnManager] Static checkpoint set at {spawnPoint}");
    }

    // Overload for checkpoints on moving platforms
    public void SetSpawnPoint(Transform checkpointTransform)
    {
        if (checkpointTransform == null) return;

        activeCheckpoint = checkpointTransform;
        spawnPoint = checkpointTransform.position; // fallback
        Debug.Log($"[RespawnManager] Following moving checkpoint '{checkpointTransform.name}'");
    }


    /// <summary>
    /// Called externally when player dies or hits a death zone.
    /// </summary>
    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);

        // Move player to saved checkpoint
        //player.position = spawnPoint;

        // Use dynamic checkpoint position if available
        Vector3 respawnPos = activeCheckpoint != null ? activeCheckpoint.position : spawnPoint;
        player.position = respawnPos + Vector3.up * 0.5f; // offset slightly above platform
        Debug.Log($"[RespawnManager] Respawning at {(activeCheckpoint ? activeCheckpoint.position : spawnPoint)}");



        // Reset health if applicable
        if (playerHealth != null)
            playerHealth.ResetHealth();

        // Re-enable movement after respawn
        Movement2D movement = player.GetComponent<Movement2D>();
        if (movement != null)
            movement.enabled = true;

        // Trigger respawn animation if animator exists
        Animator anim = player.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("respawn");
            Debug.Log("Player respawned animation triggered.");

            // Wait for animation before transitioning to idle
            yield return new WaitForSeconds(idleTransitionDelay);

            anim.SetTrigger("idle");
            Debug.Log("Player transitioned to idle state.");
        }

        Debug.Log("Player respawned at: " + spawnPoint);
    }

    /// <summary>
    /// Detects checkpoint or death zone collisions automatically.
    /// Attach this script to an object with a collider (e.g., the player).
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Save new checkpoint
        if (collision.CompareTag(checkpointTag))
        {
            SetSpawnPoint(collision.transform.position);
        }

        // Handle death zone
        if (collision.CompareTag(deathBoxTag))
        {
            RespawnPlayer();
        }
    }
}
