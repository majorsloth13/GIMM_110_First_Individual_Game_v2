using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class RetractableSpike2D : EnemyDamage
{
    public enum MovementDirection { Vertical, Horizontal }

    [Header("Spike Motion")]
    public Transform spikeTransform;
    public MovementDirection moveDirection = MovementDirection.Vertical;
    public bool reverseDirection = false;

    [Tooltip("How far the spike is hidden inside its parent when retracted (local units).")]
    public float retractedOffset = 0f;

    [Tooltip("How far the spike extends out from its base (local units).")]
    public float extendedOffset = 0.5f;

    [Header("Timing")]
    public float extendDuration = 0.15f;
    public float holdDuration = 0.8f;
    public float retractDuration = 0.15f;

    [Header("Damage Collider")]
    public Collider2D damageCollider;
    public bool enableColliderDuringTravel = false;

    [Header("Auto Cycle")]
    public bool autoCycle = true;
    public float cycleDelay = 0.5f;

    private Coroutine cycleRoutine;
    private Vector3 baseLocalPosition;
    private Vector3 retractedLocalPosition;
    private Vector3 extendedLocalPosition;

    void Awake()
    {
        if (spikeTransform == null)
            spikeTransform = transform;

        if (damageCollider == null)
            damageCollider = GetComponent<Collider2D>();

        baseLocalPosition = spikeTransform.localPosition;

        // Direction based on enum + reverse toggle
        Vector3 dir = moveDirection == MovementDirection.Vertical
            ? (reverseDirection ? Vector3.down : Vector3.up)
            : (reverseDirection ? Vector3.left : Vector3.right);

        // Define retracted and extended positions with offsets
        retractedLocalPosition = baseLocalPosition - dir * retractedOffset;
        extendedLocalPosition = baseLocalPosition + dir * extendedOffset;

        // Start retracted and collider off
        spikeTransform.localPosition = retractedLocalPosition;
        if (damageCollider) damageCollider.enabled = false;
    }

    void Start()
    {
        if (autoCycle)
            StartCycle();
    }

    public void StartCycle()
    {
        if (cycleRoutine != null)
            StopCoroutine(cycleRoutine);
        cycleRoutine = StartCoroutine(CycleRoutine());
    }

    public void StopCycleAndRetract()
    {
        if (cycleRoutine != null)
            StopCoroutine(cycleRoutine);
        cycleRoutine = null;
        spikeTransform.localPosition = retractedLocalPosition;
        if (damageCollider) damageCollider.enabled = false;
    }

    private IEnumerator CycleRoutine()
    {
        while (true)
        {
            yield return OneCycle();
            yield return new WaitForSeconds(cycleDelay);
        }
    }

    private IEnumerator OneCycle()
    {
        if (enableColliderDuringTravel && damageCollider)
            damageCollider.enabled = true;

        yield return MoveSpike(spikeTransform.localPosition, extendedLocalPosition, extendDuration);

        if (damageCollider)
            damageCollider.enabled = true;

        yield return new WaitForSeconds(holdDuration);

        if (!enableColliderDuringTravel && damageCollider)
            damageCollider.enabled = false;

        yield return MoveSpike(spikeTransform.localPosition, retractedLocalPosition, retractDuration);

        if (damageCollider)
            damageCollider.enabled = false;
    }

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
            spikeTransform.localPosition = Vector3.Lerp(from, to, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        spikeTransform.localPosition = to;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (spikeTransform == null)
            spikeTransform = transform;

        // Direction vector based on movement settings
        Vector3 dir = moveDirection == MovementDirection.Vertical
            ? (reverseDirection ? Vector3.down : Vector3.up)
            : (reverseDirection ? Vector3.left : Vector3.right);

        // Use spikeTransform.localPosition if in play mode, or transform.localPosition otherwise
        Vector3 baseLocal = Application.isPlaying ? spikeTransform.localPosition : spikeTransform.localPosition;

        // Compute world-space positions
        Vector3 retractedWorld = spikeTransform.parent != null
            ? spikeTransform.parent.TransformPoint(baseLocal - dir * retractedOffset)
            : spikeTransform.TransformPoint(baseLocal - dir * retractedOffset);

        Vector3 extendedWorld = spikeTransform.parent != null
            ? spikeTransform.parent.TransformPoint(baseLocal + dir * extendedOffset)
            : spikeTransform.TransformPoint(baseLocal + dir * extendedOffset);

        // Draw the gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(retractedWorld, 0.05f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(extendedWorld, 0.05f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(retractedWorld, extendedWorld);
    }
#endif
    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }*/
}
