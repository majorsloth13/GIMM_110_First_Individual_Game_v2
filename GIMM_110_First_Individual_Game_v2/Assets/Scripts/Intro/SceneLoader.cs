using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    private void Update()
    {
        
    }

    //Let's add our own code here
    public void LoadOnClick(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

}
