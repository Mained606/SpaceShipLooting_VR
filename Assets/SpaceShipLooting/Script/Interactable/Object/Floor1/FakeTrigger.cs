using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class FakeTrigger :XRSimpleInteractableOutline, ISignal
{
     private Collider col;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   protected  override void Start()
    {
      Floor1Console.consoleCheck.AddListener(Receiver);
        col = GetComponent<Collider>();
       
    }

    // Update is called once per frame
   
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
        this.gameObject.SetActive(false);
       
    }

    public void Sender(bool state)
    {

    }

}
