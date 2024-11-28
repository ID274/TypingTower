using PatternLibrary;
using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public GameObject pauseButton, unPauseButton;

    public TextMeshProUGUI countdownText;
    public Color endCooldownColor;

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
