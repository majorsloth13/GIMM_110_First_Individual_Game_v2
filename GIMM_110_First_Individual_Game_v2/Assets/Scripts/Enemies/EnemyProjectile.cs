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
        base.OnTriggerEnter2D(collision);

        // Only reset when hitting something on valid layers
        if (((1 << collision.gameObject.layer) & collisionLayers) != 0)
        {
            ResetProjectile();
        }
    }

    private void ResetProjectile()
    {
        isActive = false;
        gameObject.SetActive(false);
    }
}
