using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Word", menuName = "Words/Word", order = 2)]
public class Word : ScriptableObject
{
    public string word;
    public string additionalText;
    public WordDifficulty difficulty;
}
