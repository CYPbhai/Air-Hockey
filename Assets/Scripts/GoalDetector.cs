using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDetector : MonoBehaviour
{
    [SerializeField] private bool isPlayer1Goal; // True if this is Player 1's goal
    private void OnTriggerEnter(Collider other)
    {
         GameManager.instance.OnGoalScored(isPlayer1Goal);
        GetComponent<AudioSource>().Play();
    }
}
