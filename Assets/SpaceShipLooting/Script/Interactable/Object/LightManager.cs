using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour, ISignalReceiver
{
    private Dictionary<string, List<Light>> lightGroups = new Dictionary<string, List<Light>>();

    private void Start()
    {
        FindAndGroupLights();
    }

    // 이름을 기준으로 라이트를 찾고 그룹화
    private void FindAndGroupLights()
    {
        Light[] allLights = FindObjectsOfType<Light>();

        foreach (var light in allLights)
        {
            string lightName = light.name;

            // 특정 이름의 라이트만 그룹화
            if (lightName == "Point light_Up" || lightName == "Point light_Down" || lightName == "Point light")
            {
                if (!lightGroups.ContainsKey(lightName))
                {
                    lightGroups[lightName] = new List<Light>();
                }

                lightGroups[lightName].Add(light);
                light.enabled = false; // 초기화 시 모든 라이트 끄기
            }
        }

        Debug.Log($"Lights grouped by name: {lightGroups.Count} groups found");
    }

    // 신호를 받아 특정 이름의 라이트를 켜거나 끔
    public void ReceiveSignal(string signal)
    {
        Debug.Log($"LightManager received signal: {signal}");

        // 이름을 신호로 전달받아 해당 그룹의 라이트를 토글
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
}
