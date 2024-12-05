using System;
using System.Collections;
using System.Collections.Generic;
using PatternLibrary;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : Singleton<SettingsManager>
{
    [Header("Audio Settings")]
    [Range(0, 100)][SerializeField] private float masterVolume = 0;
    [Range(0, 100)][SerializeField] private float musicVolume = 0;
    [Range(0, 100)][SerializeField] private float sfxVolume = 0;
    [SerializeField] private AudioMixerGroup masterMixerGroup;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

    [Header("Settings UI")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Toggle toggleKeyboard;

    [Header("PlayerPrefs")]
    private string masterVolumeKey = "MasterVolume";
    private string musicVolumeKey = "MusicVolume";
    private string sfxVolumeKey = "SFXVolume";
    private string keyboardToggleKey = "KeyboardToggle";

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        CheckPlayerPrefs();
        LoadSettings();
        RefreshUI();
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        LoadSettings();
        RefreshUI();
    }
    public void CloseSettings()
    {
        SaveAllSettings();
        settingsPanel.SetActive(false);
    }

    public void RefreshUI()
    {
        sfxVolumeSlider.value = sfxVolume;
        musicVolumeSlider.value = musicVolume;
        masterVolumeSlider.value = masterVolume;
        toggleKeyboard.isOn = PlayerPrefs.GetInt(keyboardToggleKey) == 1 ? true : false;
    }

    public float ConvertValueToDecibels(float value)
    {
        if (value <= 0)
        {
            return -80f; // Lowest dB value for silence
        }
        return Mathf.Log10(value / 100) * 20f; // Convert normalized value to decibels
    }

    public void SetSFXVolume()
    {
        sfxVolume = sfxVolumeSlider.value;
        sfxMixerGroup.audioMixer.SetFloat("SFXVolume", ConvertValueToDecibels(sfxVolume));
        RefreshUI();
    }

    public void SetMusicVolume()
    {
        musicVolume = musicVolumeSlider.value;
        musicMixerGroup.audioMixer.SetFloat("MusicVolume", ConvertValueToDecibels(musicVolume));
        RefreshUI();
    }

    public void SetMasterVolume()
    {
        masterVolume = masterVolumeSlider.value;
        masterMixerGroup.audioMixer.SetFloat("MasterVolume", ConvertValueToDecibels(masterVolume));
        RefreshUI();
    }

    public void ToggleKeyboard()
    {
        GameManager.Instance.useOnScreenKeyboard = toggleKeyboard.isOn;

        switch (GameManager.Instance.useOnScreenKeyboard)
        {
            case true:
                KeyboardManager.Instance.keyboard.SetActive(true);
                break;
            case false:
                KeyboardManager.Instance.keyboard.SetActive(false);
                break;
        }
    }

    public void SetDefaultSettings(string value)
    {
        if (PlayerPrefs.HasKey(value).GetType() == typeof(float))
        {
            PlayerPrefs.SetFloat(value, 25);
        }
        else if (PlayerPrefs.HasKey(value).GetType() == typeof(int))
        {
            PlayerPrefs.SetInt(value, 1);
        }
        else // key is a string
        {

        }
        PlayerPrefs.Save();
    }

    public void SaveSettings(string value)
    {
        // save to player prefs
        switch (value)
        {
            case "MasterVolume":
                PlayerPrefs.SetFloat(masterVolumeKey, masterVolume);
                break;
            case "MusicVolume":
                PlayerPrefs.SetFloat(musicVolumeKey, musicVolume);
                break;
            case "SFXVolume":
                PlayerPrefs.SetFloat(sfxVolumeKey, sfxVolume);
                break;
            case "KeyboardToggle":
                PlayerPrefs.SetInt(keyboardToggleKey, toggleKeyboard.isOn ? 1 : 0);
                break;
        }
        PlayerPrefs.Save();
    }
    
    public void SaveAllSettings()
    {
        SaveSettings(masterVolumeKey);
        SaveSettings(musicVolumeKey);
        SaveSettings(sfxVolumeKey);
        SaveSettings(keyboardToggleKey);
    }

    public void LoadSettings()
    {
        // load from player prefs
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 25);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 25);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 25);
        toggleKeyboard.isOn = PlayerPrefs.GetInt("KeyboardToggle", 1) == 1 ? true : false;
        RefreshUI();
    }

    public void CheckPlayerPrefs()
    {
        if (!PlayerPrefs.HasKey(masterVolumeKey))
        {
            SetDefaultSettings(masterVolumeKey);
        }
        else if (!PlayerPrefs.HasKey(musicVolumeKey))
        {
            SetDefaultSettings(musicVolumeKey);
        }
        else if (!PlayerPrefs.HasKey(sfxVolumeKey))
        {
            SetDefaultSettings(sfxVolumeKey);
        }
        else if (!PlayerPrefs.HasKey(keyboardToggleKey))
        {
            SetDefaultSettings(keyboardToggleKey);
        }
    }
    public void ExitButton()
    {
        MusicManager.Instance.Stop();
        SaveAllSettings();
        Time.timeScale = 1;
        MySceneManager.Instance.LoadScene("Main Menu");
    }
}
