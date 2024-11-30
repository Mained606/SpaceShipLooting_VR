using UnityEngine;
using UnityEngine.Events;

public class NightVisionSystem : MonoBehaviour
{
    [Header("Night Vision Settings")]
    [SerializeField] private Canvas nightVisionCanvas; // 노이즈/스캔라인 오버레이
    [SerializeField] private Canvas leftPanel;
    [SerializeField] private Canvas rightPanel;

    private bool isNightVisionActive = false; // 야간 투시경 활성화 상태

    private PlayerInputHandler playerInputHandler;

    private void Start()
    {
        playerInputHandler = GetComponentInParent<PlayerInputHandler>();

        if(playerInputHandler)
        {
            // playerInputHandler의 OnStealthToggle에 ToggleStealthMode 메서드 연결
            playerInputHandler.OnNightVisionToggle.AddListener(ToggleNightVision);
        }
    }


    private void ToggleNightVision()
    {
        isNightVisionActive = !isNightVisionActive;

        nightVisionCanvas.gameObject.SetActive(isNightVisionActive);
        leftPanel.gameObject.SetActive(isNightVisionActive);
        rightPanel.gameObject.SetActive(isNightVisionActive);
    }
}
