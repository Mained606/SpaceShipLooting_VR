using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float rotationTime = 2f;
    private bool rotatingLeft = true;
    private Quaternion startRotation;
    private Quaternion targetRotation;

    private string rotationTimer = "rotationTimer";

    void Start()
    {
        startRotation = transform.rotation;
        TimerManager.AddTimer(rotationTimer, rotationTime);
    }

    private void Update()
    {
        LookAround(-45, 45);
    }

    void LookAround(float leftAngle, float rightAngle)
    {
        if (!TimerManager.IsContainsKey(rotationTimer))
        {
            TimerManager.AddTimer(rotationTimer, rotationTime);
        }

        float currentTime = TimerManager.currentTime(rotationTimer);

        if (currentTime > 0)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, (rotationTime - currentTime) / rotationTime);
        }
        else
        {
            SetTargetRotation(leftAngle, rightAngle);
        }
    }

    void SetTargetRotation(float leftAngle, float rightAngle)
    {
        rotatingLeft = !rotatingLeft;
        startRotation = transform.rotation;
        targetRotation = rotatingLeft ? Quaternion.Euler(0, leftAngle, 0) : Quaternion.Euler(0, rightAngle, 0);

        TimerManager.AddTimer(rotationTimer, rotationTime);
    }
}
