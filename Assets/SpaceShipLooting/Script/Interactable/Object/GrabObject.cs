using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabObject : XRGrabInteractable, ISignal
{
    public UnityEvent<string> OnSignal { get; } = new UnityEvent<string>();

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // 조건 확인 후 시그널 토스 
        if (ShouldSendSignal())
        {
            OnSignal.Invoke($"Grabbed: {gameObject.name}");
        }
    }

    private bool ShouldSendSignal()
    {
        // 조건을 작성하지 않은 상태에서는 false 반환 -> 필요에 따라 조건 작성 필요
        return false;
    }

    public void ReceiveSignal()
    {
        // 필요에 따라 구현
    }
}
