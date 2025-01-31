using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class YesNoMenuUI : MenuUI
{
    public static YesNoMenuUI instance { get; private set; }

    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one YesNoMenuUI");
            Destroy(gameObject);
            return;
        }
        instance = this;
        Debug.Log("YesNoMenuUI Created.");

        yesButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            if (previousMenuUI == PauseMenuUI.instance || previousMenuUI == DeclareWinnerMenuUI.instance)
            {
                SceneManager.LoadScene("MainMenuScene");
                Time.timeScale = 1.0f;
            }
            else
            {

                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
        });
        noButton.onClick.AddListener(() =>
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
