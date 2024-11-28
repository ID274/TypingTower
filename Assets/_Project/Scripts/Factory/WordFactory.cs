using PatternLibrary;
using UnityEngine;

public class WordFactory : Singleton<WordFactory>
{
    [Header("Rotation Attributes")]
    public float minZRotation = -40f;
    public float maxZRotation = 15f;
    public float minYRotation = -35f;
    public float maxYRotation = 40f;

    public IWord CreateWordObject(GameObject prefab, Vector3 position)
    {
        float zAngle = Random.Range(minZRotation, maxZRotation);
        float yAngle = Random.Range(minYRotation, maxYRotation);

        GameObject createdInstance = Instantiate(prefab, position, Quaternion.Euler(0, yAngle, zAngle));

        switch (GameManager.Instance.onlyBottomWordTypeable)
        {
            case true:
                WordManager.Instance.queueOfWordObjects.Enqueue(createdInstance);
                break;
            case false:
                WordManager.Instance.listOfWordObjects.Add(createdInstance);
                break;
        }
        return createdInstance?.GetComponent<IWord>();
    }
}
