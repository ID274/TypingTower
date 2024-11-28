using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class Key : MonoBehaviour, IKey
{
    [SerializeField] private char key;
    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonPressed);
        GetComponentInChildren<TextMeshProUGUI>().text = key.ToString();
        gameObject.name = $"Key {key.ToString()}";
    }
    public void OnButtonPressed()
    {
        OnKeyPressed(key);
    }
    public void OnKeyPressed(char key)
    {
        KeyboardManager.Instance.TakeInput(key);
    }
}
