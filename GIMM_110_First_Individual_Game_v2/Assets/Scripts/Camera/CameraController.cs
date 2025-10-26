using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Follow Player")]
    [SerializeField] private Transform player;      // Player to follow
    [SerializeField] private float aheadDistance = 2f; // How far ahead the camera looks horizontally
    [SerializeField] private float upDistance = 1f;    // How far ahead the camera looks vertically
    [SerializeField] private float cameraSpeed = 3f;   // How quickly camera follows
    private float lookAhead;
    private float lookUp;

    [Header("Zoom")]
    [SerializeField] private float cameraDistance = 8f; // Orthographic camera zoom distance
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("CameraController: No Camera component found!");
        }
        else
        {
            cam.orthographic = true;
            cam.orthographicSize = cameraDistance;
        }
    }

    private void Update()
    {
        if (player == null) return;

        // Smooth look-ahead horizontally (based on facing direction)
        lookAhead = Mathf.Lerp(lookAhead, aheadDistance * player.localScale.x, Time.deltaTime * cameraSpeed);

        // Smooth vertical follow
        lookUp = Mathf.Lerp(lookUp, upDistance * player.localScale.y, Time.deltaTime * cameraSpeed);

        // Update camera position (combine X and Y)
        Vector3 targetPos = new Vector3(
            player.position.x + lookAhead,
            player.position.y + lookUp,
            transform.position.z
        );

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * cameraSpeed);

        // Allow runtime zoom
        if (cam != null)
            cam.orthographicSize = cameraDistance;
    }

    public void MoveToNewRoom(Transform newRoom)
    {
        transform.position = new Vector3(newRoom.position.x, newRoom.position.y, transform.position.z);
    }
}
