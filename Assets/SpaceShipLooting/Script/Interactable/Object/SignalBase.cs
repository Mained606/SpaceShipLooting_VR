using UnityEngine;
using UnityEngine.Events;

public interface ISignal
{
 
    // 신호 발행 메서드
    void Sender(bool state);

    // 신호 수신 메서드
    void Receiver(bool state);

    // 리스너 초기화 메서드
    void Clear(UnityEvent<bool> signal);
}
