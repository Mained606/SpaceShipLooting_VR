using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.ParticleSystem;

public class GasOpen : MonoBehaviour, ISignal
{
    public static UnityEvent<bool> GasGasGas = new UnityEvent<bool>();

    private ParticleSystem particle;
    public AudioSource girik;

    private void Start()
    {
        // ParticleSystem 가져오기
        particle = GetComponent<ParticleSystem>();
        particle.Stop(); // 초기 상태에서 정지

        ValveMove.OnValve.AddListener(Receiver);
    }

    public void Sender(bool state)
    {
        GasGasGas?.Invoke(state);
    }

    public void Receiver(bool state)
    {
        if (state) particle.Play();
        girik.Play();
        Sender(true);
    }

    public void Clear(UnityEvent<bool> signal)
    {
        signal.RemoveAllListeners();
    }
}
