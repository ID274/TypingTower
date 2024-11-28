using UnityEngine;

public class WordBlock : WordObject
{
    protected override void Awake()
    {
        base.Awake();
        materialColor = Color.red;
        SetColor(materialColor);
    }
}
