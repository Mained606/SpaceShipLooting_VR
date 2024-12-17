using UnityEngine;
using UnityEngine.Events;

public class LightOff : MonoBehaviour, ISignal
{
    private Light[] lights; // 모든 조명을 담는 배열
    private int trueCount = 0; // 신호를 받은 횟수

    private readonly string[] targetLightNames = { "Point light", "Point light_Up", "Point light_Down" };

    void Start()
    {
        // 신호 수신 등록
        Floor1Console.consoleCheck.AddListener(Receiver);

        // 씬의 모든 Light 컴포넌트를 찾고 필터링
        Light[] allLights = FindObjectsOfType<Light>();
        lights = System.Array.FindAll(allLights, light => IsTargetLight(light.gameObject.name));
    }

    private bool IsTargetLight(string lightName)
    {
        // 특정 이름들에 포함되는지 확인
        foreach (var name in targetLightNames)
        {
            if (lightName == name) return true;
        }
        return false;
    }

    private void LightOffAll()
    {
        // 필터링된 조명만 끄기
        foreach (var light in lights)
        {
            light.enabled = false;
        }
        Debug.Log("All target lights are turned off.");
    }

    public void Clear(UnityEvent<bool> signal) => signal.RemoveAllListeners();

    public void Receiver(bool state)
    {
        if (state)
        {
            trueCount++;
            if (trueCount >= 3)
            {
                LightOffAll();
            }
        }
    }

    public void Sender(bool state) { }
}
