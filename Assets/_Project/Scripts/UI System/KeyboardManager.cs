using PatternLibrary;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardManager : Singleton<KeyboardManager>
{
    [Header("Attributes")]
    public int maxInputLength = 18;
    public bool keyboardActive = false;

    [Header("References")]
    public GameObject keyboard;

    public string currentInput = "";
    public Dictionary<string, bool> permittedKeys = new Dictionary<string, bool>();
    public List<string> permittedKeysList = new List<string>();

    protected override void Awake()
    {
        base.Awake();
        keyboard.SetActive(false);
    }

    public void InitializeKeyboard()
    {
        if (keyboard != null)
        {
            foreach (var key in permittedKeysList)
            {
                permittedKeys.Add(key, true);
            }
        }
        keyboard.SetActive(true);
        keyboardActive = true;
}
    public void OnBackspacePressed()
    {
        if (keyboard != null && keyboardActive == true)
        {
            if (currentInput.Length > 0)
            {
                currentInput = currentInput.Remove(currentInput.Length - 1);
                InputManager.Instance.inputField.text = currentInput;
                InputManager.Instance.OnValueChanged();
            }
        }
        ScoreManager.Instance.AddMistake();
    }

    public void TakeInput(char key)
    {
        if (keyboard != null && keyboardActive == true)
        {
            if (currentInput.Length < maxInputLength)
            {
                currentInput += key;
                InputManager.Instance.inputField.text = currentInput.ToUpper();
                InputManager.Instance.OnValueChanged();
            }
        }
    }

    public void DisableKeyboard()
    {
        keyboardActive = false;
        keyboard.SetActive(false);
    }

    public void EnableKeyboard()
    {
        keyboardActive = true;
        keyboard.SetActive(true);
    }

    public void ClearInput()
    {
        currentInput = "";
    }
}
