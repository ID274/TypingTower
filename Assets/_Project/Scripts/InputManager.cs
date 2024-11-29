using PatternLibrary;
using TMPro;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public TMP_InputField inputField;

    private void Start()
    {
        inputField.enabled = true;
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

    public void DisableInput()
    {
        KeyboardManager.Instance.DisableKeyboard();
        inputField.DeactivateInputField();
        inputField.gameObject.SetActive(false);
    }

    public void EnableInput()
    {
        KeyboardManager.Instance.EnableKeyboard();
        inputField.ActivateInputField();
        inputField.gameObject.SetActive(true);
    }
}

