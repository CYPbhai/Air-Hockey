using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayMenuUI : MenuUI
{
    public static GameplayMenuUI instance {  get; private set; }

    [SerializeField] private Button pauseButton;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one GameplayMenuUI");
            Destroy(gameObject);
            return;
        }
        instance = this;
        Debug.Log("GameplayMenuUI Created.");

        pauseButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            PauseMenuUI.instance.Show(this);
            Time.timeScale = 0f;
            Hide(false);
        });
    }
    private void Start()
    {
        Show(null);
    }
}
