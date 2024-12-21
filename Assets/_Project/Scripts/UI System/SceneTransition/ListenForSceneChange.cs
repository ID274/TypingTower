using UnityEngine;

public class ListenForSceneChange : MonoBehaviour
{
    public GameObject fadeOut, fadeIn;

    public void Start()
    {
        fadeOut.SetActive(false);
    }

    public void Transition()
    {
        switch (fadeOut.activeSelf)
        {
            case true:
                fadeOut.SetActive(false);
                fadeIn.SetActive(true);
                break;
            case false:
                fadeOut.SetActive(true);
                fadeIn.SetActive(false);
                break;
        }
    }
}
