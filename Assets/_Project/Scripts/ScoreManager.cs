using PatternLibrary;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class ScoreManager : Singleton<ScoreManager>
{
    [Header("Score Attributes")]
    [SerializeField] private int wordsTotalCounter = 0;
    private int easyWordsCounter = 0;
    private int mediumWordsCounter = 0;
    private int hardWordsCounter = 0;
    [SerializeField] private int mistakesCounter = 0;

    [Header("Score Settings")]
    public int totalScore = 0;
    [SerializeField] private int easyWordsScore = 100;
    [SerializeField] private int mediumWordsScore = 200;
    [SerializeField] private int hardWordsScore = 300;
    [SerializeField] private int mistakePenalty = 50;

    [Header("Grade Settings")]
    public char gradeAwarded = 'F';
    (char, int, int) gradeS = ('S', 5000, 0); // GRADE | SCORE REQUIRED | MISTAKES ALLOWED
    (char, int, int) gradeA = ('A', 4000, 5);
    (char, int, int) gradeB = ('B', 3000, 10);
    (char, int, int) gradeC = ('C', 2000, 15);
    (char, int, int) gradeD = ('D', 1000, 20);
    char gradeF = 'F';

    [Header("Show Text Animation")]
    [SerializeField] private float realTimeBetweenLetters = 0.2f;

    [Header("References")]
    [SerializeField] private GameObject scorePanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreTextHeader;
    [SerializeField] private TextMeshProUGUI easyWordsText;
    [SerializeField] private TextMeshProUGUI easyWordsHeader;
    [SerializeField] private TextMeshProUGUI mediumWordsText;
    [SerializeField] private TextMeshProUGUI mediumWordsHeader;
    [SerializeField] private TextMeshProUGUI hardWordsText;
    [SerializeField] private TextMeshProUGUI hardWordsHeader;
    [SerializeField] private TextMeshProUGUI mistakesText;
    [SerializeField] private TextMeshProUGUI mistakesTextHeader;
    [SerializeField] private Sprite gradeSSprite, gradeASprite, gradeBSprite, gradeCSprite, gradeDSprite, gradeFSprite;
    [SerializeField] private GameObject gradeSpriteImage;
    [SerializeField] private GameObject scoreTracker;
    [SerializeField] private GameObject fallingText;
    [SerializeField] private GameObject pointSpawnPoint;
    [SerializeField] private TextMeshProUGUI mistakesTracker;

    protected override void Awake()
    {
        base.Awake();
        gradeSpriteImage.SetActive(false);
        scorePanel.SetActive(false);

        //ensure text is disabled so we can reenable it later
        scoreText.enabled = false;
        scoreTextHeader.enabled = false;
        easyWordsText.enabled = false;
        easyWordsHeader.enabled = false;
        mediumWordsText.enabled = false;
        mediumWordsHeader.enabled = false;
        hardWordsText.enabled = false;
        hardWordsHeader.enabled = false;
        mistakesText.enabled = false;
        mistakesTextHeader.enabled = false;

    }


    public void AddWord(WordDifficulty difficulty)
    {
        SFXManager.Instance.PlaySFX(1, true);
        string pointsAdded = "";
        switch (difficulty)
        {
            case WordDifficulty.Easy:
                AddEasyWord();
                pointsAdded = $"+{easyWordsScore.ToString()}";
                break;
            case WordDifficulty.Medium:
                AddMediumWord();
                pointsAdded = $"+{mediumWordsScore.ToString()}";
                break;
            case WordDifficulty.Hard:
                AddHardWord();
                pointsAdded = $"+{hardWordsScore.ToString()}";
                break;
            default:
                Debug.LogWarning("Invalid word difficulty: " + difficulty);
                break;
        }
        TextMeshProUGUI fallingTextInstance = Instantiate(fallingText, new Vector3 (scoreTracker.transform.position.x + 10, scoreTracker.transform.position.y - 100, scoreTracker.transform.position.z), Quaternion.identity, pointSpawnPoint.transform).GetComponent<TextMeshProUGUI>();
        fallingTextInstance.text = pointsAdded;
        fallingTextInstance.color = Color.green;
        Destroy(fallingTextInstance.gameObject, 3f);
        wordsTotalCounter++;
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
        TextMeshProUGUI fallingTextInstance = Instantiate(fallingText, new Vector3(scoreTracker.transform.position.x + 10, scoreTracker.transform.position.y - 100, scoreTracker.transform.position.z), Quaternion.identity, pointSpawnPoint.transform).GetComponent<TextMeshProUGUI>();
        fallingTextInstance.text = $"-{mistakePenalty}";
        fallingTextInstance.color = Color.red;
        mistakesTracker.text = $"Mistakes: {mistakesCounter.ToString()}";
        UpdateScore();
    }

    public void UpdateScore()
    {
        totalScore = (easyWordsCounter * easyWordsScore) + (mediumWordsCounter * mediumWordsScore) + (hardWordsCounter * hardWordsScore) - (mistakesCounter * mistakePenalty);
        if (totalScore < 0)
        {
            totalScore = 0;
        }
        scoreText.text = totalScore.ToString();
        scoreTracker.GetComponent<TextMeshProUGUI>().text = $"Score: {totalScore}";
        Debug.Log("Updated score");
    }

    public char CalculateGrade(WordDifficulty difficulty, int totalScore)
    {
        switch (difficulty)
        {
            case WordDifficulty.Easy:
                break;
            case WordDifficulty.Medium:
                gradeS.Item2 *= 2;
                gradeA.Item2 *= 2;
                gradeB.Item2 *= 2;
                gradeC.Item2 *= 2;
                gradeD.Item2 *= 2;
                break;
            case WordDifficulty.Hard:
                gradeS.Item2 *= 3;
                gradeA.Item2 *= 3;
                gradeB.Item2 *= 3;
                gradeC.Item2 *= 3;
                gradeD.Item2 *= 3;
                break;
        }

        if (totalScore >= gradeS.Item2)
        {
            if (mistakesCounter == gradeS.Item3)
            {
                gradeAwarded = gradeS.Item1;
            }
            else
            {
                gradeAwarded = gradeA.Item1;
            }
        }
        else if (totalScore >= gradeA.Item2)
        {
            if (mistakesCounter <= (wordsTotalCounter * gradeA.Item3) / 100)
            {
                gradeAwarded = gradeA.Item1;
            }
            else
            {
                gradeAwarded = gradeB.Item1;
            }
        }
        else if (totalScore >= gradeB.Item2)
        {
            if (mistakesCounter <= (wordsTotalCounter * gradeB.Item3) / 100)
            {
                gradeAwarded = gradeB.Item1;
            }
            else
            {
                gradeAwarded = gradeC.Item1;
            }
        }
        else if (totalScore >= gradeC.Item2)
        {
            if (mistakesCounter <= (wordsTotalCounter * gradeC.Item3) / 100)
            {
                gradeAwarded = gradeC.Item1;
            }
            else
            {
                gradeAwarded = gradeD.Item1;
            }
        }
        else if (totalScore >= gradeD.Item2)
        {
            if (mistakesCounter <= (wordsTotalCounter * gradeD.Item3) / 100)
            {
                gradeAwarded = gradeD.Item1;
            }
            else
            {
                gradeAwarded = gradeF;
            }
        }
        else
        {
            gradeAwarded = gradeF;
        }

        return gradeAwarded;
    }

    public void DisplayGrade()
    {
        CalculateGrade(WordDifficulty.Easy, totalScore);

        switch (gradeAwarded)
        {
            case 'S':
                gradeSpriteImage.GetComponent<Image>().sprite = gradeSSprite;
                SFXManager.Instance.PlaySFX(5, false);
                break;
            case 'A':
                gradeSpriteImage.GetComponent<Image>().sprite = gradeASprite;
                SFXManager.Instance.PlaySFX(4, false);
                break;
            case 'B':
                gradeSpriteImage.GetComponent<Image>().sprite = gradeBSprite;
                SFXManager.Instance.PlaySFX(3, false);
                break;
            case 'C':
                gradeSpriteImage.GetComponent<Image>().sprite = gradeCSprite;
                SFXManager.Instance.PlaySFX(2, false);
                break;
            case 'D':
                gradeSpriteImage.GetComponent<Image>().sprite = gradeDSprite;
                break;
            case 'F':
                gradeSpriteImage.GetComponent<Image>().sprite = gradeFSprite;
                SFXManager.Instance.PlaySFX(6, false);
                break;
        }

        Debug.Log("Grade awarded: " + gradeAwarded);
        ShowGrade(true);
        StartCoroutine(GameManager.Instance.AllowRestart());
    }

    public void ShowGrade(bool animation)
    {
        switch (animation)
        {
            case true:
                // animation here
                gradeSpriteImage.SetActive(true);
                break;

            case false:
                gradeSpriteImage.SetActive(true);
                break;
        }
    }

    public void DisplayScore()
    {
        scoreTracker.SetActive(false);
        mistakesTracker.gameObject.SetActive(false);
        scorePanel.SetActive(true);

        StartCoroutine(ShowScoreAnimation(easyWordsText, easyWordsHeader, realTimeBetweenLetters)); // start coroutine chain
    }

    public IEnumerator ShowScoreAnimation(TextMeshProUGUI text, TextMeshProUGUI textHeader, float realTimeBetweenLetters)
    {
        text.enabled = true;
        textHeader.enabled = true;
        char[] randomLetters = { '#', '@', '&', '%', '$', '?' };
        if (text.text == 0.ToString() || text.text == default)
        {
            text.text = "0";
        }

        if (int.TryParse(text.text, out int textNumbers))
        {
            Debug.Log("Parsed number: " + textNumbers);
        }
        else
        {
            Debug.LogWarning("Invalid number format in text: " + text.text);
        }

        Color defaultTextColor = text.color;

        char[] textSplit = text.text.ToCharArray(); // deconstruct the text into separate strings
        
        for (int i = 0; i < textSplit.Length; i++)
        {
            textSplit[i] = ' ';
        }

        Debug.Log(textSplit.Length);

        int iterationCounter = 0;
        foreach (char character in textSplit)
        {
            char currentLetter = textSplit[iterationCounter];
            for (int i = 0; i < randomLetters.Length; i++)
            {
                currentLetter = randomLetters[i];
                textSplit[iterationCounter] = currentLetter;
                if (iterationCounter > 0)
                {
                    textSplit[iterationCounter - 1] = textNumbers.ToString()[iterationCounter - 1];
                }
                text.text = textSplit.ArrayToString();
                text.color = Random.ColorHSV();
                text.color.MinAlpha(defaultTextColor);
                yield return new WaitForSecondsRealtime(realTimeBetweenLetters);
            }
            iterationCounter++;
        }

        text.color = defaultTextColor;
        text.text = textNumbers.ToString();

        // start next coroutine if there is one

        if (text == easyWordsText)
        {
            StartCoroutine(ShowScoreAnimation(mediumWordsText, mediumWordsHeader, realTimeBetweenLetters));
        }
        else if (text == mediumWordsText)
        {
            StartCoroutine(ShowScoreAnimation(hardWordsText, hardWordsHeader, realTimeBetweenLetters));
        }
        else if (text == hardWordsText)
        {
            StartCoroutine(ShowScoreAnimation(mistakesText, mistakesTextHeader, realTimeBetweenLetters));
        }
        else if (text == mistakesText)
        {
            StartCoroutine(ShowScoreAnimation(scoreText, scoreTextHeader, realTimeBetweenLetters));
        }
        else if (text ==  scoreText)
        {
            yield return new WaitForSeconds(1f);
            CalculateGrade(GameManager.Instance.levelDifficulty, totalScore);
            DisplayGrade();
        }
    }
}
