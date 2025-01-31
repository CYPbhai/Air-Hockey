using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AIHockeyController : MonoBehaviour
{
    private float speed;
    private Rigidbody rb;
    private bool isAIReady = false;
    private Vector3 targetPosition;


    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }

    private Difficulty difficulty;

    private void Awake()
    {
        targetPosition = transform.position;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetDifficulty(Difficulty newDifficulty)
    {
        difficulty = newDifficulty;
        SetSpeedAndErrorMargin();
    }

    private void SetSpeedAndErrorMargin()
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                speed = 350;
                break;
            case Difficulty.Normal:
                speed = 500;
                break;
            case Difficulty.Hard:
                speed = 750;
                break;
        }
    }

    private void Update()
    {
        if (isAIReady)
        {
             targetPosition = CalculateTargetPosition();
        }
    }

    private Vector3 CalculateTargetPosition()
    {
        Vector3 puckPosition = GameManager.instance.GetPuckPosition();
        targetPosition = rb.position;
        if (puckPosition.z > 0.25f && puckPosition.z <= 5f)
        {
            targetPosition = new Vector3(puckPosition.x, 0, puckPosition.z);

            if (puckPosition.z > 3.0f)
            {
                targetPosition.z = 4.0f;
            }
        }
        else if (puckPosition.z <= 0 && puckPosition.z >= -5f)
        {
            targetPosition = new Vector3(puckPosition.x, 0, 4.0f);
        }
        else
        {
            targetPosition = new Vector3(0, 0, 4.0f);
        }

        return targetPosition;
    }

    private void FixedUpdate()
    {
        if (isAIReady)
        {
            MoveTowardsTarget(targetPosition);
        }
    }

    private void MoveTowardsTarget(Vector3 targetPosition)
    {
        Vector3 targetVelocity = (targetPosition - rb.position) * speed * Time.fixedDeltaTime;

        Vector3 velocityChange = targetVelocity - rb.linearVelocity;
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z); // Ignore Y axis

        velocityChange = Vector3.ClampMagnitude(velocityChange, speed);
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    public void SetIsAIReady(bool newIsAIReady)
    {
        isAIReady = newIsAIReady;
    }
}
