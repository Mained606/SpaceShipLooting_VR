using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;

public class LightOff : MonoBehaviour, ISignal
{
    private Light[]lights;
    private int TrueCount = 0;

    void Start()
    {
        Floor1Console.consoleCheck.AddListener(Receiver);

        lights = FindObjectsOfType<Light>();
    }

    private void lightoff()
    {
            foreach (var light in lights)
            {
                light.enabled = false;
            }
    }


    public void Clear(UnityEvent<bool> signal) =>  signal.RemoveAllListeners();

    public void Receiver(bool state)
    {
        if(state)
        {
            TrueCount++;
            if (TrueCount >= 3) lightoff();
        }
    }

    public void Sender(bool state) { }
   
}
