using UnityEngine;

// ISignalReceiver는 신호를 수신하는 오브젝트가 구현해야 하는 규칙을 정의합니다.
public interface ISignalReceiver
{
    // Sender로부터 전달받은 신호 데이터를 처리합니다.
    void ReceiveSignal(string signal);

    // 신호를 수신한 오브젝트(GameObject)를 반환합니다.
    GameObject GetGameObject();
}
