using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {  get; private set; }

    //public event EventHandler OnStateChanged;

    public HockeyMaterialListSO hockeyMaterialListSO;
    public PuckMaterialListSO puckMaterialListSO;
    public GlowingHockeyMaterialListSO glowingHockeyMaterialListSO;
    public GlowingPuckMaterialListSO glowingPuckMaterialListSO;

    public Hockey hockeyPrefab;
    public Puck puckPrefab;

    public int player1MaterialID = 0;
    public int player2MaterialID = 0;
    public int puckMaterialID = 0;

    [SerializeField] private Vector3 hockey1DefualtPosition;
    [SerializeField] private Vector3 hockey2DefualtPosition;
    [SerializeField] private Vector3 puckDefualtPosition;

    private Hockey hockey1;
    private Hockey hockey2;
    private Puck puck;
    public enum GameState
    {
        MainMenu,
        StartCountdown,
        StartGame,
        Playing,
        Pause,
        DeclareWinner
    }

    public enum PlayerMode
    {
        Single,
        Two
    }

    private GameState currentState;
    private PlayerMode playerMode;
    private AIHockeyController.Difficulty difficulty;
    private Coroutine resetPositionsCoroutine;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one GameManager");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("GameManager Created.");
    }

    private void Start()
    {
        SetState(GameState.MainMenu);
    }

    public void SetState(GameState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case GameState.MainMenu:
                break;
            case GameState.StartCountdown:
                if(StartCountdownUI.instance != null)
                    StartCountdownUI.instance.StartCountdown();
                break;
            case GameState.StartGame:
                StartGame();
                break;
            case GameState.Playing:
                Scores.instance.Show();
                Scores.instance.UpdateScoreUI(hockey1.score, hockey2.score);
                break;
            case GameState.Pause:
                break;
            case GameState.DeclareWinner:
                StartCoroutine(DeclareWinner());
                break;
            default:
                Debug.LogError("State not set!");
                break;
        }
    }

    public void SetDifficulty(AIHockeyController.Difficulty newDifficulty)
    {
        difficulty = newDifficulty;
    }

    public void SetPlayerMode(PlayerMode newPlayerMode)
    {
        playerMode = newPlayerMode;
    }

    public PlayerMode GetPlayerMode()
    {
        return playerMode;
    }

    private void StartGame()
    {
        if(hockey1 != null)
        {
            hockey1.gameObject.SetActive(true);
            hockey1.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            hockey1.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            hockey1.transform.position = hockey1DefualtPosition;
            hockey1.GetComponent<HockeyController>().SetTargetPosition(hockey1.transform.position);
        }
        else
        {
            hockey1 = Instantiate(hockeyPrefab, hockey1DefualtPosition, Quaternion.identity);
            hockey1.GetComponentInChildren<Renderer>().material = hockeyMaterialListSO.materials[player1MaterialID];
            hockey1.SetGlowMaterial(glowingHockeyMaterialListSO.materials[player1MaterialID]);
            hockey1.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            hockey1.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }
        if (puck != null)
        {
            puck.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            puck.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            puck.transform.position = puckDefualtPosition;
            puck.gameObject.SetActive(true);
        }
        else
        {
            puck = Instantiate(puckPrefab, puckDefualtPosition, Quaternion.identity);
            puck.GetComponentInChildren<Renderer>().material = puckMaterialListSO.materials[puckMaterialID];
            puck.GetComponentInChildren<TrailRenderer>().material = puckMaterialListSO.materials[puckMaterialID];
            puck.GetComponentInChildren<TrailRenderer>().startColor = puckMaterialListSO.materials[puckMaterialID].color;
        }
        if(hockey2 != null)
        {
            hockey2.gameObject.SetActive(true);
            hockey2.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            hockey2.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            hockey2.transform.position = hockey2DefualtPosition;
            hockey2.GetComponent<HockeyController>().SetTargetPosition(hockey2.transform.position);
        }
        else
        {
            hockey2 = Instantiate(hockeyPrefab, hockey2DefualtPosition, Quaternion.identity);
            hockey2.GetComponentInChildren<Renderer>().material = hockeyMaterialListSO.materials[player2MaterialID];
            hockey2.SetGlowMaterial(glowingHockeyMaterialListSO.materials[player2MaterialID]);
            hockey2.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            hockey2.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        if (playerMode == PlayerMode.Single)
        {
            hockey2.GetComponent<HockeyController>().enabled = false;
            hockey2.gameObject.AddComponent<AIHockeyController>();
            hockey2.GetComponent<AIHockeyController>().SetDifficulty(difficulty);
            hockey2.GetComponent<AIHockeyController>().SetIsAIReady(true);
            hockey2.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            hockey2.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        ResetScores();
        SetState(GameState.Playing);
    }

    private void ResetScores()
    {
        hockey1.score = 0;
        hockey2.score = 0;
    }

    public void OnGoalScored(bool isPlayer1Goal)
    {
        AudioManager.instance.PlaySFX("Goal");
        if (isPlayer1Goal)
        {
            hockey1.score++;
        }
        else
        {
            hockey2.score++;
        }
        Scores.instance.UpdateScoreUI(hockey1.score, hockey2.score);
        if(hockey1.score >= 7 ||  hockey2.score >= 7)
        {
            SetState(GameState.DeclareWinner);
        }
        resetPositionsCoroutine = StartCoroutine(ResetPositions(isPlayer1Goal));
    }

    private IEnumerator ResetPositions(bool isPlayer1Goal)
    {
        if (currentState == GameState.Playing)
        {
            puck.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            puck.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            puck.GetComponentInChildren<TrailRenderer>().enabled = false;
            yield return new WaitForSeconds(1);
            AudioManager.instance.PlaySFX("ResetHockey");
            hockey1.transform.position = hockey1DefualtPosition;
            hockey1.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            hockey1.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            hockey1.GetComponent<HockeyController>().SetTargetPosition(hockey1.transform.position);
            hockey2.transform.position = hockey2DefualtPosition;
            hockey2.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            hockey2.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            hockey2.GetComponent<HockeyController>().SetTargetPosition(hockey2.transform.position);
            yield return new WaitForSeconds(1);
            AudioManager.instance.PlaySFX("ResetPuck");
            if (puck != null)
            {
                if (isPlayer1Goal)
                {
                    puck.transform.position = new Vector3(0, 0, 2);
                }
                else
                {
                    puck.transform.position = new Vector3(0, 0, -2);
                }
                puck.GetComponentInChildren<TrailRenderer>().enabled = true;
            }
        }
    }

    private IEnumerator DeclareWinner()
    {
        if (resetPositionsCoroutine != null)
        {
            StopCoroutine(resetPositionsCoroutine);
            resetPositionsCoroutine = null;
        }
        DeclareWinnerMenuUI.instance.DeclareWinner(hockey1.score >= 7);
        yield return new WaitForSeconds(1);
        AudioManager.instance.PlaySFX("DeclareWinner");
        Scores.instance.Hide();
        GameplayMenuUI.instance.Hide(false);
        DeclareWinnerMenuUI.instance.Show(GameplayMenuUI.instance);
        hockey1.gameObject.SetActive(false);
        hockey2.gameObject.SetActive(false);
        puck.gameObject.SetActive(false);
    }

    public Vector3 GetPuckPosition()
    {
        return puck.transform.position;
    }
}
