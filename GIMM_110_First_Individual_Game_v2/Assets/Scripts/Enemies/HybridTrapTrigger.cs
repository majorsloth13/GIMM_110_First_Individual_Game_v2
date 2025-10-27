using UnityEngine;
using System.Collections;

/// <summary>
/// A flexible trigger that activates traps by player proximity or pressure plate contact.
/// Supports direction-based detection, gizmos, and automatic retriggering.
/// </summary>
public class HybridTrapTrigger : MonoBehaviour
{
    public enum TriggerMode { PressurePlate, Proximity }
    public enum Direction { Up, Down, Left, Right }

    [Header("Trigger Settings")]
    [SerializeField] private TriggerMode triggerMode = TriggerMode.PressurePlate;
    [SerializeField] private Direction detectDirection = Direction.Left;
    [SerializeField] private float detectRange = 3f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Linked Trap")]
    [SerializeField] private GameObject trap;  // Reference to Firetrap or ArrowTrap

    [Header("Cooldown")]
    [SerializeField] private float retriggerDelay = 2f; // Time before trap can trigger again

    private bool trapTriggered;

    private void Update()
    {
        if (triggerMode == TriggerMode.Proximity)
        {
            DetectPlayerProximity();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerMode == TriggerMode.PressurePlate && !trapTriggered && collision.CompareTag("Player"))
        {
            StartCoroutine(TriggerTrap("Pressure Plate"));
        }
    }

    private void DetectPlayerProximity()
    {
        Vector2 dir = GetDirectionVector();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, detectRange, playerLayer);

        if (hit.collider != null && !trapTriggered)
        {
            StartCoroutine(TriggerTrap($"Proximity ({detectDirection})"));
        }

        // Draw debug ray
        Debug.DrawRay(transform.position, dir * detectRange, hit.collider ? Color.green : Color.red);
    }

    private IEnumerator TriggerTrap(string method)
    {
        trapTriggered = true;

        if (trap == null)
        {
            Debug.LogWarning("No trap assigned to HybridTrapTrigger.");
            yield break;
        }

        // Try Firetrap
        Firetrap firetrap = trap.GetComponent<Firetrap>();
        if (firetrap != null)
        {
            firetrap.ActivateTrap();
            Debug.Log($"Firetrap triggered by {method}");
        }

        // Try ArrowTrap
        ArrowTrap arrowTrap = trap.GetComponent<ArrowTrap>();
        if (arrowTrap != null)
        {
            arrowTrap.ActivateTrap();
            Debug.Log($"ArrowTrap triggered by {method}");
        }

        // Wait before allowing another activation
        yield return new WaitForSeconds(retriggerDelay);
        trapTriggered = false;
    }

    private Vector2 GetDirectionVector()
    {
        return detectDirection switch
        {
            Direction.Up => Vector2.up,
            Direction.Down => Vector2.down,
            Direction.Left => Vector2.left,
            Direction.Right => Vector2.right,
            _ => Vector2.zero
        };
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector2 dir = GetDirectionVector();
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(dir * detectRange));
        Gizmos.DrawSphere(transform.position + (Vector3)(dir * detectRange), 0.1f);
    }
}
