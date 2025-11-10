using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class MoveToNextLevel : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Name of the scene to load when the player enters this trigger.")]
    [SerializeField] private string nextSceneName;

    [Header("Optional Delay (seconds)")]
    [SerializeField] private float transitionDelay = 0.5f;

    [Header("Visual Feedback")]
    [SerializeField] private bool fadeOnTransition = false; // optional fade effect

    private bool isTransitioning = false;

    private void Reset()
    {
        // Automatically set collider to trigger
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTransitioning) return;

        if (collision.CompareTag("Player"))
        {
            // Check if all Matter collectibles are collected
            if (AllMatterCollected())
            {
                Debug.Log("All Matter collected! Transitioning to next level...");
                StartCoroutine(LoadNextScene());
            }
            else
            {
                Debug.LogWarning("You must collect all Matter before proceeding to the next level!");
            }
        }
    }

    /// <summary>
    /// Returns true if all Matter GameObjects in the scene are collected (none remain active).
    /// </summary>
    private bool AllMatterCollected()
    {
        // Uses the new Unity 2023+ API
        MatterPickup[] allMatter = FindObjectsByType<MatterPickup>(FindObjectsSortMode.None);

        // If no MatterPickup objects remain, return true
        return allMatter.Length == 0;
    }

    private System.Collections.IEnumerator LoadNextScene()
    {
        isTransitioning = true;

        // Optional delay
        yield return new WaitForSeconds(transitionDelay);

        // Optional fade logic (if you later add one)
        if (fadeOnTransition)
        {
            Debug.Log("Fade effect would start here...");
        }

        // Reset the static score counter
        ScoreCounter.matterAmount = 0;

        // Load the next scene
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("No next scene name assigned in MoveToNextLevel.");
        }
    }
}
