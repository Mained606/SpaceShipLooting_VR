using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class SpaceBossController : BossController
{
    [SerializeField] private Transform target; // 타겟 참조 (캡슐화)
    [SerializeField] private GameObject eye; // 눈 오브젝트
    [SerializeField] public GameObject[] cores; // 코어 리스트

    private Health health;


    // 모든 코어 파괴 여부
    private bool allCoresDestroyed = false;
    public bool AllCoresDestroyed => allCoresDestroyed;
    private bool eyeChange = false;

    //눈 포지션 설정
    [Header("눈 위치 설정 및 탐색 범위")]
    [SerializeField] private float eyePositionY = 1.5f; // 눈 위치 조정 값
    [SerializeField] private float searchRange = 15f; // 탐색 범위

    // 상태 지속 시간
    [Header("각 상태별 딜레이 설정")]
    [SerializeField] private float defenceDuration = 5f;
    [SerializeField] private float empAttackDuration = 5f;
    [SerializeField] private float laserAttackDuration = 5f;
    [SerializeField] private float coreExplosionDuration = 5f;
    [SerializeField] private float groggyDuration = 5f;

    // 스킬 관련 값
    [Header("코어 폭발 스킬 설정")]
    [SerializeField] private float explosionDelay = 3f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float explosionDamage = 50f;



    // 읽기 전용 프로퍼티 추가
    public float DefenceDuration => defenceDuration;
    public float EMPAttackDuration => empAttackDuration;
    public float LaserAttackDuration => laserAttackDuration;
    public float CoreExplosionDuration => coreExplosionDuration;
    public float GroggyDuration => groggyDuration;

    public float ExplosionDelay => explosionDelay;
    public float ExplosionRadius => explosionRadius;
    public float ExplosionDamage => explosionDamage;

    // 공격 패턴 이벤트 리스트
    private List<System.Action> attackPatterns;
    private List<System.Action> remainingAttackPatterns;

    // 이벤트
    public UnityEvent OnCoreRecovered { get; private set; } = new UnityEvent();
    public UnityEvent OnAllCoresDestroyed { get; private set; } = new UnityEvent();
    public UnityEvent OnLaserStateStarted { get; private set; } = new UnityEvent();
    public UnityEvent OnLaserStateEnded { get; private set; } = new UnityEvent();

    protected void Awake()
    {
        health = GetComponent<Health>();
        if (health == null)
        {
            Debug.LogError("Health 컴포넌트를 찾을 수 없습니다!");
            return;
        }

        health.IsInvincible = true; // 초기 무적 상태 설정

        InitializeTarget();
        InitializeCores();
        InitializeAttackPatterns();
    }

    protected override void Start()
    {
        base.Start();

        RegisterStates();
        ChangeState<SpaceBossIdleState>(); // 초기 상태 설정

        // 이벤트 등록
        OnAllCoresDestroyed.AddListener(HandleAllCoresDestroyed);
        OnCoreRecovered.AddListener(HandleCoreRecovered);

        if (eye.TryGetComponent(out BossEye bossEye))
        {
            bossEye.onDamageReceived.AddListener(SpaceBossGroggyState);
            OnLaserStateStarted.AddListener(bossEye.OnLaserStateStarted);
            OnLaserStateEnded.AddListener(bossEye.OnLaserStateEnded);
        }
    }

    // FSM 스테이트 등록
    private void RegisterStates()
    {
        stateMachine.AddState(new SpaceBossIdleState());
        stateMachine.AddState(new SpaceBossDefenceState());
        stateMachine.AddState(new SpaceBossAttackState());
        stateMachine.AddState(new SpaceBossCoreExplosionState());
        stateMachine.AddState(new SpaceBossEyeLaserState());
        stateMachine.AddState(new SpaceBossEMPAttackState());
        stateMachine.AddState(new SpaceBossGroggyState());
    }

    // 타겟 설정
    private void InitializeTarget()
    {
        if (target == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            target = player?.transform;

            if (target == null)
            {
                Debug.LogError("플레이어를 찾을 수 없습니다.");
            }
        }
    }

    // 코어 파괴 이벤트 구독
    private void InitializeCores()
    {
        foreach (var core in cores)
        {
            if (core != null && core.TryGetComponent(out Destructable destructable))
            {
                destructable.OnObjectDestroyed.AddListener(RemoveCore);
            }
        }
    }

    // 초기 공격 패턴 설정 
    private void InitializeAttackPatterns()
    {
        attackPatterns = new List<System.Action>
        {
            SpaceBossCoreExplosionState,
            SpaceBossEyeLaserState,
            SpaceBossEMPAttackState
        };

        UpdateAttackPatterns();
    }

    private void UpdateAttackPatterns()
    {
        remainingAttackPatterns = allCoresDestroyed
            ? new List<System.Action> { SpaceBossEyeLaserState, SpaceBossEMPAttackState }
            : new List<System.Action>(attackPatterns);
    }

    // 보스 공격 패턴 함수
    public void ExecuteRandomAttackPattern()
    {
        if (remainingAttackPatterns.Count == 0)
        {
            UpdateAttackPatterns();
        }

        // 무작위로 공격 패턴 선택
        int randomIndex = Random.Range(0, remainingAttackPatterns.Count);
        System.Action selectedAttack = remainingAttackPatterns[randomIndex];
        remainingAttackPatterns.RemoveAt(randomIndex);

        // 선택 된 어택 신호 전달
        selectedAttack?.Invoke();
    }

    // 타겟과의 거리 계산
    public bool IsTargetInRange()
    {
        if (target == null) return false;
        return Vector3.Distance(transform.position, target.position) <= searchRange;
    }

    // 코어가 파괴되었을 때 리스트에서 제거 해주는 함수
    public void RemoveCore(GameObject core)
    {
        var coreList = new List<GameObject>(cores);
        coreList.Remove(core);
        cores = coreList.ToArray();

        Debug.Log($"[SpaceBossController] 코어 제거됨: {core.name}");
        UpdateCoreState(); // 코어 상태 갱신
    }

    // 만약 모든 코어가 파괴되었는지 확인하고 본체의 무적 해제 및 눈알 위치 조정
    public void UpdateCoreState()
    {
        // 모든 코어가 파괴되었는지 확인 (코어가 더 이상 남아있지 않은 경우)
        allCoresDestroyed = cores.Length == 0;

        if (allCoresDestroyed)
        {
            // 모든 코어가 파괴되었을 때 이벤트 호출
            Debug.Log("모든 코어 파괴");
            OnAllCoresDestroyed?.Invoke();
        }
    }

    // 모든 코어가 파괴되었을 때 실행되는 함수
    private void HandleAllCoresDestroyed()
    {
        health.IsInvincible = false;
        Debug.Log("모든 코어 파괴 - 무적 해제");
        AdjustEyePosition(true);
    }

    // 코어가 회복되었을 때 실행되는 함수
    private void HandleCoreRecovered()
    {
        health.IsInvincible = true;
        Debug.Log("코어 회복 - 무적 설정");
        AdjustEyePosition(false);
    }

    public void AdjustEyePosition(bool isUp)
    {
        if (eyeChange == isUp) return;

        eyeChange = isUp;
        eye.SetActive(isUp);

        Vector3 newPosition = eye.transform.position;
        newPosition.y += isUp ? eyePositionY : -eyePositionY;
        eye.transform.position = newPosition;

        Debug.Log(isUp ? "눈 위치 올라감" : "눈 위치 낮아짐");
    }

    // 보스 상태 변환 함수들
    public void SpaceBossIdle() { ChangeState<SpaceBossIdleState>(); }
    public void SpaceBossDefenceState() { ChangeState<SpaceBossDefenceState>(); }
    public void SpaceBossAttackState() { ChangeState<SpaceBossAttackState>(); }
    public void SpaceBossCoreExplosionState() { ChangeState<SpaceBossCoreExplosionState>(); }
    public void SpaceBossEyeLaserState() { ChangeState<SpaceBossEyeLaserState>(); }
    public void SpaceBossEMPAttackState() { ChangeState<SpaceBossEMPAttackState>(); }
    public void SpaceBossGroggyState() { ChangeState<SpaceBossGroggyState>(); }

    // 서치 범위 그리기
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRange);
    }
}
