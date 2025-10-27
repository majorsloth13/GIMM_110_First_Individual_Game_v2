#if UNITY_EDITOR
using UnityEngine;

[AddComponentMenu("Miscellaneous/README Info Note")]
public class CommentInformationNote : MonoBehaviour
{
    [TextArea(17, 1000)]
    public string comment = "Information Here.";

    void Awake()
    {
        comment = null;
        Destroy(this);
    }
}
#endif