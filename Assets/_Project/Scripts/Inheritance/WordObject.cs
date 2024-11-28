using TMPro;
using UnityEngine;

public class WordObject : MonoBehaviour, IWord, ITextHolder
{
    private TextMeshPro wordText;
    protected Color materialColor = Color.white;
    protected virtual void Awake()
    {
        AssignTextReference();
        SetWord();
        SetColor(materialColor);
    }
    public void AssignTextReference()
    {
        wordText = GetComponent<TextMeshPro>();
        if (wordText == null)
        {
            wordText = GetComponentInChildren<TextMeshPro>();
            if (wordText == null)
            {
                Debug.LogError("False text holder - Destroying Object");
                Destroy(gameObject);
            }
        }
    }

    public void SetWord()
    {
        wordText.text = WordManager.Instance.PickRandomWordWeighted().word;
    }

    protected void SetColor(Color color)
    {
        GetComponent<MeshRenderer>().material.color = color;
    }
}
