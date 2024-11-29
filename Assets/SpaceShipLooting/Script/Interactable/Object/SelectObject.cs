using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SelectObject : XRBaseInteractable, ISignal
{
    public UnityEvent<string> OnSignal { get; } = new UnityEvent<string>();

    [SerializeField]private bool SignalToss = true; // Inspector에서 신호 발사 여부 설정

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (SignalToss) // emitSignal이 true일 때만 신호 발사
        {
            OnSignal.Invoke($"Selected: {gameObject.name}");
        }

    }

    /*private void OnDestroy()
    {
        OnSignal.RemoveAllListeners(); // 파괴가능 오브젝트일 시 활성화
    }*/

    public void ReceiveSignal()
    {
        // 필요에 따라 구현
    }
}