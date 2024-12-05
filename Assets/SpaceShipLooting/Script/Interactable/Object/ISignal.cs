using UnityEngine;

public interface ISignal
{
    UnityEngine.Events.UnityEvent<string> OnSignal { get; }
    GameObject GetGameObject();
    void ClearListeners();
}
