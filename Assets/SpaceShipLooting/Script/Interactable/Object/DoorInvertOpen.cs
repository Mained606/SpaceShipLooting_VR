using UnityEngine;
using UnityEngine.Events;

public class DoorInvertOpen : MonoBehaviour, ISignal
{
    public UnityEvent<string> OnSignal => throw new System.NotImplementedException();

    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }


    public void ReceiveSignal()
    {
        if(anim != null)
        {
            anim.SetTrigger("Open");
        }
       

    }
}