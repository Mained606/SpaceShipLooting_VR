using UnityEngine;

public class GasOpen : MonoBehaviour, ISignalReceiver
{
    private ParticleSystem paticle;

    private void Start()
    {
        paticle = GetComponent<ParticleSystem>();
        paticle.Stop();
    }

    public void ReceiveSignal(string signal)
    {
      paticle.Play();
    }
}

