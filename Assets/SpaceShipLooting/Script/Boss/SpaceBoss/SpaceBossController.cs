using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class SpaceBossController : BossController
{
    [SerializeField] public Transform target;
    [SerializeField] private GameObject eye;
    [SerializeField] public GameObject[] cores;

    public bool allCoresDestroyed = false;

    // 올라올 때 눈알의 높이값
    [SerializeField] private float eyePositionY = 1.5f;
    
    [SerializeField] private float searchRange = 15f;
    [SerializeField] public float defenceDuration = 20f;

    // 각 보스 패턴 별 공격시작까지 걸리는 딜레이 시간
    [SerializeField] public float empAttackDuration = 5f;
    [SerializeField] public float laserAttackDuration = 5f;
    [SerializeField] public float coreExplosionDuration = 5f;


    // 공격 패턴 이벤트 리스트
    private List<System.Action> attackPatterns;
    private List<System.Action> remainingAttackPatterns;

    //코어 파괴 이벤트
    public UnityEvent onAllCoresDestroyed;

    protected override void Start()
    {
        base.Start();

        // 스테이트 머신에 스테이드 등록
        stateMachine.AddState(new SpaceBossIdleState());
        stateMachine.AddState(new SpaceBossDefenceState());
        stateMachine.AddState(new SpaceBossAttackState());
        stateMachine.AddState(new SpaceBossCoreExplosionState());
        stateMachine.AddState(new SpaceBossEyeLaserState());
        stateMachine.AddState(new SpaceBossEMPAttackState());
        
        ChangeState<SpaceBossIdleState>();

        // 공격 패턴 초기화
        attackPatterns = new List<System.Action>
        {
            SpaceBossCoreExplosionState,
            SpaceBossEyeLaserState,
            SpaceBossEMPAttackState
        };
        remainingAttackPatterns = new List<System.Action>(attackPatterns);
        
        // Player 태그가 붙은 게임 오브젝트를 서치
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }

        // 코어 파괴 이벤트 등록
        if (onAllCoresDestroyed == null)
        {
            onAllCoresDestroyed = new UnityEvent();
        }
        onAllCoresDestroyed.AddListener(OnAllCoresDestroyed);

        // 코어 Health 이벤트 등록
        foreach (var core in cores)
        {
            if (core != null)
            {
                var health = core.GetComponent<Health>();
                if (health != null)
                {
                    health.OnDie += () => RemoveCore(core);
                }
            }
        }
    }

    // 코어가 파괴되었을 때 리스트에서 제거 해주는 함수
    public void RemoveCore(GameObject core)
    {
        // 리스트에서 코어 제거
        var coreList = new List<GameObject>(cores);
        coreList.Remove(core);
        cores = coreList.ToArray();

        Debug.Log($"코어 제거됨: {core.name}");
        CheckCoresAndUpdateState(); // 코어 상태 갱신
    }

    // 만약 모든 코어가 파괴되었는지 확인하고 본체의 무적 해제 및 눈알 위치 조정
    public void CheckCoresAndUpdateState()
    {
        allCoresDestroyed = cores.Length > 0;
        foreach (var core in cores)
        {
            if (core != null)
            {
                allCoresDestroyed = false;
                break;
            }
        }

        if (allCoresDestroyed)
        {
            // 모든 코어가 파괴되었을 때 이벤트 호출
            onAllCoresDestroyed.Invoke();
        }
    }

    // 모든 코어가 파괴되었을 때 실행되는 함수
    private void OnAllCoresDestroyed()
    {
        // 본체 무적 해제
        GetComponent<Health>().isInvincible = false;
        Debug.Log("모든 코어가 파괴됨 - 본체 무적 해제");

        // 눈알 위치 조정 (y축으로 올리기)
        ChangeEyePositionUp();
    }

    public void ChangeEyePositionUp()
    {
        eye.SetActive(true);
        // 눈알 위치 조정 (y축으로 올리기)
        Vector3 newPosition = eye.transform.position;
        newPosition.y += eyePositionY; // y 위치를 eyePositionY 값만큼 올림 (필요에 따라 조정 가능)
        eye.transform.position = newPosition;
        Debug.Log("눈알 위치 올라감");
    }

    public void ChangeEyePositionDown()
    {
        eye.SetActive(false);
        Vector3 newPosition = eye.transform.position;
        newPosition.y -= eyePositionY; // y 위치를 eyePositionY 값만큼 내림 (필요에 따라 조정 가능)
        eye.transform.position = newPosition;
        Debug.Log("눈알 위치 낮아짐");

    }

    // 타겟과의 거리 계산
    public bool IsTargetInRange()
    {
        if (target == null) return false;
        return Vector3.Distance(transform.position, target.position) <= searchRange;
    }

    // 보스 공격 패턴 함수
    public void SpaceBossAttack()
    {
        if (remainingAttackPatterns.Count == 0)
        {
            // 모든 패턴이 사용되었을 때, 공격 패턴을 다시 초기화
            remainingAttackPatterns = new List<System.Action>(attackPatterns);

        }

        // 무작위로 공격 패턴 선택
        int randomIndex = Random.Range(0, remainingAttackPatterns.Count);
        System.Action selectedAttack = remainingAttackPatterns[randomIndex];
        remainingAttackPatterns.RemoveAt(randomIndex);

        // 선택 된 어택 신호 전달
        selectedAttack.Invoke();
    }

    // 보스 상태 변환 함수들
    public void SpaceBossIdle()
    {
        ChangeState<SpaceBossIdleState>();
    }
    public void SpaceBossDefenceState()
    {
        ChangeState<SpaceBossDefenceState>();
    }
    public void SpaceBossAttackState()
    {
        ChangeState<SpaceBossAttackState>();
    }
    public void SpaceBossCoreExplosionState()
    {
        ChangeState<SpaceBossCoreExplosionState>();
    }
    public void SpaceBossEyeLaserState()
    {
        ChangeState<SpaceBossEyeLaserState>();
    }
    public void SpaceBossEMPAttackState()
    {
        ChangeState<SpaceBossEMPAttackState>();
    }

    // 서치 범위 그리기
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRange);
    }  
}
