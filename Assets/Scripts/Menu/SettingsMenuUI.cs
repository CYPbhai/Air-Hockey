using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuUI : MenuUI
{

    const string MASTER_VOLUME = "masterVolume";
    const string MUSIC_VOLUME = "musicVolume";
    const string SFX_VOLUME = "sfxVolume";

    public static SettingsMenuUI instance { get; private set; }

    [SerializeField] private Slider masterSlider, musicSlider, sfxSlider;
    [SerializeField] private Button graphicsButton, backButton;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one SettingMenuUI");
            Destroy(gameObject);
            return;
        }
        instance = this;
        Debug.Log("SettingMenuUI Created.");

        masterSlider.onValueChanged.AddListener((value) =>
        {
            AudioManager.instance.MasterVolume(masterSlider.value);
            PlayerPrefs.SetFloat(MASTER_VOLUME, masterSlider.value);
            PlayerPrefs.Save();
        });

        musicSlider.onValueChanged.AddListener((value) =>
        {
            AudioManager.instance.MusicVolume(musicSlider.value);
            PlayerPrefs.SetFloat(MUSIC_VOLUME, musicSlider.value);
            PlayerPrefs.Save();
        });

        sfxSlider.onValueChanged.AddListener(value =>
        {
            AudioManager.instance.SFXVolume(sfxSlider.value);
            PlayerPrefs.SetFloat(SFX_VOLUME, sfxSlider.value);
            PlayerPrefs.Save();
        });

        graphicsButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            GraphicsSettingsMenuUI.instance.Show(this);
            Hide(false);
        });

        backButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            Hide(true);
        });
    }

    private void Start()
    {
        LoadVolumeSettings();
        Hide(false);
    }

    private void LoadVolumeSettings()
    {
        // Load saved volume settings, default to 1 (full volume) if not found
        masterSlider.value = PlayerPrefs.GetFloat(MASTER_VOLUME, 1f);
        musicSlider.value = PlayerPrefs.GetFloat(MUSIC_VOLUME, 0.8f);
        sfxSlider.value = PlayerPrefs.GetFloat(SFX_VOLUME, 1f);

        // Apply loaded settings to AudioManager
        AudioManager.instance.MasterVolume(masterSlider.value);
        AudioManager.instance.MusicVolume(musicSlider.value);
        AudioManager.instance.SFXVolume(sfxSlider.value);
    }

    private void OnEnable()
    {
        LoadVolumeSettings();
    }
}
