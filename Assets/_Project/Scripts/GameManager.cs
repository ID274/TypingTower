using PatternLibrary;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>   
{
    [Header("Application Attributes")]
    public bool useOnScreenKeyboard = true;

    [Header("Level Details")]
    public WordDifficulty levelDifficulty;
    private int levelIndex = 0;
    public int seed;

    [Header("Word Spawner")]
    public Transform spawnPoint;
    public float spawnInterval = 3f;
    public GameObject wordObjectPrefab;
    private bool canSpawn = true;
    [SerializeField] private float initialDelay = 1f;

    [Header("Game Speed")]
    public float assignedGameSpeed = 1f;
    private float gameSpeed = 1f;
    public float speedIncreaseStep = 1.0001f;
    public float maxSpeed = 3f;
    public int lossCountdownTime = 3;

    [Header("Game State")]
    public GameState gameState = GameState.PreStart;
    private GameState previousState = GameState.PreStart;
    private float previousTimeScale;
    [SerializeField] private GameObject restartButton;

    [Header("Trie Settings")]
    public bool onlyBottomWordTypeable = true;

    public enum GameState
    {
        PreStart, // when level is first loaded and awaits input from player
        Playing,
        Paused,
        GameOver
    }

    protected override void Awake()
    {
        base.Awake();
        levelDifficulty = WordManager.Instance.LoadWordList(WordManager.Instance.availableWordLists[WordManager.Instance.indexToLoad]);
        levelIndex = WordManager.Instance.indexToLoad;
        gameSpeed = assignedGameSpeed;
        previousTimeScale = Time.timeScale;
        //SetResolution(585, 1266, true);
    }

    private void Update()
    {
        if (canSpawn && gameState == GameState.Playing)
        {
            StartCoroutine(SpawnCooldown());
            StartCoroutine(SpawnWordObject());
        }
    }

    public IEnumerator SpawnCooldown()
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnInterval);
        canSpawn = true;
    }

    public IEnumerator SpawnWordObject()
    {
        yield return new WaitForSecondsRealtime(initialDelay);
        var wordInstance = WordFactory.Instance.CreateWordObject(wordObjectPrefab, spawnPoint.position);
    }

    private void FixedUpdate()
    {
        switch (gameState)
        {
            case GameState.PreStart:
                // pre-start behaviour
                break;
            case GameState.Paused:
                // paused behaviour
                break;
            case GameState.Playing:
                Time.timeScale = IncrementGameSpeed();
                break;
            case GameState.GameOver:
                // game over behaviour
                break;
        }
    }

    public static void SetResolution(int width, int height, bool fullscreen)
    {
        Screen.SetResolution(width, height, fullscreen);
    }

    private float IncrementGameSpeed()
    {
        gameSpeed *= speedIncreaseStep;
        Mathf.Clamp(gameSpeed, 0, maxSpeed);
        return gameSpeed;
    }

    public void GameOver()
    {
        gameState = GameState.GameOver;
        Time.timeScale = 1;
        MusicManager.Instance.StartCoroutine(MusicManager.Instance.FadeOutCurrentTrack());
        UIManager.Instance.GameOverBehaviour();
        InputManager.Instance.inputField.enabled = false;
        StartCoroutine(DelayScore(2f));
    }

    private IEnumerator DelayScore(float delay)
    {
        yield return new WaitForSeconds(delay);
        ScoreManager.Instance.DisplayScore();
    }

    public void StartGame()
    {
        gameState = GameState.Playing;
        if (!MusicManager.Instance.audioSource.isPlaying)
        {
            MusicManager.Instance.PlayMusic(0, true);
        }
        UIManager.Instance.EnableUI();
        InputManager.Instance.inputField.enabled = true;
    }

    public void PauseGame()
    {
        previousState = gameState;
        previousTimeScale = Time.timeScale;
        gameState = GameState.Paused;
        Time.timeScale = 0;
        InputManager.Instance.inputField.enabled = false;
        UIManager.Instance.unPauseButton.SetActive(true);
        UIManager.Instance.pauseButton.SetActive(false);
    }

    public void ResumeGame()
    {
        if (previousState != GameState.Paused)
        {
            gameState = previousState;
        }
        else
        {
            gameState = GameState.Playing;
        }
        if (previousTimeScale != 0)
        {
            Time.timeScale = previousTimeScale;
        }
        else
        {
            Time.timeScale = gameSpeed;
        }
        InputManager.Instance.inputField.enabled = true;
        UIManager.Instance.unPauseButton.SetActive(false);
        UIManager.Instance.pauseButton.SetActive(true);
    }

    public void RestartGame()
    {
        MySceneManager.Instance.ReloadCurrentScene();
        //gameState = GameState.PreStart;
        //Time.timeScale = 1;
        //InputManager.Instance.inputField.enabled = true;
    }

    public IEnumerator AllowRestart()
    {
        yield return new WaitForSecondsRealtime(1f);
        restartButton.SetActive(true);
    }

    public void WordHasBeenFound(Word word)
    {
        WordManager.Instance.RemoveWord(word);
        ScoreManager.Instance.AddWord(word.difficulty);
        // do some fancy stuff
    }

}
