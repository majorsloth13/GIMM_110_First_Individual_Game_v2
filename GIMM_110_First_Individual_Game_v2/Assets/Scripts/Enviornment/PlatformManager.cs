using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    #region Variables
    // Array setup for platforms
    GameObject[] platforms;
    GameObject currentPlatform;
    int index = -1; // Index of the current platform
    int previousIndex = -1; // Prevents the same platform from being selected twice in a row

    // GameObject for coin appearing above platform
    public GameObject coin;
    #endregion

    #region Unity Methods
    private void Start()
    {
        SetPlatforms(); // Calls the SetPlatformsList method

        NewPlatform(); // Calls the NewPlatform method
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Sets the platforms array to all objects with the tag "Platform".
    /// </summary>
    public void SetPlatforms()
    {
        platforms = GameObject.FindGameObjectsWithTag("Platform"); // Creates an array of all objects with the tag platform
    }

    /// <summary>
    /// Randomly selects a platform for the player to reach.
    /// </summary>
    public void NewPlatform()
    {
        while (index == previousIndex) // Prevents the same platform from being selected twice in a row
        {
            index = Random.Range(0, platforms.Length); // randomly selects one platform
        }

        previousIndex = index; // sets the previous index to the current index

        currentPlatform = platforms[index]; // registers random platform as the one the player must get to
        coin.transform.position = new Vector2(currentPlatform.transform.position.x, currentPlatform.transform.position.y + 2f);
    }
    #endregion
}
