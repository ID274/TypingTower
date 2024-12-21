using System.Collections;
using System.Collections.Generic;
using PatternLibrary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : Singleton<MySceneManager>
{
    public string mainMenuName;
    public float delay = 1f;
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

    public void LoadSceneDelayed(string sceneName)
    {
        StartCoroutine(LoadSceneDelay(sceneName));
    }

    public IEnumerator LoadSceneDelay(string sceneName)
    {
        ListenForSceneChange sceneTransition = FindObjectOfType<ListenForSceneChange>();
        if (sceneTransition != null)
        {
            sceneTransition.Transition();
        }
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
