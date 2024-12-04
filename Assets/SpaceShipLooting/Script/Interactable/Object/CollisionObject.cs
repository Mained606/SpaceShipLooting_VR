using UnityEngine;
using UnityEngine.Events;

public class CollisionObject : MonoBehaviour, ISignal
{
    public UnityEvent<string> OnSignal { get; } = new UnityEvent<string>();

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void ClearListeners()
    {
        OnSignal.RemoveAllListeners();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌 시 신호 발행
        Debug.Log($"Collision detected on {gameObject.name} with {collision.gameObject.name}");
        OnSignal.Invoke($"Collision: {gameObject.name}");
    }
}
