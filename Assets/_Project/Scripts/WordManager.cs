using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PatternLibrary;
using System.Linq;
using UnityEditor;

public class WordManager : Singleton<WordManager>
{
    [Header("Word List References")]
    public WordList[] availableWordLists;
    public Word[] words;
    private WordList loadedWordList;

    [Header("Attributes")]
    public int indexToLoad = 0;
    [Tooltip("Lower to increase harder odds, increase to increase easier odds")] public float difficultyBias = 1f; // lower number means harder words are more likely to be picked (higher indices in the array), and vice verse. 1 is standard

    [Header("Queue/List Logic")]
    public Queue<Word> wordQueue = new Queue<Word>();
    public List<Word> wordList = new List<Word>();
    public Queue<GameObject> queueOfWordObjects = new Queue<GameObject>();  
    public List<GameObject> listOfWordObjects = new List<GameObject>();


    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        SortWords();
    }

    public Word PickRandomWord()
    {
        // Check if all words have already been added to the queue
        if (wordQueue.Count >= words.Length)
        {
            Debug.LogWarning("All words have already been added to the queue.");
            GameManager.Instance.GameOver();
            return null;
        }

        // Initialize variables
        int attempts = 0; // Track attempts to prevent infinite loops
        const int maxAttempts = 100; // Define a maximum number of attempts
        Word wordPicked = null;
        bool wordFound = false;

        while (!wordFound && attempts < maxAttempts)
        {
            // Pick a random word
            wordPicked = words[Random.Range(0, words.Length)];

            // Check if the word is already in the queue
            if (!wordQueue.Contains(wordPicked))
            {
                wordFound = true; // Exit the loop if a valid word is found
            }
            else
            {
                attempts++; // Increment attempts if the word is already in the queue
            }
        }

        // Handle cases where a valid word couldn't be found
        if (!wordFound)
        {
            Debug.LogWarning("Could not find a new word after max attempts - Check difficultyBias value isn't 0 or too low");
            GameManager.Instance.GameOver();
            return null;
        }

        // Add the word to the queue and return it
        switch (GameManager.Instance.onlyBottomWordTypeable)
        {
            case true:
                wordQueue.Enqueue(wordPicked);
                break;
            case false:
                wordList.Add(wordPicked);
                break;
        }
        Debug.Log($"Word picked: {wordPicked.name}");
        WordSearchManager.Instance.AddWord(wordPicked);
        return wordPicked;
    }


    public Word PickRandomWordWeighted()
    {
        // Check if the wordQueue already contains all available words
        if (wordQueue.Count >= words.Length)
        {
            Debug.LogWarning("All words have already been added to the queue.");
            GameManager.Instance.GameOver();
            return null;
        }

        // Initialize variables
        int attempts = 0; // Track attempts to prevent infinite loops
        const int maxAttempts = 100; // Define a maximum number of attempts
        Word wordPicked = null;
        bool wordFound = false;

        while (!wordFound && attempts < maxAttempts)
        {
            // Pick a random word index based on the difficulty bias
            int randomIndexWeighted = Mathf.RoundToInt(Mathf.Pow(Random.Range(0f, 1f), difficultyBias) * (words.Length - 1));
            wordPicked = words[randomIndexWeighted];

            // Check if the word is already in the queue
            if (!wordQueue.Contains(wordPicked))
            {
                wordFound = true; // Exit the loop if a valid word is found
            }
            else
            {
                attempts++;
            }
        }

        // Handle cases where a valid word couldn't be found
        if (!wordFound)
        {
            Debug.LogWarning("Could not find a new word after max attempts - Check difficultyBias value isn't 0 or too low");
            GameManager.Instance.GameOver();
            return null;
        }

        // Add the word to the queue and return it
        switch (GameManager.Instance.onlyBottomWordTypeable)
        {
            case true:
                wordQueue.Enqueue(wordPicked);
                break;
            case false:
                wordList.Add(wordPicked);
                break;
        }
        Debug.Log($"Word picked: {wordPicked.name}");
        WordSearchManager.Instance.AddWord(wordPicked);
        return wordPicked;
    }


    public void RemoveWord(Word word)
    {
        switch (GameManager.Instance.onlyBottomWordTypeable)
        {
            case true:
                // Remove the first word in the queue
                if (wordQueue.Count > 0 && queueOfWordObjects.Count > 0)
                {
                    GameObject wordObject = queueOfWordObjects.Dequeue();
                    Destroy(wordObject);
                    wordQueue.Dequeue();
                }
                break;

            case false:
                // Remove a specific word and its associated object
                int index = wordList.IndexOf(word);
                if (index >= 0 && index < listOfWordObjects.Count)
                {
                    listOfWordObjects[index].GetComponent<WordObject>().SpawnExplosion();
                    Destroy(listOfWordObjects[index]);
                    listOfWordObjects.RemoveAt(index);
                    wordList.RemoveAt(index);
                }
                break;
        }

        // Notify the WordSearchManager to remove the word
        WordSearchManager.Instance.RemoveWord(word);
    }

    public WordDifficulty LoadWordList(WordList wordList)
    {
        loadedWordList = wordList;
        words = new Word[wordList.words.Length];

        for (int i = 0; i < wordList.words.Length; i++)
        {
            words[i] = wordList.words[i];
        }

        return wordList.estimatedDifficulty;
    }
    private void SortWords() // sorts the words by name length and then by difficulty
    {
        words = words.OrderBy(word => word.name.Length)
                     .ThenBy(word => word.difficulty)
                     .ToArray();
    }
}
