using UnityEngine;
using UnityEngine.Events;

public class InvertDoor : SelectObject,ISignal
{
 //   public UnityEvent<string> OnSignal { get; } = new UnityEvent<string>();

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ReceiveSignal()
    {
        anim.SetTrigger("Open");
    }

   

    
}
