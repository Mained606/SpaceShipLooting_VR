using UnityEngine;
using UnityEngine.Events;

public class Third : MonoBehaviour, ISignal
{
    private Animator anim;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Animator 가져오기
        anim = GetComponentInParent<Animator>();
        if (anim == null)
        {
            anim = GetComponentInParent<Animator>();
        }

        KeyPadUI.codeCheck.AddListener(Receiver);  
    }

    public void Clear(UnityEvent<bool> signal)
    {
        signal.RemoveAllListeners();
    }

    public void Receiver(bool state)
    {
        if (state == true)
        {
            anim.SetTrigger("Open");
        }
    }

    public void Sender(bool state)
    {
    }

}
