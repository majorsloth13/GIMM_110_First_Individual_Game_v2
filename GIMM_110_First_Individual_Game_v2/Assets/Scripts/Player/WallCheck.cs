using UnityEngine;

public class WallCheck : MonoBehaviour
{
    [Header("Wall Check Settings")]
    [SerializeField] private float checkDistance = 0.4f;   // How far to check from the player
    [SerializeField] private LayerMask wallLayers;         // Which layers count as walls

    public bool IsTouchingWall { get; private set; }

    private void Update()
    {
        // Raycast in the direction the player is facing (based on local scale)
        IsTouchingWall = Physics2D.Raycast(transform.position, Vector2.right * transform.parent.localScale.x, checkDistance, wallLayers);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * transform.parent.localScale.x * checkDistance);
    }

    public bool IsTouchingWall()
    {
        return Physics2D.OverlapCircle(transform.position, checkRadius, wallLayers);
    }

    public bool IsTouchingRightWall => Physics2D.Raycast(transform.position, Vector2.right, checkDistance, wallLayers);
    public bool IsTouchingLeftWall => Physics2D.Raycast(transform.position, Vector2.left, checkDistance, wallLayers);

}
