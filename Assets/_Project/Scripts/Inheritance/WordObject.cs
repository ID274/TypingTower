using TMPro;
using UnityEngine;

public class WordObject : MonoBehaviour, IWord, ITextHolder
{
    private TextMeshPro wordText;
    protected Color materialColor = Color.white;
    private Rigidbody rb;
    protected virtual void Awake()
    {
        AssignTextReference();
        SetWord();
        SetMaterialColor(materialColor);
        rb = GetComponent<Rigidbody>();
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

    private void FixedUpdate()
    {
        if (rb.velocity.y > 0)
        {
            rb.velocity = new Vector3 (rb.velocity.x, 0.1f, rb.velocity.z);
        }
    }
}
