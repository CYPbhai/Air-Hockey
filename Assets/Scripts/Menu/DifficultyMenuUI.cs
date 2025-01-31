using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyMenuUI : MenuUI
{
    public static DifficultyMenuUI instance { get; private set; }

    [SerializeField] private Button easyButton;
    [SerializeField] private Button normalButton;
    [SerializeField] private Button hardButton;
    [SerializeField] private Button backButton;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one DifficultyMenuUI");
            Destroy(gameObject);
            return;
        }
        instance = this;
        Debug.Log("DifficultyMenuUI Created.");

        easyButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            GameManager.instance.SetDifficulty(AIHockeyController.Difficulty.Easy);
            Hide(false);
            AssetsSelectMenuUI.instance.Show(this);
        });
        normalButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            GameManager.instance.SetDifficulty(AIHockeyController.Difficulty.Normal);
            Hide(false);
            AssetsSelectMenuUI.instance.Show(this);
        });
        hardButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            GameManager.instance.SetDifficulty(AIHockeyController.Difficulty.Hard);
            Hide(false);
            AssetsSelectMenuUI.instance.Show(this);
        });
        backButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            Hide(true);
        });
    }

    private void Start()
    {
        Hide(false);
    }
}
