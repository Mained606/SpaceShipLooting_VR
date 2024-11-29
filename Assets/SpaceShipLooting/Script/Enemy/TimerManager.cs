using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance{ get; private set; }
    private Dictionary<string, float> timers = new Dictionary<string, float>();

    private string rotationTimer = "rotationTimer";

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        UpdateTimer();
    }

    public static void AddTimer(string timerName, float time)
    {

        if (!Instance.timers.ContainsKey(timerName))
        {
            Instance.timers.Add(timerName, time);
            Debug.Log("timer 생성");
        }
    }

    void UpdateTimer()
    {
        if (timers.Count > 0)
        {
            var keys = new List<string>(timers.Keys);
            foreach (string key in keys)
            {
                if (timers[key] > 0)
                {
                    timers[key] -= Time.deltaTime;
                }
                else
                {
                    timers.Remove(key);
                    Debug.Log("Remove Timer");
                }
            }
        }

    }

    public static float currentTime(string timerName)
    {
        return Instance.timers[timerName];
    }

    public static bool IsContainsKey(string timerName)
    {
        return Instance.timers.ContainsKey(timerName);
    }
}
