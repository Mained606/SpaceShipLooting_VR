using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WarningLight : MonoBehaviour, ISignal
{
    private Light lightall;

    void Start()
    {
        Floor1Console.consoleCheck.AddListener(Receiver);

        lightall = GetComponentInChildren<Light>();
    }

    public void Receiver(bool state)
    {
        if (!state)
        {
            StartCoroutine(Warning());
        }
    }
    IEnumerator Warning()
    {
        yield return  new WaitForSeconds(1f);
        lightall.gameObject.SetActive(true);
        yield return new WaitForSeconds(10f);
        lightall.gameObject.SetActive(false);
    }
    public void Clear(UnityEvent<bool> signal) { }
    public void Sender(bool state) { }
}
