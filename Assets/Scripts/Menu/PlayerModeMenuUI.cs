using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerModeMenuUI : MenuUI
{
    public static PlayerModeMenuUI instance { get; private set; }

    [SerializeField] private Button singlePlayerButton;
    [SerializeField] private Button twoPlayerButton;
    [SerializeField] private Button backButton;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one PlayerModeMenuUI");
            Destroy(gameObject);
            return;
        }
        instance = this;
        Debug.Log("PlayerModeMenuUI Created.");

        singlePlayerButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            Hide(false);
            GameManager.instance.SetPlayerMode(GameManager.PlayerMode.Single);
            DifficultyMenuUI.instance.Show(this);
        });
        twoPlayerButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            Hide(false);
            GameManager.instance.SetPlayerMode(GameManager.PlayerMode.Two);
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
