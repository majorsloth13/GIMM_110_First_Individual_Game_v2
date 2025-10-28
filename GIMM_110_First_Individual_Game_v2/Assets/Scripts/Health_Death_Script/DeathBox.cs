using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBox : MonoBehaviour
{
    [SerializeField] private float damageAmount = 999f; // Enough to kill the player
    [SerializeField] private Color gizmoColor = new Color(1f, 0f, 0f, 0.3f); // Semi-transparent red

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

    private void OnDrawGizmos()
    {
        // Set color for visualization in the editor
        Gizmos.color = gizmoColor;

        // Try to match collider bounds (works for BoxCollider2D and CircleCollider2D)
        Collider2D col = GetComponent<Collider2D>();
        if (col is BoxCollider2D box)
        {
            // Draw a filled box that matches the trigger’s size and position
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(box.offset, box.size);
            Gizmos.color = Color.red; // Draw outline in red
            Gizmos.DrawWireCube(box.offset, box.size);
        }
        else if (col is CircleCollider2D circle)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireSphere(circle.offset, circle.radius);
        }
    }
}
