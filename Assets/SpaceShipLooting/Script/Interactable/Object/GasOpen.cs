using UnityEngine;
using UnityEngine.Events;

public class GasOpen : MonoBehaviour, ISignal
{
    public UnityEvent<string> OnSignal { get; } = new UnityEvent<string>();

    private ParticleSystem particle;

    private void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>(true);
    }

    public void ReceiveSignal()
    {
        particle.gameObject.SetActive(true);
    }

    

   
}
