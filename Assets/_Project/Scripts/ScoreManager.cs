using PatternLibrary;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Combo Settings & Attributes")]
    public int comboCounter = 0;
    [SerializeField] private int highestCombo = 0;
    [SerializeField] private int comboBonus = 20;
    [SerializeField] private Color comboColor1;
    [SerializeField] private Color comboColor2;
    [SerializeField] private Color comboColor3;
    [SerializeField] private Color comboColor4;
    [SerializeField] private GameObject mistakeHurtPanel;
    [SerializeField] private AnimationClip hurtAnimation;
    [SerializeField] private AnimationClip cameraShake;
    private int mistakesLastWord = 0;
    private int totalBonusPoints = 0;

    [Header("Combo References")]
    public GameObject comboText;
    [SerializeField] private GameObject comboSpawnPoint;
    [SerializeField] private GameObject comboFallingText;

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
    [SerializeField] private TextMeshProUGUI highestComboText;
    [SerializeField] private TextMeshProUGUI highestComboTextHeader;
    [SerializeField] private Sprite gradeSSprite, gradeASprite, gradeBSprite, gradeCSprite, gradeDSprite, gradeFSprite;
    [SerializeField] private GameObject gradeSpriteImage;
    [SerializeField] private GameObject fallingText;
    [SerializeField] private GameObject pointSpawnPoint;
    public GameObject scoreTracker;
    public TextMeshProUGUI mistakesTracker;

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
        highestComboText.enabled = false;
        highestComboTextHeader.enabled = false;

    }


    public void CheckCombo()
    {
        if (comboCounter > highestCombo)
        {
            highestCombo = comboCounter;
        }
        if (mistakesLastWord != mistakesCounter)
        {
            comboCounter = 0;
        }
        else
        {
            comboCounter++;
        }

        mistakesLastWord = mistakesCounter;
        DisplayCombo();
    }


    private void Update()
    {
        TextMeshProUGUI comboTextMesh = comboText.GetComponent<TextMeshProUGUI>();
        if (comboCounter < 3)
        {
            // Smooth transition between comboColor3 and comboColor2
            float t = Mathf.PingPong(Time.time, 1); // Cycles between 0 and 1 over time
            comboTextMesh.color = Color.Lerp(Color.white, comboColor1, t);
        }
        else if (comboCounter >= 3 && comboCounter < 5)
        {
            float t = Mathf.PingPong(Time.time, 1); // Cycles between 0 and 1 over time
            comboTextMesh.color = Color.Lerp(comboColor1, comboColor2, t);
        }
        else if (comboCounter >= 5 && comboCounter < 10)
        {
            float t = Mathf.PingPong(Time.time, 1); // Cycles between 0 and 1 over time
            comboTextMesh.color = Color.Lerp(comboColor2, comboColor3, t);
        }
        else // 10 or higher
        {
            float t = Mathf.PingPong(Time.time, 1); // Cycles between 0 and 1 over time
            comboTextMesh.color = Color.Lerp(comboColor3, comboColor4, t);
        }
    }

    public void DisplayCombo()
    {
        if (comboCounter == 0)
        {
            comboText.SetActive(false);
            return;
        }
        else
        {
            comboText.SetActive(true);
        }

        TextMeshProUGUI comboTMP = comboText.GetComponent<TextMeshProUGUI>();
        comboTMP.text = $"x{comboCounter}";
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
        CheckCombo();
        Debug.Log(comboCounter);
        if (comboCounter > 0)
        {
            TextMeshProUGUI fallingComboTextInstance = Instantiate(comboFallingText, new Vector3(scoreTracker.transform.position.x + 10, scoreTracker.transform.position.y - 100, scoreTracker.transform.position.z), Quaternion.identity, comboSpawnPoint.transform).GetComponent<TextMeshProUGUI>();
            string comboPointsAdded = $"+{comboBonus * comboCounter}";
            totalBonusPoints += comboBonus * comboCounter;
            UpdateScore();
            fallingComboTextInstance.text = comboPointsAdded;
            fallingComboTextInstance.color = comboText.GetComponent<TextMeshProUGUI>().color;
            Destroy(fallingComboTextInstance.gameObject, 3f);
        }
        UpdateMistakesTracker();
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
        StartCoroutine(HurtAnimation());
        CheckCombo();
        mistakesText.text = mistakesCounter.ToString();
        TextMeshProUGUI fallingTextInstance = Instantiate(fallingText, new Vector3(scoreTracker.transform.position.x + 10, scoreTracker.transform.position.y - 100, scoreTracker.transform.position.z), Quaternion.identity, pointSpawnPoint.transform).GetComponent<TextMeshProUGUI>();
        fallingTextInstance.text = $"-{mistakePenalty}";
        fallingTextInstance.color = Color.red;
        UpdateMistakesTracker();
        UpdateScore();
    }

    private IEnumerator HurtAnimation()
    {
        Animator hurtAnimator = mistakeHurtPanel.GetComponent<Animator>();
        mistakeHurtPanel.SetActive(true);
        hurtAnimator.Play(hurtAnimation.name.ToString());
        Camera.main.GetComponent<Animator>().SetTrigger("mistakeMade");
        yield return new WaitForSeconds(hurtAnimation.length);
        mistakeHurtPanel.SetActive(false);
    }

    public void UpdateMistakesTracker()
    {
        if (wordsTotalCounter > 0)
        {
            float percentage = PercentMistakes();
            mistakesTracker.text = $"Mistakes: {mistakesCounter.ToString()} ({percentage.ToString("0.")}%)";
        }
        else if (mistakesCounter >= wordsTotalCounter)
        {
            mistakesTracker.text = $"Mistakes: {mistakesCounter.ToString()} (100%)";
        }
    }

    public float PercentMistakes()
    {
        float percentage = ((float)mistakesCounter / wordsTotalCounter) * 100;
        percentage = Mathf.Clamp(percentage, 0, 100);
        return percentage;
    }

    public void UpdateScore()
    {
        totalScore = (easyWordsCounter * easyWordsScore) + (mediumWordsCounter * mediumWordsScore) + (hardWordsCounter * hardWordsScore) - (mistakesCounter * mistakePenalty) + totalBonusPoints;
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
                SFXManager.Instance.PlaySFX(5, false, true);
                break;
            case 'A':
                gradeSpriteImage.GetComponent<Image>().sprite = gradeASprite;
                SFXManager.Instance.PlaySFX(4, false, true);
                break;
            case 'B':
                gradeSpriteImage.GetComponent<Image>().sprite = gradeBSprite;
                SFXManager.Instance.PlaySFX(3, false, true);
                break;
            case 'C':
                gradeSpriteImage.GetComponent<Image>().sprite = gradeCSprite;
                SFXManager.Instance.PlaySFX(2, false, true);
                break;
            case 'D':
                gradeSpriteImage.GetComponent<Image>().sprite = gradeDSprite;
                break;
            case 'F':
                gradeSpriteImage.GetComponent<Image>().sprite = gradeFSprite;
                SFXManager.Instance.PlaySFX(6, false, true);
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
        mistakesText.text = $"{mistakesCounter}";

        highestComboText.text = $"x{highestCombo}";

        scoreTracker.SetActive(false);
        mistakesTracker.gameObject.SetActive(false);
        comboText.SetActive(false);

        StartCoroutine(DisplayScoreDelayed(1.5f));
    }

    private IEnumerator DisplayScoreDelayed(float delay)
    {
        scorePanel.SetActive(true);
        yield return new WaitForSecondsRealtime(delay);
        StartCoroutine(ShowScoreAnimation(easyWordsText, easyWordsHeader, realTimeBetweenLetters)); // start coroutine chain
    }

    public IEnumerator ShowScoreAnimation(TextMeshProUGUI text, TextMeshProUGUI textHeader, float realTimeBetweenLetters)
    {
        text.enabled = true;
        textHeader.enabled = true;

        char[] randomLetters = { '#', '@', '&', '%', '$', '?' };

        // Preserve the original text content
        string originalText = text.text;

        // Ensure the dynamic text has valid content
        if (string.IsNullOrEmpty(originalText))
        {
            originalText = "?"; // Default to "?" if empty
        }

        // Split the original dynamic text into characters for animation
        char[] textSplit = originalText.ToCharArray();
        Color defaultTextColor = text.color;

        // Replace all dynamic content with spaces initially
        for (int i = 0; i < textSplit.Length; i++)
        {
            textSplit[i] = ' ';
        }

        Debug.Log($"Dynamic text length: {textSplit.Length}");

        int iterationCounter = 0;
        foreach (char _ in textSplit)
        {
            for (int i = 0; i < randomLetters.Length; i++)
            {
                // Replace the current character with a random letter
                textSplit[iterationCounter] = randomLetters[i];
                text.text = new string(textSplit); // Update dynamic text only

                // Assign a random color while preserving alpha
                Color randomColor = Random.ColorHSV();
                text.color = new Color(randomColor.r, randomColor.g, randomColor.b, defaultTextColor.a);

                yield return new WaitForSecondsRealtime(realTimeBetweenLetters);
            }

            // Restore the original character at the current position
            if (iterationCounter < originalText.Length)
            {
                textSplit[iterationCounter] = originalText[iterationCounter];
            }

            iterationCounter++;
        }
        SFXManager.Instance.PlaySFX(7, false);

        // Reset the text color and restore the full original dynamic text
        text.color = defaultTextColor;
        text.text = originalText;

        // Continue the coroutine chain
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
            mistakesText.text += $" ({PercentMistakes().ToString("0.")}%)";
            StartCoroutine(ShowScoreAnimation(highestComboText, highestComboTextHeader, realTimeBetweenLetters));
        }
        else if (text == highestComboText)
        {
            StartCoroutine(ShowScoreAnimation(scoreText, scoreTextHeader, realTimeBetweenLetters));
        }
        else if (text == scoreText)
        {
            yield return new WaitForSeconds(1f);
            CalculateGrade(GameManager.Instance.levelDifficulty, totalScore);
            DisplayGrade();
        }
    }

}
