using UnityEngine;
using UnityEngine.Events;


public class CollisionObject : MonoBehaviour, ISignal
{
    public UnityEvent<string> OnSignal { get; } = new UnityEvent<string>();

    [SerializeField] private bool SignalToss = true; // Inspector에서 신호 발사 여부 설정

    private void OnCollisionEnter(Collision collision)
    {
        if (SignalToss) // emitSignal이 true일 때만 신호 발사
        {
            OnSignal.Invoke($"Selected: {gameObject.name}");
        }
    }

    /*private void OnDestroy()
   {
       OnSignal.RemoveAllListeners(); // 파괴가능 오브젝트일 시 활성화
   }*/

    public void ReceiveSignal()
    {
        // 필요에 따라 구현
    }
}
