using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class RetractableSpike2D : EnemyDamage
{
    public enum MovementDirection { Vertical, Horizontal }

    [Header("Spike Motion")]
    public Transform spikeTransform;                     // The actual spike object that will move
    public MovementDirection moveDirection = MovementDirection.Vertical;  // Direction of spike movement
    public bool reverseDirection = false;                // Reverses direction (up/down or left/right)

    [Tooltip("How far the spike is hidden inside its parent when retracted (local units).")]
    public float retractedOffset = 0f;

    [Tooltip("How far the spike extends out from its base (local units).")]
    public float extendedOffset = 0.5f;

    [Header("Timing")]
    public float extendDuration = 0.15f;                 // Time to fully extend
    public float holdDuration = 0.8f;                    // Time to stay extended
    public float retractDuration = 0.15f;                // Time to fully retract

    [Header("Damage Collider")]
    public Collider2D damageCollider;                    // Collider used to damage the player
    public bool enableColliderDuringTravel = false;      // Whether damage can occur during movement

    [Header("Auto Cycle")]
    public bool autoCycle = true;                        // Should spike cycle automatically
    public float cycleDelay = 0.5f;                      // Delay between each spike cycle

    private Coroutine cycleRoutine;                      // Reference to currently running coroutine
    private Vector3 baseLocalPosition;                   // Starting local position of spike
    private Vector3 retractedLocalPosition;              // Fully retracted local position
    private Vector3 extendedLocalPosition;               // Fully extended local position
    private bool canDamage = false;

    void Awake()
    {
        // Use this object if no spikeTransform was assigned
        if (spikeTransform == null)
            spikeTransform = transform;

        // Use this object's collider if none was manually assigned
        if (damageCollider == null)
            damageCollider = GetComponent<Collider2D>();

        baseLocalPosition = spikeTransform.localPosition;

        // Calculate movement direction vector
        Vector3 dir = moveDirection == MovementDirection.Vertical
            ? (reverseDirection ? Vector3.down : Vector3.up)
            : (reverseDirection ? Vector3.left : Vector3.right);

        // Calculate full retracted and extended positions
        retractedLocalPosition = baseLocalPosition - dir * retractedOffset;
        extendedLocalPosition = baseLocalPosition + dir * extendedOffset;

        spikeTransform.localPosition = retractedLocalPosition;

        // Always enable the collider so collisions are detected
        if (damageCollider)
            damageCollider.enabled = true;
    }

    void Start()
    {
        // If autoCycle is enabled, begin the spike animation loop
        if (autoCycle)
            StartCycle();
    }

    /// <summary>
    /// Starts the spike movement loop.
    /// </summary>
    public void StartCycle()
    {
        // Stop any existing cycle and start a new one
        if (cycleRoutine != null)
            StopCoroutine(cycleRoutine);
        cycleRoutine = StartCoroutine(CycleRoutine());
    }

    /// <summary>
    /// Stops the spike loop and fully retracts it.
    /// </summary>
    public void StopCycleAndRetract()
    {
        if (cycleRoutine != null)
            StopCoroutine(cycleRoutine);
        cycleRoutine = null;

        // Force retract the spike
        spikeTransform.localPosition = retractedLocalPosition;

        // Disable collider
        //if (damageCollider) damageCollider.enabled = false;
    }

    /// <summary>
    /// Repeats the spike movement cycle indefinitely.
    /// </summary>
    private IEnumerator CycleRoutine()
    {
        while (true)
        {
            yield return OneCycle();
            yield return new WaitForSeconds(cycleDelay);
        }
    }

    /// <summary>
    /// Executes a single spike extend-hold-retract cycle.
    /// </summary>
private IEnumerator OneCycle()
{
    // Spike starts retracted — safe
    canDamage = false;

    // Extend
    if (enableColliderDuringTravel)
        canDamage = true;

    yield return MoveSpike(spikeTransform.localPosition, extendedLocalPosition, extendDuration);

    // Fully extended — deal damage
    canDamage = true;

    yield return new WaitForSeconds(holdDuration);

    // Retract — stop damaging
    if (!enableColliderDuringTravel)
        canDamage = false;

    yield return MoveSpike(spikeTransform.localPosition, retractedLocalPosition, retractDuration);

    // Retracted — safe again
    canDamage = false;
}


    /// <summary>
    /// Smoothly moves the spike from one position to another over time.
    /// </summary>
    private IEnumerator MoveSpike(Vector3 from, Vector3 to, float duration)
    {
        if (duration <= 0f)
        {
            spikeTransform.localPosition = to;
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // Smooth interpolation between from and to
            spikeTransform.localPosition = Vector3.Lerp(from, to, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        // Ensure exact final position
        spikeTransform.localPosition = to;
    }

#if UNITY_EDITOR
    /// <summary>
    /// Draw gizmos in the editor to show spike movement bounds.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (spikeTransform == null)
            spikeTransform = transform;

        // Get direction vector based on movement type
        Vector3 dir = moveDirection == MovementDirection.Vertical
            ? (reverseDirection ? Vector3.down : Vector3.up)
            : (reverseDirection ? Vector3.left : Vector3.right);

        // Base local position (during edit or play mode)
        Vector3 baseLocal = Application.isPlaying ? spikeTransform.localPosition : spikeTransform.localPosition;

        // Calculate world positions for retracted and extended states
        Vector3 retractedWorld = spikeTransform.parent != null
            ? spikeTransform.parent.TransformPoint(baseLocal - dir * retractedOffset)
            : spikeTransform.TransformPoint(baseLocal - dir * retractedOffset);

        Vector3 extendedWorld = spikeTransform.parent != null
            ? spikeTransform.parent.TransformPoint(baseLocal + dir * extendedOffset)
            : spikeTransform.TransformPoint(baseLocal + dir * extendedOffset);

        // Draw spheres at each position and a line between them
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(retractedWorld, 0.05f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(extendedWorld, 0.05f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(retractedWorld, extendedWorld);
    }
#endif
}
