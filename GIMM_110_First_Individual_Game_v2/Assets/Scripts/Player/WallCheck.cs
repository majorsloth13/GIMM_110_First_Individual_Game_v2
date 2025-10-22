using UnityEngine;

public class WallCheck : MonoBehaviour
{
    [Header("Wall Check Settings")]
    [SerializeField] private float checkDistance = 0.4f;   // How far to check from the player
    [SerializeField] private LayerMask wallLayers;         // Which layers count as walls

    public bool IsTouchingWall { get; private set; }
    public bool IsTouchingLeftWall { get; private set; }
    public bool IsTouchingRightWall { get; private set; }

    private void Update()
    {
        // Raycast returns RaycastHit2D; check if it hits
        IsTouchingLeftWall = Physics2D.Raycast(transform.position, Vector2.left, checkDistance, wallLayers).collider != null;
        IsTouchingRightWall = Physics2D.Raycast(transform.position, Vector2.right, checkDistance, wallLayers).collider != null;

        IsTouchingWall = IsTouchingLeftWall || IsTouchingRightWall;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * checkDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * checkDistance);
    }
}
