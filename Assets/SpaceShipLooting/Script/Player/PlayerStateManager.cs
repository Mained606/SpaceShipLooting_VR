using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
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

    [SerializeField] private GameObject gameOverUI;

    // 플레이어 치트 관련
    private bool cheatMode = false;
    public bool CheatMonde => cheatMode;
    private int previousAmmo;
    private float previousBulletDamage;
    private float previousSwordDamage;
    private Health playerHealth;

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
        GameOverEvent gameOverEvent = GetComponentInChildren<GameOverEvent>();
        if (gameOverEvent != null)
        {
            gameOverEvent.PlayerDataInit.AddListener(PlayerDataInit);
        }
        // Destructable 이벤트 구독
        Destructable destructable = GetComponent<Destructable>();
        if (destructable != null)
        {
            destructable.OnPlayerDestroyed.AddListener(HandlePlayerDeath);
        }

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
        if (playerInputHandler)
        {
            // playerInputHandler의 OnStealthToggle에 ToggleStealthMode 메서드 연결
            playerInputHandler.OnStealthToggle.AddListener(ToggleStealthMode);
            playerInputHandler.OnRunningToggle.AddListener(ToggleRunningMode);
            playerInputHandler.OnCheatButtonToggle.AddListener(ToggleCheatButton);
            playerInputHandler.OnNextSceneButton.AddListener(ToggleNextSceneButton);
            // 다른 기능 추가시 다른 이벤트 리스너 추가 가능...
            OnStealthStateChanged.AddListener(UpdateStealthModeData);
            OnRunningStateChanged.AddListener(UpdateRunningModeData);
        }

        playerHealth = GetComponent<Health>();
        {
            if (playerHealth == null)
            {
                Debug.Log("DynamicMoveProvider를 찾을 수 없습니다!");
                enabled = false; //만약 null이면 추가 오류를 방지하기 위해 스크립트 비활성화
                return;
            }
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

    private void ToggleNextSceneButton()
    {
        if (!cheatMode) return;

        Debug.Log("넥스트 씬 이동");

        SceneFader fader = GameManager.Instance.GetComponentInChildren<SceneFader>();
        if (fader != null)
        {
            //현재 씬 번호
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            int nextSceneIndex;
            if (currentSceneIndex == SceneManager.sceneCountInBuildSettings - 1)
            {
                // 만약 다음씬이 마지막 씬 이라면 1번 씬으로 이동
                nextSceneIndex = 1;
            }
            else
            {
                // 다음 씬 인덱스
                nextSceneIndex = currentSceneIndex + 1;
            }

            fader.FadeTo(nextSceneIndex);
        }
    }

    // 치트 모드 추가
    private void ToggleCheatButton()
    {
        cheatMode = !cheatMode;
        if (cheatMode)
        {
            Debug.Log("치트 모드 온");
            AudioManager.Instance.Play("Button", false);
            previousAmmo = GameManager.Instance.PlayerStatsData.maxAmmo;
            previousBulletDamage = GameManager.Instance.PlayerStatsData.bulletDamage;
            previousSwordDamage = GameManager.Instance.PlayerStatsData.knifeDamage;

            playerHealth.IsInvincible = true;
            GameManager.Instance.PlayerStatsData.maxAmmo = 999;
            GameManager.Instance.PlayerStatsData.bulletDamage = 999f;
            GameManager.Instance.PlayerStatsData.knifeDamage = 999f;
        }
        else
        {
            Debug.Log("치트 모드 오프");
            playerHealth.IsInvincible = false;
            GameManager.Instance.PlayerStatsData.maxAmmo = previousAmmo;
            GameManager.Instance.PlayerStatsData.bulletDamage = previousBulletDamage;
            GameManager.Instance.PlayerStatsData.knifeDamage = previousSwordDamage;
        }
    }

    // 재도전시 플레이어 데이터 초기화
    private void PlayerDataInit()
    {
        StartCoroutine(PlayerDataReset());
    }
    private IEnumerator PlayerDataReset()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("플레이어 초기화 스타트");
        Health health = GetComponentInChildren<Health>();
        health.isDead = false;
        health.CurrentHealth = health.maxHealth;
        GameManager.Instance.PlayerStatsData.currentAmmo = 0;
        GameManager.Instance.PlayerStatsData.maxAmmo = 7;
        Pistol pistol = Object.FindFirstObjectByType<Pistol>();
        if (pistol != null)
        {
            pistol.ResetAmmo();
        }
        else
        {
            Debug.LogWarning("Pistol 오브젝트를 찾을 수 없습니다.");
        }
        Debug.Log("플레이어 초기화 종료");
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

        Destructable destructable = GetComponent<Destructable>();
        if (destructable != null)
        {
            destructable.OnObjectDestroyed.RemoveListener(HandlePlayerDeath);
        }

        GameOverEvent gameOverEvent = GetComponentInChildren<GameOverEvent>();
        if (gameOverEvent != null)
        {
            gameOverEvent.PlayerDataInit.RemoveListener(PlayerDataInit);
        }

        OnStealthStateChanged.RemoveListener(UpdateStealthModeData);
        OnRunningStateChanged.RemoveListener(UpdateRunningModeData);
    }

    private void HandlePlayerDeath(GameObject player)
    {
        if (player.CompareTag("Player"))
        {
            Debug.Log("[PlayerStateManager] 플레이어가 사망했습니다.");
            ShowGameOverUI();
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
        if (newState == currentState) return;

        // 현재 상태 종료
        currentState.ExitState(this);
        // 새로운 상태로 전환
        currentState = newState;
        // 새로운 상태로 진입
        currentState.EnterState(this);

        // 이벤트 호출: 상태 변화 알림
        TriggerStateChangeEvent();
    }

    // 상태 변화 이벤트 호출
    private void TriggerStateChangeEvent()
    {
        // 현재 상태에 따라 이벤트 호출
        if (currentState is PlayerStealthState)
        {
            OnStealthStateChanged?.Invoke(true);
            OnRunningStateChanged?.Invoke(false);
        }
        else if (currentState is PlayerRunningState)
        {
            OnStealthStateChanged?.Invoke(false);
            OnRunningStateChanged?.Invoke(true);
        }
        else if (currentState is PlayerIdleState)
        {
            OnStealthStateChanged?.Invoke(false);
            OnRunningStateChanged?.Invoke(false);
        }
    }

    // 스텔스 모드 데이터 업데이트
    private void UpdateStealthModeData(bool isStealth)
    {
        GameManager.Instance.PlayerStatsData.enableStealthMode = isStealth;
    }

    // 달리기 모드 데이터 업데이트
    private void UpdateRunningModeData(bool isRunning)
    {
        GameManager.Instance.PlayerStatsData.enableRunningMode = isRunning;
    }

    // 스텔스 모드 토글
    private void ToggleStealthMode()
    {
        // 런닝 모드가 활성화되어 있을 경우 먼저 끔
        if (IsRunningMode)
        {
            IsRunningMode = false;
            OnRunningStateChanged?.Invoke(false);
            GameManager.Instance.PlayerStatsData.enableRunningMode = false;
        }

        // 스텔스 모드 전환
        IsStealthMode = !IsStealthMode;
        OnStealthStateChanged?.Invoke(IsStealthMode);
        GameManager.Instance.PlayerStatsData.enableStealthMode = IsStealthMode;

        // 상태 전환
        SwitchState(IsStealthMode ? new PlayerStealthState() : new PlayerIdleState());
    }
    private void ToggleRunningMode()
    {
        // 스텔스 모드가 활성화되어 있을 경우 먼저 끔
        if (IsStealthMode)
        {
            IsStealthMode = false;
            OnStealthStateChanged?.Invoke(false);
            GameManager.Instance.PlayerStatsData.enableStealthMode = false;
        }

        // 런닝 모드 전환
        IsRunningMode = !IsRunningMode;
        OnRunningStateChanged?.Invoke(IsRunningMode);
        GameManager.Instance.PlayerStatsData.enableRunningMode = IsRunningMode;

        // 상태 전환
        SwitchState(IsRunningMode ? new PlayerRunningState() : new PlayerIdleState());
    }

    private void ShowGameOverUI()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true); // 게임오버 UI 활성화
        }
        else
        {
            Debug.LogError("GameOver UI가 설정되지 않았습니다!");
        }
    }

}