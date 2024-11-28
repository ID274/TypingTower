using PatternLibrary;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>   
{
    [Header("Level Details")]
    public WordDifficulty levelDifficulty;

    [Header("Word Spawner")]
    public Transform spawnPoint;
    public float spawnInterval = 3f;
    public GameObject wordObjectPrefab;
    private bool canSpawn = true;

    [Header("Game Speed")]
    public float gameSpeed = 1f;
    public float speedIncreaseStep = 1.0001f;
    public float maxSpeed = 3f;
    public int lossCountdownTime = 3;

    [Header("Game State")]
    public GameState gameState = GameState.Paused;

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
    }

    private void Update()
    {
        if (canSpawn && gameState == GameState.Playing)
        {
            StartCoroutine(SpawnCooldown());
            var wordInstance = WordFactory.Instance.CreateWordObject(wordObjectPrefab, spawnPoint.position);
        }
    }

    public IEnumerator SpawnCooldown()
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnInterval);
        canSpawn = true;
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

    private float IncrementGameSpeed()
    {
        gameSpeed *= speedIncreaseStep;
        Mathf.Clamp(gameSpeed, 0, maxSpeed);
        return gameSpeed;
    }

    public void GameOver()
    {
        gameState = GameState.GameOver;
        InputManager.Instance.inputField.enabled = false;
    }

    public void StartGame()
    {
        gameState = GameState.Playing;
        InputManager.Instance.inputField.enabled = true;
    }

    public void PauseGame()
    {
        gameState = GameState.Paused;
        Time.timeScale = 0;
        InputManager.Instance.inputField.enabled = false;
    }

    public void ResumeGame()
    {
        gameState = GameState.Playing;
        Time.timeScale = GameManager.Instance.gameSpeed;
        InputManager.Instance.inputField.enabled = true;
    }

    public void RestartGame()
    {
        gameState = GameState.PreStart;
        Time.timeScale = 1;
        InputManager.Instance.inputField.enabled = true;
    }

    public void WordHasBeenFound(Word word)
    {
        WordManager.Instance.RemoveWord(word);
        // do some fancy stuff
    }

}
