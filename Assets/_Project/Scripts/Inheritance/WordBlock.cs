using UnityEngine;

public class WordBlock : WordObject
{
    [SerializeField] private Color objectColor;
    [SerializeField] private bool randomColor;
    protected override void Awake()
    {
        base.Awake();
        
        switch (randomColor)
        {
            case true:
                objectColor = RandomColor();
                break;
            case false:
                break;
        }
        materialColor = objectColor;

        SetMaterialColor(materialColor);
    }

    private Color RandomColor()
    {
        Color color = new Color();
        color.r = Random.Range(0, 255);
        color.g = Random.Range(0, 255);
        color.b = Random.Range(0, 255);
        color.a = 1;
        Debug.Log($"Color {color})");
        return color;
    }
}
