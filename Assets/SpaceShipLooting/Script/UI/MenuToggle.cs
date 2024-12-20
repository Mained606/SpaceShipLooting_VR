using UnityEngine;
using UnityEngine.InputSystem;

public class MenuToggle : MonoBehaviour
{
    public GameObject menuUI; // 메뉴 UI 캔버스
    public InputActionProperty toggleMenuAction; // Input System 액션

    private void OnEnable()
    {
        // 메뉴 버튼 입력 감지 이벤트 등록
        toggleMenuAction.action.performed += ToggleMenu;
    }

    private void OnDisable()
    {
        // 이벤트 해제
        toggleMenuAction.action.performed -= ToggleMenu;
    }

    private void ToggleMenu(InputAction.CallbackContext context)
    {
        // 메뉴 UI 활성화/비활성화 전환
        menuUI.SetActive(!menuUI.activeSelf);
    }
}
