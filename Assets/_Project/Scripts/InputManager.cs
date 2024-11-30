using PatternLibrary;
using TMPro;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public TMP_InputField inputField;

    private string previousText;

    public void StartInputUI()
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

        previousText = inputField.text;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            KeyboardManager.Instance.OnBackspacePressed();
        }
    }

    public void OnValueChanged()
    {
        SFXManager.Instance.PlaySFX(0, true); // play keyboard press sound with random pitch
        inputField.text = inputField.text.ToUpper();
        if (previousText.Length < inputField.text.Length)
        {
            KeyboardManager.Instance.mistakePending = false;
        }
        Debug.Log($"OnValueChanged: {inputField.text}");
        bool wordFound = WordSearchManager.Instance.CheckWord(inputField.text);
        if (wordFound)
        {
            Word wordObjectFound = WordSearchManager.Instance.WordFound(inputField.text);
            GameManager.Instance.WordHasBeenFound(wordObjectFound);
            ClearField();
        }
        previousText = inputField.text;
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

