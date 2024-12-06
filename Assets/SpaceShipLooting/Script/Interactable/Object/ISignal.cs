using UnityEngine;
using UnityEngine.Events;

public interface ISignal
{
    // UnityEvent는 Unity에서 제공하는 이벤트 시스템입니다.
    // 이 인터페이스의 OnSignal은 신호를 발행하는 이벤트입니다.
    UnityEvent<string> OnSignal { get; }

    // GetGameObject는 이 인터페이스를 구현한 오브젝트가 어떤 GameObject에 붙어 있는지를 반환합니다.
    // 예를 들어, 상호작용할 오브젝트의 이름이나 태그를 가져올 때 유용합니다.
    GameObject GetGameObject();

    // ClearListeners는 신호 이벤트에 등록된 모든 리스너(리스너 = 이벤트를 듣고 반응하는 코드)를 제거합니다.
    // 이를 통해 메모리 누수나 잘못된 동작을 방지할 수 있습니다.
    void ClearListeners();
}
