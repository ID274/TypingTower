using PatternLibrary;
using TMPro;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public TMP_InputField inputField;

    private void Start()
    {
        if (GameManager.Instance.useOnScreenKeyboard)
        {
            inputField.DeactivateInputField();
            KeyboardManager.Instance.InitializeKeyboard();
        }
        else
        {
            inputField.ActivateInputField();
        }
    }

    public void OnValueChanged()
    {
        inputField.text = inputField.text.ToUpper();
        Debug.Log($"OnValueChanged: {inputField.text}");
        bool wordFound = WordSearchManager.Instance.CheckWord(inputField.text);
        if (wordFound)
        {
            Word wordObjectFound = WordSearchManager.Instance.WordFound(inputField.text);
            GameManager.Instance.WordHasBeenFound(wordObjectFound);
            ClearField();
        }
    }

    public void OnFieldSelected()
    {
        Debug.Log("Field selected.");
    }

    public void OnFieldDeselected()
    {
        Debug.Log("Field deselected.");
        inputField.DeactivateInputField();
    }

    public void ClearField()
    {
        inputField.text = "";
        KeyboardManager.Instance.ClearInput();
    }

    public void DisableKeyboard()
    {
        KeyboardManager.Instance.DisableKeyboard();
    }
}

