using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AssetsSelectMenuUI : MenuUI
{
    public static AssetsSelectMenuUI instance { get; private set; }

    [SerializeField] private Button backButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button p1LeftButton;
    [SerializeField] private Button p1RightButton;
    [SerializeField] private Button puckLeftButton;
    [SerializeField] private Button puckRightButton;
    [SerializeField] private GameObject p2Buttons;
    [SerializeField] private Button p2LeftButton;
    [SerializeField] private Button p2RightButton;
    [SerializeField] private TMP_Text player1Text;
    [SerializeField] private TMP_Text player2Text;

    [SerializeField] private GameObject hockeyVisualPrefab;
    [SerializeField] private GameObject puckVisualPrefab;

    [SerializeField] private Vector3 hockey1VisualPosition;
    [SerializeField] private Vector3 hockey2VisualPosition;
    [SerializeField] private Vector3 puckVisualPosition;

    private GameObject hockey1Visual;
    private GameObject hockey2Visual;
    private GameObject puckVisual;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one AssetsSelectMenuUI");
            Destroy(gameObject);
            return;
        }
        instance = this;
        Debug.Log("AssetsSelectMenuUI Created.");

        backButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            Hide(true);
        });
        startButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            SceneManager.LoadScene("GameplayScene");
        });
        p1LeftButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            GameManager.instance.player1MaterialID--;
            if(GameManager.instance.player1MaterialID < 0)
                GameManager.instance.player1MaterialID = GameManager.instance.hockeyMaterialListSO.materials.Length - 1;
            UpdateVisual();
        });
        p1RightButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            GameManager.instance.player1MaterialID++;
            if (GameManager.instance.player1MaterialID >= GameManager.instance.hockeyMaterialListSO.materials.Length)
                GameManager.instance.player1MaterialID = 0;
            UpdateVisual();
        });
        puckLeftButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            GameManager.instance.puckMaterialID--;
            if (GameManager.instance.puckMaterialID < 0)
                GameManager.instance.puckMaterialID = GameManager.instance.puckMaterialListSO.materials.Length - 1;
            UpdateVisual();
        });
        puckRightButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            GameManager.instance.puckMaterialID++;
            if (GameManager.instance.puckMaterialID >= GameManager.instance.puckMaterialListSO.materials.Length)
                GameManager.instance.puckMaterialID = 0;
            UpdateVisual();
        });
        p2LeftButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            GameManager.instance.player2MaterialID--;
            if (GameManager.instance.player2MaterialID < 0)
                GameManager.instance.player2MaterialID = GameManager.instance.hockeyMaterialListSO.materials.Length - 1;
            UpdateVisual();
        });
        p2RightButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            GameManager.instance.player2MaterialID++;
            if (GameManager.instance.player2MaterialID >= GameManager.instance.hockeyMaterialListSO.materials.Length)
                GameManager.instance.player2MaterialID = 0;
            UpdateVisual();
        });
    }

    private void Start()
    {
        hockey1Visual = Instantiate(hockeyVisualPrefab, hockey1VisualPosition, Quaternion.identity, transform);
        hockey2Visual = Instantiate(hockeyVisualPrefab, hockey2VisualPosition, Quaternion.identity, transform);
        puckVisual = Instantiate(puckVisualPrefab, puckVisualPosition, Quaternion.identity, transform);
        hockey1Visual.transform.localPosition = hockey1VisualPosition;
        hockey2Visual.transform.localPosition = hockey2VisualPosition;
        puckVisual.transform.localPosition = puckVisualPosition;
        Hide(false);
        UpdateVisual();
    }


    public void UpdateVisual()
    {
        hockey1Visual.GetComponentInChildren<MeshRenderer>().material = GameManager.instance.hockeyMaterialListSO.materials[GameManager.instance.player1MaterialID];
        hockey2Visual.GetComponentInChildren<MeshRenderer>().material = GameManager.instance.hockeyMaterialListSO.materials[GameManager.instance.player2MaterialID];
        puckVisual.GetComponentInChildren<MeshRenderer>().material = GameManager.instance.puckMaterialListSO.materials[GameManager.instance.puckMaterialID];
    }

    override public void Show(MenuUI previous)
    {
        if (previous)
            previousMenuUI = previous;
        gameObject.SetActive(true);

        if(GameManager.instance.GetPlayerMode() == GameManager.PlayerMode.Single)
        {
            player2Text.text = "AI";
            player2Text.transform.eulerAngles = new Vector3(90, 0, 0);
            p2Buttons.SetActive(false);
            GameManager.instance.player2MaterialID = Random.Range(0, GameManager.instance.hockeyMaterialListSO.materials.Length);
            UpdateVisual();
        }
        else
        {
            player2Text.text = "PLAYER 2";
            player2Text.transform.eulerAngles = new Vector3(90, 0, 180);
            if (!p2Buttons.activeSelf)
                p2Buttons.SetActive(true);
        }
    }
}
