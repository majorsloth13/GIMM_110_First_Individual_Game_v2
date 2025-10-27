using UnityEngine;

/// <summary>
/// Moves an enemy or trap object up and down between two custom vertical points.
/// Each direction (up/down) can have its own movement distance.
/// Damages the player on contact.
/// </summary>
public class Enemy_UpDown : MonoBehaviour // Name changed from Enemy_Sideways
{
    [Header("Movement Settings")]
    [SerializeField] private float upDistance = 2f;    // How far up the object moves
    [SerializeField] private float downDistance = 1f;  // How far down the object moves
    [SerializeField] private float speed = 2f;         // Movement speed
    [SerializeField] private float damage = 1f;        // Damage dealt to the player

    [Tooltip("If true, starts by moving downward instead of upward.")]
    public bool startInReverse = false;

    private float bottomEdge;   // Lowest Y position
    private float topEdge;      // Highest Y position
    private bool movingDown;    // Tracks current direction of movement

    private void Awake()
    {
        // The initial Y position determines the middle point
        float startY = transform.position.y;

        // Calculate movement limits
        bottomEdge = startY - downDistance;
        topEdge = startY + upDistance;

        // Decide which direction to start moving
        movingDown = startInReverse;
    }

    private void Update()
    {
        if (movingDown)
        {
            // Move downward
            if (transform.position.y > bottomEdge)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y - speed * Time.deltaTime,
                    transform.position.z
                );
            }
            else
            {
                // Reached bottom limit, start moving upward
                movingDown = false;
            }
        }
        else
        {
            // Move upward
            if (transform.position.y < topEdge)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y + speed * Time.deltaTime,
                    transform.position.z
                );
            }
            else
            {
                // Reached top limit, start moving downward
                movingDown = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Apply damage to player
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw gizmos to visualize movement range in the Scene view
        Vector3 basePos = transform.position;
        Vector3 topPos = basePos + Vector3.up * upDistance;
        Vector3 bottomPos = basePos - Vector3.up * downDistance;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(bottomPos, topPos);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(bottomPos, 0.1f); // Bottom marker
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(topPos, 0.1f);    // Top marker
    }
}
