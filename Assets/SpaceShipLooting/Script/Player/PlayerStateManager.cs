using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
/// <summary>
///  스테이트 매니저
/// </summary>
public class PlayerStateManager : MonoBehaviour
{
    // 싱글톤
    public static PlayerStateManager Instance { get; private set; }
    
    // 왼쪽 컨트롤러 조이스틱 값 받아오기 (New Input System 사용)
    public Vector2 MoveInput => moveProvider.leftHandMoveInput.ReadValue();

    // 스텔스 모드 활성화 여부 (true: 스텔스 모드, false: 일반 모드)
    public bool IsStealthMode { get; private set; } = false;

    // 뛰기 모드 활성화 여부 
    public bool IsrunningMode { get; private set; } = false;

    // 현재 플레이어 상태
    private IPlayerState currentState;

    private PlayerInputHandler playerInputHandler;
    public DynamicMoveProvider moveProvider;

    // 스텔스 상태 변경을 알리는 UnityEvent (외부 구독 가능)
    [HideInInspector] public UnityEvent<bool> OnStealthStateChanged = new UnityEvent<bool>();

    // 싱글톤 초기화
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        // 초기 상태를 Idle로 설정
        currentState = new PlayerIdleState();
        currentState.EnterState(this);

        // DynamicMoveProvider 참조
        moveProvider = GetComponentInChildren<DynamicMoveProvider>();
        if (moveProvider == null)
        {
            Debug.Log("DynamicMoveProvider를 찾을 수 없습니다!");
            enabled = false; //만약 null이면 추가 오류를 방지하기 위해 스크립트 비활성화
            return;
        }

        // PlayerInputHandler 컴포넌트 참조 및 이벤트 연결
        playerInputHandler = GetComponent<PlayerInputHandler>();
        if(playerInputHandler)
        {
            // playerInputHandler의 OnStealthToggle에 ToggleStealthMode 메서드 연결
            playerInputHandler.OnStealthToggle.AddListener(ToggleStealthMode);
            playerInputHandler.OnRuuningToggle.AddListener(ToggleRunningMode);
            // 다른 기능 추가시 다른 이벤트 리스너 추가 가능...


        }
    }

    // 플레이어 입력 핸들러에서 이벤트 리스너 제거 함수 (메모리 누수 방지)
    private void OnDestroy()
    {
        if (playerInputHandler != null)
        {
            // 이벤트 구독 제거
            playerInputHandler.OnStealthToggle.RemoveListener(ToggleStealthMode);
            playerInputHandler.OnRuuningToggle.RemoveListener(ToggleRunningMode);
        }
        
    }

    private void Update()
    {
        // 현재  State 상태를 매 프레임 업데이트
        currentState.UpdateState(this);
    }

    // 상태 전환 메서드 (외부 사용 필요)
    public void SwitchState(IPlayerState newState)
    {
        // 현재 상태 종료
        currentState.ExitState(this);
        // 새로운 상태로 전환
        currentState = newState;
        // 새로운 상태로 진입
        currentState.EnterState(this);
    }

    // 스텔스 모드 토글
    private void ToggleStealthMode()
    {
        // 현재 스텔스 상태를 반대로 변경
        IsStealthMode = !IsStealthMode;
        
        // 외부에 스텔스 상태 변경 알림 (구독된 이벤트 호출) - 몬스터 Ai에 리스너 추가 필요
        OnStealthStateChanged?.Invoke(IsStealthMode);

        // 스텔스 모드 상태에 따라 상태 전환
        if (IsStealthMode)
        {
            SwitchState(new PlayerStealthState());
        }
        else
        {
            SwitchState(new PlayerIdleState());
        }
        // 스텔스 모드 변경 사항 로그 출력 (디버그용)
        // Debug.Log($"Stealth Mode: {IsStealthMode}");
    }
    private void ToggleRunningMode()
    {
        IsrunningMode = !IsrunningMode;

        if(IsrunningMode)
        {
            SwitchState(new PlayerRuningState());
        }
        else
        {
            SwitchState(new PlayerIdleState());
        }



    }

}