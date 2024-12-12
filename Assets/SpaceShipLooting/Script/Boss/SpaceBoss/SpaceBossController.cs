using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class SpaceBossController : BossController
{
    [SerializeField] private Transform target; // 타겟 참조 (캡슐화)
    [SerializeField] private GameObject eye; // 눈 오브젝트
    [SerializeField] public GameObject[] cores; // 코어 리스트
    
    [SerializeField] public Canvas canvas;
    [SerializeField] public TextMeshProUGUI textbox;

    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Transform laserFirePoint;

    [SerializeField] public GameObject bossShield;
    [SerializeField] public List<GameObject> coreShields;




    private Health health;

    // 모든 코어 파괴 여부
    private bool allCoresDestroyed = false;
    public bool AllCoresDestroyed => allCoresDestroyed;

    //눈 포지션 설정
    [Header("눈 위치 설정 및 탐색 범위")]
    [SerializeField] private float eyePositionY = 2.5f; // 눈 위치 조정 값
    [SerializeField] private float eyeMoveSpeed = 5f; // 눈 이동 속도
    [SerializeField] private float searchRange = 15f; // 탐색 범위
    private Coroutine eyeMovementCoroutine;
    private bool eyeChange = false;

    // 상태 지속 시간
    [Header("각 상태별 딜레이 설정")]
    [SerializeField] private float defenceDuration = 5f;
    [SerializeField] private float empAttackDuration = 5f;
    [SerializeField] private float laserAttackDuration = 5f;
    [SerializeField] private float groggyDuration = 10f;

    // 스킬 관련 값
    [Header("코어 폭발 스킬 설정")]
    [SerializeField] private float explosionDelay = 5f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float explosionDamage = 50f;

    [Header("EMP 공격 스킬 설정")]
    [SerializeField] private float empAttackRadius = 15f;
    [SerializeField] private float empAttackDamage = 100f;

    [Header("레이저 공격 스킬 설정")]
    [SerializeField] private float laserCooldown = 5f;
    [SerializeField] private float laserChargeDuration = 3f;
    [SerializeField] private float trackingDuration = 3f;
    [SerializeField] private float laserDamage = 50f;
    [SerializeField] private float laserSpeed = 20f;
    [SerializeField] private float laserHealAmount = 100f;

    // 읽기 전용 프로퍼티 추가
    public Transform Target => target;
    public Transform EyeTransform => eye.transform;

    public float DefenceDuration => defenceDuration;
    public float EMPAttackDuration => empAttackDuration;
    public float LaserAttackDuration => laserAttackDuration;
    public float GroggyDuration => groggyDuration;

    public float ExplosionDelay => explosionDelay;
    public float ExplosionRadius => explosionRadius;
    public float ExplosionDamage => explosionDamage;

    public float EMPAttackRadius => empAttackRadius;
    public float EMPAttackDamage => empAttackDamage;

    public float LaserCooldown => laserCooldown;
    public float LaserChargeDuration => laserChargeDuration;
    public float TrackingDuration => trackingDuration;
    public float LaserDamage => laserDamage;
    public float LaserSpeed => laserSpeed;
    public float LaserHealAmount => laserHealAmount;

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

    public void RecoverCore(GameObject core)
    {
        // 이미 리스트에 있는 경우 중복 추가 방지
        if (System.Array.Exists(cores, existingCore => existingCore == core))
        {
            Debug.LogWarning("[SpaceBossController] 이미 존재하는 코어입니다.");
            return;
        }

        Debug.Log($"[SpaceBossController] 코어 회복됨: {core.name}");
        var coreList = new List<GameObject>(cores);
        coreList.Add(core);
        cores = coreList.ToArray();
        Debug.Log(cores +" 코어 리스트");

        OnCoreRecovered?.Invoke();
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

    public void AdjustEyePosition(bool isTracking, Vector3? targetPosition = null)
    {
        if (isTracking && targetPosition.HasValue)
        {
            EyeTransform.LookAt(targetPosition.Value); // 플레이어를 바라봄
        }
        else
        {
            EyeTransform.localRotation = Quaternion.identity; // 기본 위치로 복구
        }
    }

    public void FireLaser(Vector3 targetPosition)
    {
        // 레이저 발사 로직

        // 1. 레이저 VFX 생성
        GameObject laserGo = Instantiate(laserPrefab, laserFirePoint.position, Quaternion.identity);
        laserGo.transform.LookAt(targetPosition);
        
        // 2. 레이저의 이동 및 도달 처리
        Rigidbody rb = laserGo.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = laserFirePoint.forward * LaserSpeed;
        }

        // 3. 레이저 발사 소리 재생

    }

    public void AdjustEyePosition(bool isUp)
    {
        if (eyeChange == isUp) return; // 현재 상태와 동일하면 무시
        eyeChange = isUp;

        if (eyeMovementCoroutine != null)
        {
            StopCoroutine(eyeMovementCoroutine); // 기존 코루틴 중지
        }

        // 새로운 눈 이동 코루틴 시작
        Vector3 targetPosition = eye.transform.position;
        targetPosition.y += isUp ? eyePositionY : -eyePositionY;
        eyeMovementCoroutine = StartCoroutine(MoveEyeToPosition(targetPosition, isUp));
    }

    // 눈 위치를 부드럽게 이동시키는 코루틴
    private IEnumerator MoveEyeToPosition(Vector3 targetPosition, bool isUp)
    {
        if (isUp)
        {
            eye.SetActive(true);
        }

        while (Vector3.Distance(eye.transform.position, targetPosition) > 0.01f)
        {
            eye.transform.position = Vector3.Lerp(
                eye.transform.position,
                targetPosition,
                Time.deltaTime * eyeMoveSpeed
            );
            yield return null;
        }


        // 정확한 위치로 스냅
        eye.transform.position = targetPosition;

        if (!isUp)
        {
            eye.SetActive(false);
        }

        Debug.Log("눈 이동 완료");
    }
    // 코어 폭발 시작 함수
    public Coroutine StartSkillCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
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
