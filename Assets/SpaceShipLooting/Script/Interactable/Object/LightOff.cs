using UnityEngine;
using UnityEngine.Events;

public class LightOff : MonoBehaviour, ISignal
{
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Floor1Console.consoleCheck.AddListener(Receiver);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Clear(UnityEvent<bool> signal)
    {
       
    }

    public void Receiver(bool state)
    {
      
    }

    public void Sender(bool state)
    {
     
    }

}
