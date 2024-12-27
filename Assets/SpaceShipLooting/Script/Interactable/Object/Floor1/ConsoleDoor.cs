using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ConsoleDoor : XRSimpleInteractableOutline, ISignal
{
    private Animator anim;
    private int falseCount = 0; // 신호를 받은 횟수
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        // Animator 가져오기
        anim = GetComponentInParent<Animator>();
        Floor1Console.consoleFalse.AddListener(Receiver);
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        anim.SetTrigger("Open");
        AudioManager.Instance.Play("DoorOpen_");
    }

    public void Sender(bool state) { }
    public void Clear(UnityEvent<bool> signal) => signal.RemoveAllListeners();

    public void Receiver(bool state)
    {
        if (!state)
        {
            falseCount++;
            if (falseCount >= 2)
            {
                anim.SetTrigger("Close");
                AudioManager.Instance.Play("DoorOpen_");
                AudioManager.Instance.Play("F1Siren",true,0.5f,0.7f);
                Debug.Log("문 닫힘");
            }
        }
    }
    private void OnDisable()
    {
        AudioManager.Instance.Stop("F1Siren");
    }
}
