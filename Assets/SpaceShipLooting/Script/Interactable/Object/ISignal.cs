using UnityEngine;
using UnityEngine.Events;

public interface ISignal
{
    UnityEvent<string> OnSignal { get; }  // 신호 발행 이벤트
    GameObject GetGameObject();           // 발신자의 GameObject 참조
    void ClearListeners();                // 리스너 정리
}
