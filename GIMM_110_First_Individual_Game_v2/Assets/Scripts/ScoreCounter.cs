using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    #region Variables
    Text text;
    public static int coinAmount; // public static variable to be accessed by other scripts.
    #endregion

    #region Unity Methods
    void Start()
    {
        text = GetComponent<Text>(); // Gets the text component of the object this script is attached to.
    }

    void Update()
    {
        // Could be better to put this in a separate method to avoid calling it every frame.
        text.text = coinAmount.ToString(); // Updates the text to display the current coin amount.
    }
    #endregion
}
