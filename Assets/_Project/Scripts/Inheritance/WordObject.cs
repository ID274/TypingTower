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
        SetMaterialColor(materialColor);
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

    protected void SetMaterialColor(Color color)
    {
        MeshRenderer meshRend = GetComponent<MeshRenderer>();
        Material mat = meshRend.material;
        mat.color = color;
    }
}
