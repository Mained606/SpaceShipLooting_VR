using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ValveMove : XRSimpleInteractableOutline, ISignal
{
    private Animator anim;
    private bool hasExecuted = false; // 중복 방지 플래그

    public static UnityEvent<bool> OnValve { get; } = new UnityEvent<bool>();

    protected override void Start()
    {
        base.Start();
        // Animator를 현재 오브젝트 또는 부모에서 검색
        anim = GetComponent<Animator>() ?? GetComponentInParent<Animator>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (hasExecuted) return; // 이미 실행되었다면 종료

        if (anim != null)
        {
            anim.SetTrigger("Open");
            AudioManager.Instance.Play("Valve", false);
        }

        Sender(true);
        hasExecuted = true; // 실행 플래그 설정
    }

    public void Sender(bool state) => OnValve?.Invoke(state);

    public void Receiver(bool state) { }

    public void Clear(UnityEvent<bool> signal)
    {
        OnValve.RemoveAllListeners();
        hasExecuted = false; // 초기화 시 실행 플래그도 재설정
    }
}
