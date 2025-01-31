using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class StartCountdownUI : MenuUI
{
    public static StartCountdownUI instance { get; private set; }

    [SerializeField] private TextMeshProUGUI countdownTextP1;
    [SerializeField] private TextMeshProUGUI countdownTextP2;
    private float timer;
    private float previousTimerValue;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one StartCountdownUI");
            Destroy(gameObject);
            return;
        }
        instance = this;
        Debug.Log("StartCountdownUI Created.");
    }

    private void Start()
    {
        Hide(false);
        StartCountdown();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (math.ceil(timer - 1) != math.ceil(previousTimerValue - 1) && timer > 1)
        {
            AudioManager.instance.PlaySFX("Countdown");
        }
        countdownTextP1.text = math.ceil(timer-1).ToString();
        countdownTextP2.text = math.ceil(timer-1).ToString();

        if(timer <= 1 && timer > 0)
        {
            if (math.ceil(previousTimerValue - 1) > 0)
            {
                AudioManager.instance.PlaySFX("Go");
            }
            countdownTextP1.text = "GO";
            countdownTextP2.text = "GO";
        }
        if(timer <= 0)
        {
            Hide(false);
            GameManager.instance.SetState(GameManager.GameState.StartGame);
        }

        previousTimerValue = timer;
    }

    public void StartCountdown()
    {
        timer = 4f;
        Show(null);
    }
}
