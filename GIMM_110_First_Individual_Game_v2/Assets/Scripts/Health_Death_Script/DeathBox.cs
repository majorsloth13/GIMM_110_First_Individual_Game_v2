using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBox : MonoBehaviour
{
    [SerializeField] private float damageAmount = 999f; // Enough to kill the player

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        

        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("you are dead");
            // Try to get the Health component from the player
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                // Deal fatal damage
                playerHealth.TakeDamage(damageAmount);
                
            }
        }
    }
}
