using UnityEngine;

public class WordBall : WordObject
{
    protected override void Awake()
    {
        base.Awake();
        materialColor = Color.green;
        SetColor(materialColor);
    }
}
