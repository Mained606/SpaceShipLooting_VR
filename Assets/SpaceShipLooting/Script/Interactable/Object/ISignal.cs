using UnityEngine.Events;

public interface ISignal
{
    UnityEvent<string> OnSignal { get; } // 신호 발행 이벤트

    void ReceiveSignal(); // 신호 수신 처리 메서드
}