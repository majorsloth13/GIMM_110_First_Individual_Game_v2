using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    [SerializeField] private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only react to the player
        if (!collision.CompareTag("Player")) return;

        // If already activated, skip
        if (isActivated)
         {
             Debug.Log($"[Checkpoint] {name} already activated — skipping.");
             return;
         }

        // Look for the RespawnManager in both parent and child objects
        RespawnManager respawnManager = collision.GetComponentInParent<RespawnManager>();
        if (respawnManager == null)
            respawnManager = collision.GetComponentInChildren<RespawnManager>();

        // If found, update checkpoint
        if (respawnManager != null)
        {
            respawnManager.SetSpawnPoint(transform.position);
            isActivated = true;
            Debug.Log($"[Checkpoint] Triggered at {transform.position}");
        }
        else
        {
            Debug.LogWarning("[Checkpoint] Player triggered checkpoint but has no RespawnManager in its hierarchy!");
        }

        /*if (collision.CompareTag("Checkpoint"))
        {
            
        }*/
    }
}
