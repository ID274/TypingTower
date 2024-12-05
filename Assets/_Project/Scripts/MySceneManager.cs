using System.Collections;
using System.Collections.Generic;
using PatternLibrary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : Singleton<MySceneManager>
{
    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneAsynchronously(string sceneName)
    {
        //StartCoroutine(LoadSceneAsync(sceneName));
    }
}
