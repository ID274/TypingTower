using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PatternLibrary;
using TMPro;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System;

public class ScoreManager : Singleton<ScoreManager>
{
    [Header("Score Attributes")]
    private int easyWordsCounter = 0;
    private int mediumWordsCounter = 0;
    private int hardWordsCounter = 0;
    private int mistakesCounter = 0;

    [Header("Score Settings")]
    public int totalScore = 0;
    private int easyWordsScore = 100;
    private int mediumWordsScore = 200;
    private int hardWordsScore = 300;

    [Header("Grade Settings")]
    public char[] availableGrades = { 'S', 'A', 'B', 'C', 'D', 'F' };
    public char gradeAwarded = 'F';
    int scoreRequiredForS = 5000;
    int mistakesAllowedForS = 0;
    int scoreRequiredForA = 4000;
    int mistakesAllowedForA = 5;
    int scoreRequiredForB = 3000;
    int mistakesAllowedForB = 10;
    int scoreRequiredForC = 2000;
    int mistakesAllowedForC = 15;
    int scoreRequiredForD = 1000;
    int mistakesAllowedForD = 20;

    [Header("Show Text Animation")]
    [SerializeField] private float realTimeBetweenLetters = 0.2f;

    [Header("References")]
    [SerializeField] private GameObject scorePanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI easyWordsText;
    [SerializeField] private TextMeshProUGUI mediumWordsText;
    [SerializeField] private TextMeshProUGUI hardWordsText;
    [SerializeField] private TextMeshProUGUI mistakesText;

    protected override void Awake()
    {
        base.Awake();
        scorePanel.SetActive(false);
    }


    public void AddWord(WordDifficulty difficulty)
    {
        switch (difficulty)
        {
            case WordDifficulty.Easy:
                AddEasyWord();
                break;
            case WordDifficulty.Medium:
                AddMediumWord();
                break;
            case WordDifficulty.Hard:
                AddHardWord();
                break;
            default:
                Debug.LogWarning("Invalid word difficulty: " + difficulty);
                break;
        }
    }

    public void AddEasyWord()
    {
        easyWordsCounter++;
        easyWordsText.text = easyWordsCounter.ToString();
        UpdateScore();
    }

    public void AddMediumWord()
    {
        mediumWordsCounter++;
        mediumWordsText.text = mediumWordsCounter.ToString();
        UpdateScore();
    }

    public void AddHardWord()
    {
        hardWordsCounter++;
        hardWordsText.text = hardWordsCounter.ToString();
        UpdateScore();
    }

    public void AddMistake()
    {
        mistakesCounter++;
        mistakesText.text = mistakesCounter.ToString();
        UpdateScore();
    }

    public void UpdateScore()
    {
        totalScore = (easyWordsCounter * easyWordsScore) + (mediumWordsCounter * mediumWordsScore) + (hardWordsCounter * hardWordsScore);
    }

    public char CalculateGrade(WordDifficulty difficulty, int totalScore)
    {
        switch (difficulty)
        {
            case WordDifficulty.Easy:
                break;
            case WordDifficulty.Medium:
                scoreRequiredForS = scoreRequiredForS * 2;
                scoreRequiredForA = scoreRequiredForA * 2;
                scoreRequiredForB = scoreRequiredForB * 2;
                scoreRequiredForC = scoreRequiredForC * 2;
                scoreRequiredForD = scoreRequiredForD * 2;
                break;
            case WordDifficulty.Hard:
                scoreRequiredForS = scoreRequiredForS * 3;
                scoreRequiredForA = scoreRequiredForA * 3;
                scoreRequiredForB = scoreRequiredForB * 3;
                scoreRequiredForC = scoreRequiredForC * 3;
                scoreRequiredForD = scoreRequiredForD * 3;
                break;
        }

        if (totalScore >= scoreRequiredForS)
        {
            if (mistakesCounter <= mistakesAllowedForS)
            {
                gradeAwarded = availableGrades[0];
            }
            else
            {
                gradeAwarded = availableGrades[1];
            }
        }
        else if (totalScore >= scoreRequiredForA)
        {
            if (mistakesCounter <= mistakesAllowedForA)
            {
                gradeAwarded = availableGrades[1];
            }
            else
            {
                gradeAwarded = availableGrades[2];
            }
        }
        else if (totalScore >= scoreRequiredForB)
        {
            if (mistakesCounter <= mistakesAllowedForB)
            {
                gradeAwarded = availableGrades[2];
            }
            else
            {
                gradeAwarded = availableGrades[3];
            }
        }
        else if (totalScore >= scoreRequiredForC)
        {
            if (mistakesCounter <= mistakesAllowedForC)
            {
                gradeAwarded = availableGrades[3];
            }
            else
            {
                gradeAwarded = availableGrades[4];
            }
        }
        else if (totalScore >= scoreRequiredForD)
        {
            if (mistakesCounter <= mistakesAllowedForD)
            {
                gradeAwarded = availableGrades[4];
            }
            else
            {
                gradeAwarded = availableGrades[5];
            }
        }
        else
        {
            gradeAwarded = availableGrades[5];
        }

        return gradeAwarded;
    }

    public void DisplayGrade()
    {
        CalculateGrade(WordDifficulty.Easy, totalScore);
        Debug.Log("Grade awarded: " + gradeAwarded);
        // display grade here
    }

    public IEnumerator DisplayScore()
    {
        scorePanel.SetActive(true);

        StartCoroutine(ShowScoreAnimation(easyWordsText, realTimeBetweenLetters));
        StartCoroutine(ShowScoreAnimation(mediumWordsText, realTimeBetweenLetters));
        StartCoroutine(ShowScoreAnimation(hardWordsText, realTimeBetweenLetters));
        StartCoroutine(ShowScoreAnimation(mistakesText, realTimeBetweenLetters));
        
        yield return new WaitForSeconds(1f);
        StartCoroutine(ShowScoreAnimation(scoreText, realTimeBetweenLetters));

        CalculateGrade(GameManager.Instance.levelDifficulty, totalScore);
        DisplayGrade();
    }

    public IEnumerator ShowScoreAnimation(TextMeshProUGUI text, float realTimeBetweenLetters)
    {
        char[] randomLetters = { '#', '@', '&', '%', '$', '?', '|' };

        if (int.TryParse(text.text, out int textNumbers))
        {
            Debug.Log("Parsed number: " + textNumbers);
        }
        else
        {
            Debug.LogWarning("Invalid number format in text: " + text.text);
        }

        Color defaultTextColor = text.color;
        foreach (char letter in text.text)
        {
            for (int i = 0; i < randomLetters.Length; i++)
            {
                text.text = text.text.Replace(letter, randomLetters[i]);
                text.color = UnityEngine.Random.ColorHSV();
                yield return new WaitForSecondsRealtime(realTimeBetweenLetters);
            }
        }
        text.color = defaultTextColor;
        text.text = textNumbers.ToString();
    }
}
