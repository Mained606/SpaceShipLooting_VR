using UnityEngine.Events;

/// <summary>
/// UnityEvent를 위한 확장 메서드를 제공하는 유틸리티 클래스.
/// </summary>
public static class UnityEventExtensions
{
    /// <summary>
    /// UnityEvent가 null인 경우 초기화합니다.
    /// </summary>
    /// <param name="unityEvent">초기화할 UnityEvent</param>
    /// <returns>초기화된 UnityEvent</returns>
    public static UnityEvent Initialize(this UnityEvent unityEvent)
    {
        return unityEvent ?? new UnityEvent();
    }

    /// <summary>
    /// 제네릭 UnityEvent가 null인 경우 초기화합니다.
    /// </summary>
    public static UnityEvent<T> Initialize<T>(this UnityEvent<T> unityEvent)
    {
        return unityEvent ?? new UnityEvent<T>();
    }
}