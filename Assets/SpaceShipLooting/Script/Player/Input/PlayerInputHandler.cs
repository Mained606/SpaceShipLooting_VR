using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    // 스텔스 버튼 입력 액션 (Unity의 New Input System 사용)
    public InputActionProperty stealthButton;

    // 스텔스 모드 토글 이벤트 (UnityEvent를 통해 외부 구독 가능)
    [HideInInspector] public UnityEvent OnStealthToggle = new UnityEvent();

    private void Update()
    {
        // 스텔스 모드 입력받기
        HandleStealthInput();
    }

    private void HandleStealthInput()
    {
        // 키 입력 받아서 토글로 스텔스 모드 변경
        if (stealthButton.action.WasPressedThisFrame())
        {
            // 스텔스 토글 이벤트 호출
            OnStealthToggle?.Invoke();
        }
    }
}
