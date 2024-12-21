using System.Collections;
using TMPro;
using UnityEngine;

public class WordObject : MonoBehaviour, IWord, ITextHolder
{
    private TextMeshPro wordText;
    protected Color materialColor = Color.white;
    private Rigidbody rb;
    [SerializeField] private GameObject explosionPrefab;
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

    private void FixedUpdate()
    {
        var currentVelocity = rb.velocity;

        if (currentVelocity.y <= 0f)
        {
            return;
        }

        currentVelocity.y = 0f;

        rb.velocity = currentVelocity;
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

    public void SpawnExplosion()
    {
        GameObject explosionInstance = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosionInstance, 3f);
    }
}
