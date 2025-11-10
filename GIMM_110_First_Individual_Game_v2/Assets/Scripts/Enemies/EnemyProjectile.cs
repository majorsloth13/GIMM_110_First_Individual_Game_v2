using UnityEngine;

public class EnemyProjectile : DeathBox
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float resetTime = 3f;
    [SerializeField] private LayerMask collisionLayers;

    private float lifetime;
    private Vector2 direction;
    private bool isActive;

    /// <summary>
    /// Activates the projectile and sets its direction.
    /// </summary>
    public void ActivateProjectile(Vector2 dir)
    {
        dir.y = 0f; // force purely horizontal shot
        direction = dir.normalized;
        lifetime = 0f;
        isActive = true;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!isActive)
            return;

        // Move the projectile forward
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
        {
            ResetProjectile();
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        {
             base.OnTriggerEnter2D(collision);

             // Always reset if we hit the player
             if (collision.CompareTag("Player"))
             {
                 ResetProjectile();
                 return;
             }

             // Otherwise only reset on specific layers
             if (((1 << collision.gameObject.layer) & collisionLayers) != 0)
             {
                //ResetProjectile();
                Debug.Log($"HIT: {collision.name} on layer {LayerMask.LayerToName(collision.gameObject.layer)}");
             }
         }
    }

    private void ResetProjectile()
    {
        if (!isActive) return;

        Debug.Log($"ResetProjectile() called on {name}");

        isActive = false;

        // Immediately halt all movement
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // Stop further movement from Update
        direction = Vector2.zero;

        // Disable collisions and visuals instantly
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.enabled = false;

        //ensures it’s not mid-air when disabled
        transform.position = Vector3.zero;

        // Safely disable after a single frame
        DeactivateProjectile();
    }

    private void DeactivateProjectile()
    {
        gameObject.SetActive(false);

        // Re-enable components so it can be reused later
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = true;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.enabled = true;
    }
}
