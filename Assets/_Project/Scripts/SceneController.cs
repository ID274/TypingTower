using UnityEngine;

public class SceneController : MonoBehaviour
{
    private MySceneManager mySceneManager;
    private void Start()
    {
        mySceneManager = MySceneManager.Instance;
        if (mySceneManager == null)
        {
            Debug.LogError("MySceneManager is not found in the scene. Creating a new one.");
            Instantiate(new GameObject("-MySceneManager")).AddComponent<MySceneManager>().persistentAcrossScenes = true;
        }
    }

    public void ReloadCurrentScene()
    {
        mySceneManager.ReloadCurrentScene();
    }

    public void LoadSceneSingle(string sceneName)
    {
        mySceneManager.LoadSceneSingle(sceneName);
    }

    public void LoadScene(string sceneName)
    {
        mySceneManager.LoadScene(sceneName);
    }

    public void LoadSceneAsync(string sceneName)
    {
        mySceneManager.LoadSceneAsync(sceneName);
    }
}
