using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MenuUI
{
    public static MainMenuUI instance { get; private set; }

    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsMenuButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one MainMenuUI");
            Destroy(gameObject);
            return;
        }
        instance = this;
        Debug.Log("MainMenuUI Created.");

        playButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            Hide(false);
            PlayerModeMenuUI.instance.Show(this);
        });

        optionsMenuButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            Hide(false);
            SettingsMenuUI.instance.Show(this);
        });

        quitButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            Hide(false);
            YesNoMenuUI.instance.Show(this);
        });
    }

    private void Start()
    {
        Show(null);
    }
}
