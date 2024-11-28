using System.Collections.Generic;

public class TrieNode
{
    public Dictionary<char, TrieNode> Children = new Dictionary<char, TrieNode>();
    public bool IsEndOfWord = false;
    public int WordCount = 0;
}

public class Trie
{
    private readonly TrieNode root;

    public Trie()
    {
        root = new TrieNode();
    }

    public void Insert(string word)
    {
        var currentNode = root;
        foreach (var letter in word)
        {
            if (!currentNode.Children.ContainsKey(letter))
            {
                currentNode.Children[letter] = new TrieNode();
            }
            currentNode = currentNode.Children[letter];
        }
        currentNode.IsEndOfWord = true;
        currentNode.WordCount++;
    }

    public bool Search(string word)
    {
        var currentNode = root;
        foreach (var letter in word)
        {
            if (!currentNode.Children.ContainsKey(letter))
            {
                return false;
            }
            currentNode = currentNode.Children[letter];
        }
        return currentNode.IsEndOfWord;
    }

    public void Remove(string word)
    {
        if (string.IsNullOrEmpty(word))
        {
            return;
        }
        RemoveHelper(root, word, 0);
    }

    private bool RemoveHelper(TrieNode currentNode, string word, int index)
    {
        if (index == word.Length)
        {
            if (!currentNode.IsEndOfWord)
            {
                return false;
            }
            currentNode.WordCount--;
            if (currentNode.WordCount == 0)
            {
                currentNode.IsEndOfWord = false;
            }
            return currentNode.Children.Count == 0 && currentNode.WordCount == 0;
        }

        char letter = word[index];
        if (!currentNode.Children.ContainsKey(letter))
        {
            return false;
        }

        bool shouldDeleteChildNode = RemoveHelper(currentNode.Children[letter], word, index + 1);

        if (shouldDeleteChildNode)
        {
            currentNode.Children.Remove(letter);
            return !currentNode.IsEndOfWord && currentNode.Children.Count == 0;
        }
        return false;
    }
}
