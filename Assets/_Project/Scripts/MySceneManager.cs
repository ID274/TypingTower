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
}
