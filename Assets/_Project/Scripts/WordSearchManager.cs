using PatternLibrary;
using System.Collections.Generic;
using UnityEngine;

public class WordSearchManager : Singleton<WordSearchManager>
{
    private Trie wordTrie;
    private List<Word> wordsToType = new List<Word>();
    private List<string> stringsToType = new List<string>();

    private void Start()
    {
        wordTrie = new Trie();
    }

    public void AddWord(Word word)
    {
        wordsToType.Add(word);
        stringsToType.Add(word.word.ToUpper());
        wordTrie.Insert(word.word);
    }

    public void RemoveWord(Word word)
    {
        wordsToType.Remove(word);
        stringsToType.Remove(word.word.ToUpper());
        wordTrie.Remove(word.word);
    }

    public Word WordFound(string word)
    {
        int index = stringsToType.IndexOf(word);
        if (index != -1)
        {
            Word foundWord = wordsToType[index];
            return foundWord;
        }
        else
        {
            Debug.LogError("Something went wrong in WordSearchManager");
            return null;
        }
    }

    public bool CheckWord(string selectedWord)
    {
        if (wordTrie.Search(selectedWord))
        {
            Debug.Log("Word found in trie");
            return true;
        }
        else
        {
            Debug.Log("Word not found in trie");
            return false;
        }
    }
}
