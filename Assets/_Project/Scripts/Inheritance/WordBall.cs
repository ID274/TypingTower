using UnityEngine;

public class WordBall : WordObject
{
    [SerializeField] private Color objectColor;
    protected override void Awake()
    {
        base.Awake();
        materialColor = objectColor;
        SetMaterialColor(materialColor);
    }
}
