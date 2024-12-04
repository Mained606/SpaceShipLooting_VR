using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SelectObject : XRSimpleInteractable, ISignal
{
    public UnityEvent<string> OnSignal { get; } = new UnityEvent<string>();

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void ClearListeners()
    {
        OnSignal.RemoveAllListeners();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // 신호 발행
        Debug.Log($"Object selected: {gameObject.name}");
        OnSignal.Invoke($"Selected: {gameObject.name}");
    }
}
