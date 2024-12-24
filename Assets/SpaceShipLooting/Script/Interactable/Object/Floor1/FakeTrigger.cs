using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class FakeTrigger :XRSimpleInteractableOutline, ISignal
{
   private int trueCount = 0; // 신호를 받은 횟수
   private Collider col;

   protected  override void Start()
    {
      Floor1Console.consoleCheck.AddListener(Receiver);
      col = GetComponent<Collider>();
   }

    public void Clear(UnityEvent<bool> signal)
    {
        signal.RemoveAllListeners();
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        JsonTextManager.instance.OnDialogue("stage1-2");
    }

    public void Receiver(bool state)
    {
        if(state)
        {
            trueCount++;
            if (trueCount >= 3)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
    public void Sender(bool state) { }
}
