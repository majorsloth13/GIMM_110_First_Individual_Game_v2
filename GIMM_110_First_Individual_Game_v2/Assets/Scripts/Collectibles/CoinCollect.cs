using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    #region Variables
    public PlatformManager platformManagerScript;
    #endregion

    #region Unity Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ScoreCounter.coinAmount += 1; // Increments the coin amount by 1.

        // Calls the NewPlatform method from the PlatformManager script.
        platformManagerScript.NewPlatform();
    }
    #endregion
}
