using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuUI : MenuUI
{
    public static PauseMenuUI instance { get; private set; }

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;
    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one PauseMenuUI");
            Destroy(gameObject);
            return;
        }
        instance = this;
        Debug.Log("PauseMenuUI Created.");

        resumeButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            Time.timeScale = 1.0f;
            Hide(true);
        });
        optionsButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            Hide(false);
            SettingsMenuUI.instance.Show(this);
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            Hide(false);
            YesNoMenuUI.instance.Show(this);
        });
    }

    private void Start()
    {
        Hide(false);
    }
}
