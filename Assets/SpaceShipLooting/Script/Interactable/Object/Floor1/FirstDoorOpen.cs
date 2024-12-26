using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class FirstDoorOpen : XRSimpleInteractableOutline, ISignal
{
    private Animator anim;
    private int trueCount = 0; // 신호를 받은 횟수
    private bool isCan = false; // 상호작용 가능 여부

    protected override void Start()
    {
        base.Start();
        // Animator 가져오기
        anim = GetComponentInParent<Animator>();
        if (anim == null)
        {
            anim = GetComponentInParent<Animator>();
        }
        this.enabled = false;

        Floor1Console.consoleCheck.AddListener(Receiver);

    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (isCan)
        {
            base.OnSelectEntered(args);
            anim.SetTrigger("Open");
            AudioManager.Instance.Play("DoorOpen_");
        }
    }
    public void Sender(bool state) { }

    public void Receiver(bool state)
    {
        if (state)
        {
            trueCount++;
            if (trueCount >= 3)
            {
                AudioManager.Instance.Play("F1Correct");
                JsonTextManager.instance.OnDialogue("stage1-7");
                isCan = true; // 상호작용 가능 상태로 변경
                this.enabled = true; // XRSimpleInteractable 활성화
                Debug.Log("2층문 활성화");
            }
        }
    }
    public void Clear(UnityEvent<bool> signal) => signal.RemoveAllListeners();

}
