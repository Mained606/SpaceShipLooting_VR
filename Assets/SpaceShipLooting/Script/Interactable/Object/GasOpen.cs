using UnityEngine;

public class GasOpen : MonoBehaviour, ISignalReceiver
{
    private ParticleSystem particle;

    private void Start()
    {
        // ParticleSystem 가져오기
        particle = GetComponent<ParticleSystem>();
        if (particle != null)
        {
            particle.Stop(); // 초기 상태에서 정지
        }
        else
        {
            Debug.LogError($"[{gameObject.name}] ParticleSystem is missing!");
        }
    }

    public void ReceiveSignal(string signal)
    {
        Debug.Log($"[{gameObject.name}] Received signal: {signal}");

        if (signal == "OpenGas" && particle != null)
        {
            particle.Play();
            Debug.Log($"[{gameObject.name}] Gas particle started.");
        }
        else
        {
            Debug.LogError($"[{gameObject.name}] Signal not matched or ParticleSystem is missing!");
        }
    }

    public GameObject GetGameObject()
    {
        return gameObject; // 현재 오브젝트 반환
    }
}
