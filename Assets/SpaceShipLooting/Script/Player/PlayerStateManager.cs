using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
/// <summary>
///  스테이트 매니저
/// </summary>
public class PlayerStateManager : MonoBehaviour
{
    // 싱글톤 부분은 추후 제거 필요
    public static PlayerStateManager Instance { get; private set; }
    
    // 왼쪽 컨트롤러 조이스틱 값 받아오기 (New Input System 사용)
    public Vector2 MoveInput => moveProvider.leftHandMoveInput.ReadValue();
    // 스텔스 모드 활성화 여부 (true: 스텔스 모드, false: 일반 모드)
    public bool IsStealthMode { get; set; } = false;
    // 뛰기 모드 활성화 여부 
    public bool IsRunningMode { get; set; } = false;

    // 현재 플레이어 상태
    private IPlayerState currentState;

    private PlayerInputHandler playerInputHandler;

    private DynamicMoveProvider moveProvider;
    public DynamicMoveProvider MoveProvider => moveProvider;

    // // 스텔스 상태 변경을 알리는 UnityEvent (외부 구독 가능)
    [HideInInspector] public UnityEvent<bool> OnStealthStateChanged = new UnityEvent<bool>();
    // // Running State 변경을 알리는 UnityEvent
    [HideInInspector] public UnityEvent<bool> OnRunningStateChanged = new UnityEvent<bool>();

    public static Transform PlayerTransform;

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

        PlayerTransform = this.transform;
    }
    
    private void Start()
    {
        

        // PlayerStatsConfig 설정
        if (GameManager.Instance.PlayerStatsData == null)
        {
            Debug.Log("PlayerStatsConfig가 설정되지 않았습니다!");
            enabled = false; // Config가 없으면 실행하지 않음
            return;
        }

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
            playerInputHandler.OnRunningToggle.AddListener(ToggleRunningMode);
            // 다른 기능 추가시 다른 이벤트 리스너 추가 가능...
        }


        // 시작시 상태 설정
        if (GameManager.Instance.PlayerStatsData.enableStealthMode)
        {
            IsStealthMode = true;
            currentState = new PlayerStealthState();
        }
        else if (GameManager.Instance.PlayerStatsData.enableRunningMode)
        {
            IsRunningMode = true;
            currentState = new PlayerRunningState();
        }
        else
        {
            currentState = new PlayerIdleState();
        }

        currentState.EnterState(this);
    }

    // 플레이어 입력 핸들러에서 이벤트 리스너 제거 함수 (메모리 누수 방지)
    private void OnDestroy()
    {
        if (playerInputHandler != null)
        {
            // 이벤트 구독 제거
            playerInputHandler.OnStealthToggle.RemoveListener(ToggleStealthMode);
            playerInputHandler.OnRunningToggle.RemoveListener(ToggleRunningMode);
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
        if(newState == currentState) return;

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
        GameManager.Instance.PlayerStatsData.enableStealthMode = !GameManager.Instance.PlayerStatsData.enableStealthMode;
        // // 현재 스텔스 상태를 반대로 변경
        IsStealthMode = !IsStealthMode;
        
        // // 외부에 스텔스 상태 변경 알림 (구독된 이벤트 호출) - 몬스터 Ai에 리스너 추가 필요
        OnStealthStateChanged?.Invoke(IsStealthMode);

        // // 스텔스 모드 상태에 따라 상태 전환
        SwitchState(IsStealthMode ? new PlayerStealthState() : new PlayerIdleState());
        // // 스텔스 모드 변경 사항 로그 출력 (디버그용)
        // // Debug.Log($"Stealth Mode: {IsStealthMode}");
    }
    private void ToggleRunningMode()
    {
        GameManager.Instance.PlayerStatsData.enableRunningMode = !GameManager.Instance.PlayerStatsData.enableRunningMode;

        IsRunningMode = !IsRunningMode;
        
        OnRunningStateChanged?.Invoke(IsRunningMode);

        SwitchState(IsRunningMode ? new PlayerRunningState() : new PlayerIdleState());
    }

    
}