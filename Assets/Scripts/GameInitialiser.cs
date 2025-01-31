using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitialiser : MonoBehaviour
{
    [SerializeField] private GameManager gameManagerPrefab;
    [SerializeField] private AudioManager audioManagerPrefab;

    private void Awake()
    {
        if (GameManager.instance == null)
            Instantiate(gameManagerPrefab);
        if (AudioManager.instance == null)
            Instantiate(audioManagerPrefab);
    }
}
