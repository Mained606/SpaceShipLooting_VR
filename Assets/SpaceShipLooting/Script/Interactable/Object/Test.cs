using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class Test : MonoBehaviour,ISignal
{

    private void Start()
    {
       // GasOpen.GasGasGas.AddListener(Receiver);
     //   KeyPadUI.codeCheck.AddListener(Receiver);
    }

    public void Clear(UnityEvent<bool> signal)
    {
      signal.RemoveAllListeners();
    }

    public void Receiver(bool state)
    {
        if (state== true)
        {
            Debug.Log("Ture");
        }

        if(state == false)
        {
            Debug.Log("False");
        }

    }
    public void Sender(bool state)
    {
    }

    
}
