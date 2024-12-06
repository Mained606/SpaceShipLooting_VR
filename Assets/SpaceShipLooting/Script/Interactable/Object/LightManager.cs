using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour, ISignalReceiver
{
    [SerializeField]
    private bool startLightsOn = false; // 시작 시 라이트 상태 (Inspector에서 설정 가능)

    private Dictionary<string, List<Light>> lightGroups = new Dictionary<string, List<Light>>();

    private void Start()
    {
        FindAndGroupLights();
    }

    // 특정 이름의 라이트만 그룹화
    private void FindAndGroupLights()
    {
        Light[] allLights = FindObjectsOfType<Light>();

        foreach (var light in allLights)
        {
            string lightName = light.name;

            // 특정 이름만 그룹화
            if (lightName == "Point light_Up" || lightName == "Point light_Down" || lightName == "Point light")
            {
                if (!lightGroups.ContainsKey(lightName))
                {
                    lightGroups[lightName] = new List<Light>();
                }

                lightGroups[lightName].Add(light);

                // 초기 상태를 설정 (켜짐/꺼짐)
                light.enabled = startLightsOn;
            }
        }

        Debug.Log($"Lights grouped by name: {lightGroups.Count} groups found. Initial state: {(startLightsOn ? "ON" : "OFF")}");
    }

    // 신호를 받아 특정 이름의 라이트를 제어
    public void ReceiveSignal(string signal)
    {
        Debug.Log($"LightManager received signal: {signal}");

        // 정확한 이름으로 매칭된 그룹의 라이트를 토글
        if (lightGroups.ContainsKey(signal))
        {
            ToggleLights(lightGroups[signal]);
        }
        else
        {
            Debug.LogWarning($"No lights found for name: {signal}");
        }
    }

    // 라이트 상태를 토글
    private void ToggleLights(List<Light> lights)
    {
        foreach (var light in lights)
        {
            light.enabled = !light.enabled;
        }
    }

    // Receiver의 GameObject 반환
    public GameObject GetGameObject()
    {
        return gameObject; // 현재 GameObject 반환
    }
}
