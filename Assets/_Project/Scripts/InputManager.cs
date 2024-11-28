using PatternLibrary;
using TMPro;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public TMP_InputField inputField;

    private void Start()
    {
        if (Application.isMobilePlatform)
        {
            // Add listeners for focus and deselection handling
            inputField.onSelect.AddListener(OpenNativeKeyboard);
            inputField.onDeselect.AddListener(HideNativeKeyboard);
        }
        else if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Debug.Log("Running on WebGL. Keyboard is managed by the browser.");
        }
        else
        {
            Debug.Log("Running on a non-mobile, non-WebGL platform. No special keyboard behavior.");
        }
    }

    private void OpenNativeKeyboard(string text)
    {
        TouchScreenKeyboard.Open(inputField.text, TouchScreenKeyboardType.Default);
    }

    private void HideNativeKeyboard(string text)
    {
        inputField.DeactivateInputField();
        Debug.Log("Keyboard hidden as field is deselected.");
    }

    public void OnValueChanged()
    {
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
        if (Application.isMobilePlatform)
        {
            // Ensure the keyboard is open when the field is selected
            TouchScreenKeyboard.Open(inputField.text, TouchScreenKeyboardType.Default);
        }
    }

    public void OnFieldDeselected()
    {
        Debug.Log("Field deselected.");
        if (Application.isMobilePlatform)
        {
            // Hide the keyboard when the field is deselected
            inputField.DeactivateInputField(); // Ensure the field loses focus
        }
    }

    public void ClearField()
    {
        inputField.text = "";
    }
}

