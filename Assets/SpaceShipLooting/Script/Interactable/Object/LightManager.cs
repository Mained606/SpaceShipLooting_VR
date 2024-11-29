using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LightManager : MonoBehaviour, ISignal
{
    private Light[] managedLights;

    public UnityEvent<string> OnSignal { get; } = new UnityEvent<string>();

    private void Start()
    {
        // 대상 이름에 해당하는 라이트만 필터링
        managedLights = FindObjectsOfType<Light>().Where(light =>
            light.name == "Point light_Up" ||
            light.name == "Point light_Down" ||
            light.name == "Point light").ToArray();

        foreach (var light in managedLights)
        {
            light.enabled = false; // 초기화 시 모든 라이트 끄기
        }
    }

    public void ReceiveSignal()
    {
        foreach (var light in managedLights)
        {
            light.enabled = !light.enabled; // 상태 반전
        }
        OnSignal.Invoke("Lights toggled");
    }
}
