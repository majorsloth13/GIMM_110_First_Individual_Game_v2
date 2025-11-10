using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Follow Player")]
    [SerializeField] private Transform player;         // Player to follow
    [SerializeField] private float aheadDistance = 2f; // How far ahead the camera looks horizontally
    [SerializeField] private float upDistance = 1f;    // How far ahead the camera looks vertically
    [SerializeField] private float cameraSpeed = 3f;   // How quickly camera follows
    private float lookAhead;
    private float lookUp;

    [Header("Zoom")]
    //[SerializeField] private float cameraDistance = 8f; // Orthographic camera zoom distance
    [SerializeField] private float startZoom = 15f;     // Starting zoomed-out size
    [SerializeField] private float targetZoom = 8f;     // Final zoom size
    [SerializeField] private float zoomSpeed = 1f;      // How fast to zoom back in
    private bool zoomingIn = true;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("CameraController: No Camera component found!");
            return;
        }

        cam.orthographic = true;
        cam.orthographicSize = startZoom;
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

        
        if (zoomingIn)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);

            if (Mathf.Abs(cam.orthographicSize - targetZoom) < 0.05f)
            {
                cam.orthographicSize = targetZoom;
                zoomingIn = false; // Stop zooming once target reached
            }
        }
    }

public void MoveToNewRoom(Transform newRoom)
    {
        transform.position = new Vector3(newRoom.position.x, newRoom.position.y, transform.position.z);
    }

    public void ZoomEffect(float zoomOutAmount, float zoomDuration, float holdTime = 1.5f)
    {
        StopAllCoroutines(); // stop any current zooming coroutine
        StartCoroutine(ZoomRoutine(zoomOutAmount, zoomDuration, holdTime));
    }

    private System.Collections.IEnumerator ZoomRoutine(float zoomOutAmount, float zoomDuration, float holdTime = 1.5f)
    {
        float originalZoom = cam.orthographicSize;
        float targetOutZoom = originalZoom + zoomOutAmount;

        // Zoom out
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / (zoomDuration / 2);
            cam.orthographicSize = Mathf.Lerp(originalZoom, targetOutZoom, t);
            yield return null;
        }

        //Hold at zoomed out distance
        yield return new WaitForSeconds(holdTime); // Adjust this to control how long it stays zoomed out

        // Zoom back in
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / (zoomDuration / 2);
            cam.orthographicSize = Mathf.Lerp(targetOutZoom, originalZoom, t);
            yield return null;
        }

        cam.orthographicSize = originalZoom; // ensure final value
    }
}
