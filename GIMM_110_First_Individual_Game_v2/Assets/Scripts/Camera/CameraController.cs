using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Room camera
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    //follow player
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float cameraZoom;
    private float lookAhead;

    // Zoom
    [Header("Camera Distance")]
    [SerializeField] private float cameraDistance = 8f;
    private Camera cam;

    private void Start()
    {
        // Cache the Camera component
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("CameraController: No Camera component found!");
        }
        else
        {
            // Set the initial orthographic size based on cameraDistance
            cam.orthographic = true; // Ensure we're in 2D mode
            cam.orthographicSize = cameraDistance;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //Room camera
        //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z), ref velocity, speed);

        //follow player
        transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);

        //allow real-time camera zooming if cameraDistance changes at runtime
        if (cam != null)
        {
            cam.orthographicSize = cameraDistance;
        }
    }


    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }
}
