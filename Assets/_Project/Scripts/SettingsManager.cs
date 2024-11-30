using System;
using System.Collections;
using System.Collections.Generic;
using PatternLibrary;
using UnityEngine;
using UnityEngine.Audio;
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

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        sfxVolumeSlider.value = sfxVolume;
        musicVolumeSlider.value = musicVolume;
        masterVolumeSlider.value = masterVolume;
        RefreshSliders();
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        RefreshSliders();
    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void RefreshSliders()
    {
        sfxVolumeSlider.value = sfxVolume;
        musicVolumeSlider.value = musicVolume;
        masterVolumeSlider.value = masterVolume;
    }

    public float ConvertValue(float value)
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
        sfxMixerGroup.audioMixer.SetFloat("SFXVolume", ConvertValue(sfxVolume));
        RefreshSliders();
    }

    public void SetMusicVolume()
    {
        musicVolume = musicVolumeSlider.value;
        musicMixerGroup.audioMixer.SetFloat("MusicVolume", ConvertValue(musicVolume));
        RefreshSliders();
    }

    public void SetMasterVolume()
    {
        masterVolume = masterVolumeSlider.value;
        masterMixerGroup.audioMixer.SetFloat("MasterVolume", ConvertValue(masterVolume));
        RefreshSliders();
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

    public void SaveSettings()
    {
        // save to player prefs
    }

    public void ExitButton()
    {
        // exit game
    }
}
