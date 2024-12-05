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

    public void LoadSceneSingle(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneAsync(string sceneName)
    {
        //StartCoroutine(LoadSceneAsync(sceneName));
    }
}
