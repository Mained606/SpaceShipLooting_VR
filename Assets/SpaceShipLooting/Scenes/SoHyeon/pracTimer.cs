using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class pracTimer : MonoBehaviour
{
    #region Variables
    private float rotationTime = 2f;
    private bool rotationLeft = true;
    private Quaternion startRotation;
    private Quaternion targetRotation;
    Dictionary<string, float> timer = new Dictionary<string, float>();
    #endregion 

    private void Start()
    {
        startRotation = transform.rotation;
        timer.Add("RotationTimer", rotationTime);
    }

    private void Update()
    {
        UpdateTimer();
        UpdateRotation(-45f, 45f);
    }

    void UpdateTimer()
    {
        var keys = new List<string>(timer.Keys);
        foreach(var key in keys)
        {
            if (timer[key] > 0)
            {
                timer[key] -= Time.deltaTime;
            }
        }
    }

    void UpdateRotation(float leftAngle, float rightAngle)
    {
        if (timer["RotationTimer"] > 0)
        {
            // 돌리기
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, (rotationTime - timer["RotationTimer"]) / rotationTime);
        }
        else
        {
            // 방향전환
            timer["RotationTimer"] = rotationTime;
            rotationLeft = !rotationLeft;
            if (rotationLeft)
            {
                startRotation = transform.rotation;
                targetRotation = Quaternion.Euler(0f, leftAngle, 0f);
            }
            else
            {
                startRotation = transform.rotation;
                targetRotation = Quaternion.Euler(0f, rightAngle, 0f);
            }
            
        }
    }

}
