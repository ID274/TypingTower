using UnityEngine;

[CreateAssetMenu(fileName = "WordList", menuName = "Words/WordList", order = 1)]
public class WordList : ScriptableObject
{
    public string listName = "Placeholder";
    public Word[] words;
    public WordDifficulty estimatedDifficulty;
}
