using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region Custom Methods
    /// <summary>
    /// Loads the given game scene.
    /// </summary>
    public void PlayGame()
    {
        // Can use a serialized variable here to improve code flexibility.
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Called to quit the game.
    /// </summary>
    public void QuitGame()
    {
        //Application.Quit(); // Closes the built application.
        Debug.Log("Quit");
    }
    #endregion
}
