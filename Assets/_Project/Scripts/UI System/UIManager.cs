using PatternLibrary;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject pauseButton, unPauseButton, settingsButton, buttons;

    public GameObject tapToStart;

    public GameObject inputUI;

    public GameObject countDownHolder;
    public Image three, two, one;

    public Color endCooldownColor;

    public void EnableUI()
    {
        tapToStart.SetActive(false);
        inputUI.SetActive(true);
        buttons.SetActive(true);
        InputManager.Instance.StartInputUI();
    }

    public void GameOverBehaviour()
    {
        pauseButton.transform.parent.GetComponent<Animator>().SetBool("isGameOver", true);
        unPauseButton.transform.parent.GetComponent<Animator>().SetBool("isGameOver", true);
        settingsButton.transform.parent.GetComponent<Animator>().SetBool("isGameOver", true);

        ScoreManager.Instance.scoreTracker.transform.parent.GetComponent<Animator>().SetTrigger("showScore");
        ScoreManager.Instance.mistakesTracker.transform.parent.GetComponent<Animator>().SetTrigger("showScore");

        inputUI.GetComponent<Animator>().SetBool("isGameOver", true);   
    }

    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    public void PauseGame()
    {
        GameManager.Instance.PauseGame();
    }
    public void ResumeGame()
    {
        GameManager.Instance.ResumeGame();
    }
    public void OpenSettings()
    {
        PauseGame();
        SettingsManager.Instance.OpenSettings();
    }
    public void CloseSettings()
    {
        SettingsManager.Instance.CloseSettings();
    }

    public void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }

    public void DisplayCountdown(int valueToDisplay)
    {
        countDownHolder.SetActive(true);

        switch (valueToDisplay)
        {
            case 3:
                three.gameObject.SetActive(true);
                two.gameObject.SetActive(false);
                one.gameObject.SetActive(false);
                break;
            case 2:
                three.gameObject.SetActive(false);
                two.gameObject.SetActive(true);
                one.gameObject.SetActive(false);
                break;
            case 1:
                three.gameObject.SetActive(false);
                two.gameObject.SetActive(false);
                one.gameObject.SetActive(true);
                break;
            case 0:
                break;
        }
    }

    public void HideCountdown()
    {
        countDownHolder.SetActive(false);
    }

    private void FixedUpdate()
    {
        switch (GameManager.Instance.gameState)
        {
            case GameManager.GameState.Paused:
                pauseButton.SetActive(false);
                unPauseButton.SetActive(true);
                break;
            case GameManager.GameState.Playing:
                pauseButton.SetActive(true);
                unPauseButton.SetActive(false);
                break;
            case GameManager.GameState.GameOver:
                break;
        }
    }
}
