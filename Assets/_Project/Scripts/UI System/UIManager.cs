using PatternLibrary;
using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public GameObject pauseButton, unPauseButton, settingsButton, buttons;

    public GameObject tapToStart;

    public GameObject inputUI;

    public TextMeshProUGUI countdownText;
    public Color endCooldownColor;

    public void EnableUI()
    {
        tapToStart.SetActive(false);
        inputUI.SetActive(true);
        buttons.SetActive(true);
        InputManager.Instance.StartInputUI();
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

    public void DisplayCountdown(string value)
    {
        countdownText.enabled = true;
        if (value == "L")
        {
            countdownText.color = endCooldownColor;
        }
        else
        {
            countdownText.color = Color.white;
        }
        countdownText.text = value;
    }

    public void HideCountdown()
    {
        countdownText.enabled = false;
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
        }
    }
}
