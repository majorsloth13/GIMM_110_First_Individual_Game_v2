using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private RespawnManager respawnManager; // Reference to the respawn manager for handling player death

    [Header("Health")]
    [SerializeField] private float startingHealth;          // Starting health value set in the Inspector
    public float currentHealth { get; private set; }        // Public read-only access to current health

    private Animator anim;                                  // Reference to Animator for playing hurt/die animations
    private bool dead;                                      // Tracks whether the player is already dead

    [Header("iFrames")]
    [SerializeField] private float iFrameDuration;          // Total duration of invincibility frames (after damage)
    [SerializeField] private int numberOfFlashes;           // Number of times the sprite flashes during iFrames
    private SpriteRenderer spriteRend;                      // Reference to the SpriteRenderer for visual feedback

    private void Awake()
    {
        currentHealth = startingHealth;                     // Initialize health at the start
        anim = GetComponent<Animator>();                    // Cache reference to Animator
        spriteRend = GetComponent<SpriteRenderer>();        // Cache reference to SpriteRenderer
    }

    /// <summary>
    /// Call this to deal damage to the player.
    /// Triggers hurt animation and invincibility frames if not dead.
    /// </summary>
    public void TakeDamage(float _damage)
    {
        // Subtract damage from current health, clamping between 0 and starting health
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");                        // Trigger hurt animation
            StartCoroutine(Invulnerability());              // Start invincibility frames
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("die");                     // Trigger death animation
                GetComponent<Movement2D>().enabled = false; // Disable movement
                dead = true;                                // Mark as dead to prevent repeat calls

                // Handle respawn if manager is assigned
                if (respawnManager != null)
                {
                    respawnManager.RespawnPlayer();
                }
            }
        }
    }

    /// <summary>
    /// Resets the health back to full and revives the player.
    /// </summary>
    public void ResetHealth()
    {
        currentHealth = startingHealth;
        dead = false;
    }

    /// <summary>
    /// Adds health to the player, clamped to the starting max.
    /// </summary>
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    /// <summary>
    /// Temporarily disables collisions between player and enemy layers and flashes the sprite.
    /// </summary>
    private IEnumerator Invulnerability()
    {
        // Ignore collision between player (layer 8) and enemy (layer 9)
        Physics2D.IgnoreLayerCollision(8, 9, true);

        // Flash the sprite several times
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);     // Semi-transparent red
            yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));

            spriteRend.color = Color.white;                  // Back to normal
            yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));
        }

        // Re-enable collisions
        Physics2D.IgnoreLayerCollision(8, 9, false);
    }
}
