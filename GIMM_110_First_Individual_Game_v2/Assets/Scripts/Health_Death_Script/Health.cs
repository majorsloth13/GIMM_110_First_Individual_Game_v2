using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private RespawnManager respawnManager; // Reference to the respawn manager for handling player death

    [Header("Health")]
    [SerializeField] private float startingHealth = 100f;   // Starting health
    public float currentHealth { get; private set; }        // Public read-only access to current health

    private Animator anim;                                  // Animator reference
    private bool dead;                                      // Tracks death status
    private SpriteRenderer spriteRend;                      // For visual feedback (optional)

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Call this to deal damage to the player.
    /// </summary>
    public void TakeDamage(float damage)
    {
        Debug.Log($"{name} took {damage} damage! Called by: {new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().DeclaringType}");
        if (dead)
        {
            Debug.LogWarning("TakeDamage called while already dead!");
        }

        // Subtract health, clamp between 0 and max
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        Debug.Log($"Player took {damage} damage. Health: {currentHealth}");

        if (currentHealth > 0)
        {
            // Hurt feedback if still alive
            anim?.SetTrigger("hurt");
        }
        else
        {
            // Death sequence
            if (!dead)
            {
                dead = true;
                Debug.Log("Player died.");

                anim?.SetTrigger("die");

                // Disable movement temporarily
                Movement2D move = GetComponent<Movement2D>();
                if (move != null)
                    move.enabled = false;

                // Respawn after delay
                if (respawnManager != null)
                    respawnManager.RespawnPlayer();
            }
        }
    }

    /// <summary>
    /// Fully restores health and revives player.
    /// </summary>
    public void ResetHealth()
    {
        currentHealth = startingHealth;
        dead = false;
        Debug.Log("Player health reset to full.");
    }
}
