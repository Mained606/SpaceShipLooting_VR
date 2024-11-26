using UnityEngine;

public class AiTest : MonoBehaviour
{
    private bool isPlayerInStealthMode;

    private void Start()
    {
        // 플레이어 State 에 이벤트 구독
        PlayerStateManager.Instance.OnStealthStateChanged.AddListener(PlayerStealthCheck);
    }

    // 플레이어 스텔스 모드 변경 이벤트 함수
    private void PlayerStealthCheck(bool isStealth)
    {
        // 플레이어 스텔스 모드 변경 여부 저장
        isPlayerInStealthMode = isStealth;
        Debug.Log("PlayerStealthCheck : " + isStealth);
    }

    // 사용 예시 코드
    // private void aiLogic()
    // {
    //     // AI 행동 변경 로직 추가
    //     if (isPlayerInStealthMode)
    //     {
    //         // 플레이어가 스텔스 모드일 때 AI 행동

    //     }
    //     else
    //     {
    //         // 플레이어가 일반 모드일 때 AI 행동

    //     }
    // }
}
