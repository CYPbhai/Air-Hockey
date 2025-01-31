using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeclareWinnerMenuUI : MenuUI
{
    public static DeclareWinnerMenuUI instance {  get; private set; }

    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;

    [SerializeField] private TextMeshProUGUI winnerTextPlayer1;
    [SerializeField] private TextMeshProUGUI winnerTextPlayer2;
    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one DeclareWinnerMenuUI");
            Destroy(gameObject);
            return;
        }
        instance = this;
        Debug.Log("DeclareWinnerMenuUI Created.");

        playAgainButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            Hide(true);
            GameManager.instance.SetState(GameManager.GameState.StartCountdown);
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

    public void DeclareWinner(bool player1Won)
    {
        if (player1Won)
        {
            winnerTextPlayer1.text = "WON";
            winnerTextPlayer1.color = Color.green;
            winnerTextPlayer2.text = "LOSE";
            winnerTextPlayer2.color = Color.red;
        }
        else
        {

            winnerTextPlayer1.text = "LOSE";
            winnerTextPlayer1.color = Color.red;
            winnerTextPlayer2.text = "WON";
            winnerTextPlayer2.color = Color.green;
        }
    }
}
