using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scores : MonoBehaviour
{
    public static Scores instance {  get; private set; }

    [SerializeField] private TMP_Text player1ScoreText;
    [SerializeField] private TMP_Text player2ScoreText;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one Scores");
            Destroy(gameObject);
            return;
        }
        instance = this;
        Debug.Log("Scores Created.");
    }

    private void Start()
    {
        Hide();
    }

    public void UpdateScoreUI(int player1Score, int player2Score)
    {
        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
