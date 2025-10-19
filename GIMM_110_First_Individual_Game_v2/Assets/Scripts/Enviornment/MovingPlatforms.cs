using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatforms : MonoBehaviour
{
    [Header("Movement Settings")]
    public bool moveVertically = false;
    public bool startInReverse = false;
    public float moveDistance = 3f;
    public float moveSpeed = 2f;

    private Vector3 startPos;
    private Vector3 endPos;
    private bool movingToEnd = true;

    private Rigidbody2D rb;
    private Vector3 lastPos;

    public Vector2 CurrentVelocity { get; private set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        startPos = transform.position;

        // Determine direction
        Vector3 moveDir;
        if (moveVertically)
            moveDir = startInReverse ? Vector3.down : Vector3.up;
        else
            moveDir = startInReverse ? Vector3.left : Vector3.right;

        // Set end position based on direction
        endPos = startPos + moveDir * moveDistance;

        lastPos = rb.position;

        // Always start moving toward the end position from the placed position
        movingToEnd = true;
    }

    private void FixedUpdate()
    {
        Vector3 target = movingToEnd ? endPos : startPos;
        Vector3 newPos = Vector3.MoveTowards(rb.position, target, moveSpeed * Time.fixedDeltaTime);

        CurrentVelocity = (newPos - lastPos) / Time.fixedDeltaTime;
        lastPos = newPos;

        rb.MovePosition(newPos);

        if (Vector3.Distance(newPos, target) < 0.01f)
            movingToEnd = !movingToEnd;
    }

    private void OnDrawGizmosSelected()
    {
        // Always start from the editor position
        Vector3 basePos = transform.position;

        // Determine direction exactly like in Start()
        Vector3 moveDir;
        if (moveVertically)
            moveDir = startInReverse ? Vector3.down : Vector3.up;
        else
            moveDir = startInReverse ? Vector3.left : Vector3.right;

        Vector3 endPos = basePos + moveDir * moveDistance;

        // Draw line for movement path
        Gizmos.color = Color.green;
        Gizmos.DrawLine(basePos, endPos);

        // Start and end markers
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(basePos, 0.1f); // Start
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(endPos, 0.1f);  // Target
    }
}