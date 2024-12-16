using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public EnemyData enemyData;
    private EnemyPatrol patrolBehavior;
    private EnemyChase chaseBehaviour;

    private NavMeshAgent agent;
    private Health health;
    private float maxHealth;
    private Collider _collider;
    [HideInInspector] public Transform target;

    [HideInInspector] public Vector3 spawnPosition;
    private Transform[] wayPoints;

    private Vector3 dir;

    private float lookAroundTimer = 0f;
    [SerializeField] private bool isLookAround = false;
    public bool IsLookAround { get; set; }

    [SerializeField] private float patrolSpeed = 1f;
    private Vector3 destination;
    public Vector3 Destination { get; set; }

    [SerializeField] private bool isInterActEvent = false;
    [SerializeField] private InterActEventData interActEventData;

    int currentCount = 0;

    [HideInInspector] public Transform Target { get; }
    [HideInInspector] public Damageable targetDamageable;
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpawnType spawnType;

    private bool isInfinite;

    [HideInInspector] public EnemyState currentState;
    private float attackTimer;
    private float chaseTimer;
    private float encounterTimer = 0f;

    private FanShapePerception fanPerception;
    //private bool isInTrigger = false;
    private bool isPlayerVisible = false;
    private Transform eyePoint;
    private Transform targetHead;
    private Vector3 directionToPlayer = Vector3.zero;
    public Vector3 DirectionToPlayer { get; }
    public LayerMask obstacleLayer;

    private float distance;
    private bool isPlayerInStealthMode;
    private bool isPlayerRunning;

    private bool isEncounter = false;
    public bool IsEncounter { get; set; } 
    public bool isAssassiable = true;

    private bool isDeath = false;
    private bool hasItem = false;



    private void Start()
    {
        PlayerStateManager.Instance.OnStealthStateChanged.AddListener(PlayerStealthCheck);
        GasOpen.GasGasGas.AddListener(EventOn);

        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        _collider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
        if (target.GetComponent<Damageable>() != null)
        {
            targetDamageable = target.GetComponent<Damageable>();
        }
        targetHead = GameObject.FindWithTag("Player").transform.GetChild(1);
        fanPerception = GetComponentInChildren<FanShapePerception>();

        enemyData.currentState = EnemyState.E_Idle;
        agent.speed = enemyData.moveSpeed;
        eyePoint = transform.GetChild(0);
        health.CurrentHealth = enemyData.health;
        maxHealth = health.CurrentHealth;
        spawnPosition = transform.position;
        isInfinite = enemyData.infinitePatrolMode;

        if (enemyData.item != null)
        {
            hasItem = true;
        }

        SetPatrolBehavior();
        SetChaseBehavior();
    }

    private void Update()
    {
        if (isDeath)
            return;

        directionToPlayer = (targetHead.position - eyePoint.position).normalized;

        if (health.CurrentHealth <= 0)
        {
            Die();
        }

        distance = Vector2.Distance(new Vector2(eyePoint.position.x, eyePoint.position.z), new Vector2(targetHead.position.x, targetHead.position.z));
        switch (enemyData.currentState)
        {
            case EnemyState.E_Idle:
                enemyData.SetState(EnemyState.E_Patrol);
                break;
            case EnemyState.E_Patrol:
                CheckForTarget();
                PreemptiveStrike();
                if (isInterActEvent)
                {
                    InvastigateTarget(interActEventData);
                }
                else
                {
                    if (isLookAround)
                    {
                        LookAround();
                    }
                    else
                    {
                        agent.enabled = true;
                        animator.SetBool("IsLookAround", false);
                        isLookAround = patrolBehavior.Patrol(agent);
                    }
                }
                break;
            case EnemyState.E_Chase:
                isAssassiable = false;
                chaseBehaviour.Chase(agent);
                break;
            case EnemyState.E_BusterCall:
                BusterCall();
                break;
            case EnemyState.E_Attack:
                animator.SetBool("IsAttack", true);
                AttackingWait();
                break;
            case EnemyState.E_Death:
                agent.enabled = false;
                animator.SetBool("IsDeath", true);
                break;
                
        }
    }

    private void SetPatrolBehavior()
    {
        switch (spawnType)
        {
            case SpawnType.RandomPatrol:
                patrolBehavior = new RandomPatrol();
                patrolBehavior.Initialize(this);
                break;
            case SpawnType.WayPointPatrol:
                patrolBehavior = new WayPointPatrol();
                patrolBehavior.Initialize(this);
                break;
            case SpawnType.Normal:
                patrolBehavior = new NonePatrol();
                patrolBehavior.Initialize(this);
                break;
        }
    }

    private void SetChaseBehavior()
    {
        switch (enemyData.enemyChaseType)
        {
            case ChaseType.Aggressive:
                chaseBehaviour = new AggressiveChase();
                chaseBehaviour.Initialize(this);
                break;
            case ChaseType.Defensive:
                //chaseBehaviour = new DefensiveChase();
                chaseBehaviour.Initialize(this);
                break;
            case ChaseType.Runaway:
                chaseBehaviour = new Runaway();
                chaseBehaviour.Initialize(this);
                break;
        }
    }

    private void EventOn(bool isEventOn)
    {
        isInterActEvent = isEventOn;
    }

    private void PlayerStealthCheck(bool isStealth)
    {
        // 플레이어 스텔스 모드 변경 여부 저장
        isPlayerInStealthMode = isStealth;
        Debug.Log("PlayerStealthCheck : " + isStealth);
    }

    private void PlayerRunningCheck(bool isRunning)
    {
        isPlayerRunning = isRunning;
        Debug.Log("Player Running");
    }

    Vector3 preClossTarget = Vector3.zero;
    private void InvastigateTarget(InterActEventData interActEventData)
    {
        // 1. 예외 처리: 유효하지 않은 데이터
        if (interActEventData.interActPosition == null || interActEventData.interActPosition.Count == 0)
        {
            if (interActEventData.interActType == InterActType.BusterCall)
            {
                agent.enabled = true;
                enemyData.SetState(EnemyState.E_BusterCall);
                return;
            }
            Debug.LogWarning("interActEventData가 없거나 포지션 리스트가 없습니다.");
            return;
        }

        // 2. 에이전트 활성화
        if (!agent.enabled) agent.enabled = true;

        // 3. 기존 목표지점 도달 여부 확인
        if (preClossTarget != Vector3.zero)
        {
            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                // 목표에 도달한 경우
                Debug.Log("이벤트목표 도착");
                animator.SetBool("IsLookAround", true);
                agent.enabled = false;
                preClossTarget = Vector3.zero;
                if (interActEventData.interActType == InterActType.Dispatch)
                {
                    isInterActEvent = false;
                    isLookAround = true;
                }
            }
            else
            {
                // 목표지점으로 계속 이동
                agent.enabled = true;
                isLookAround = false;
                animator.SetBool("IsLookAround", false);
                animator.SetBool("IsPatrol", true);
                agent.SetDestination(preClossTarget);
                return;
            }
        }
        else
        {
            // 4. 가장 가까운 위치 계산 및 설정
            Vector3 closestPosition = Vector3.zero;
            float closestDistance = float.MaxValue;

            foreach (Transform position in interActEventData.interActPosition)
            {
                float distance = Vector3.Distance(agent.transform.position, position.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPosition = position.position;
                }
            }

            Debug.LogWarning("이벤트 근거리 : " + closestPosition);

            // 5. 새로운 목표지점 설정
            preClossTarget = closestPosition;
            agent.SetDestination(closestPosition);
        }
    }

    public void SetSpawnType(SpawnType spawnerType, Transform[] gob)
    {
        spawnType = spawnerType;
        if (gob != null && gob.Length > 0)
        {
            wayPoints = gob;
        }
        else
        {
            //Debug.Log("no wayPoints");
        }
        Debug.Log($"SpawnType : {spawnType}");
    }

    void LookAround()
    {
        if (isInfinite)
        {
            agent.enabled = true;
            animator.SetBool("IsLookAround", false);
            agent.speed = patrolSpeed;
            isLookAround = false;
            if (spawnType == SpawnType.WayPointPatrol)
            {
                agent.SetDestination(wayPoints[currentCount].position);
                currentCount++;
                if (currentCount >= wayPoints.Length)
                {
                    currentCount = 0;
                }
            }
        }
        else
        {
            lookAroundTimer += Time.deltaTime;
            agent.enabled = false;
            animator.SetBool("IsLookAround", true);
            if (lookAroundTimer > 4f)
            {
                lookAroundTimer = 0f;
                agent.enabled = true;
                animator.SetBool("IsLookAround", false);
                agent.speed = patrolSpeed;
                isLookAround = false;
                if (spawnType == SpawnType.WayPointPatrol)
                {
                    agent.SetDestination(wayPoints[currentCount].position);
                    currentCount++;
                    if (currentCount >= wayPoints.Length)
                    {
                        currentCount = 0;
                    }
                }
            }
        }
    }

    private void CheckForTarget()   // 타겟 감지, 서칭 함수
    {
        bool isInTrigger = fanPerception.IsInRange;

        if (distance <= enemyData.runPerceptionRange)
        {
            if (isPlayerRunning == true)   // Player Running Check
            {
                Debug.Log("Player Running Perception");
                enemyData.SetState(EnemyState.E_Chase);
            }
        }

        if (isInTrigger == true && distance > enemyData.stealthPerceptionRange)   // 움직임 감지 범위 안엔 있지만 장애물 뒤에 숨은 경우
        {
            Ray ray = new Ray(eyePoint.position, directionToPlayer);
            if (Physics.Raycast(ray, out RaycastHit hit, distance, obstacleLayer))
            {
                int hitLayer = hit.collider.gameObject.layer;   // Obstacle

                Debug.Log("레이가 맞은 오브젝트의 레이어: " + hitLayer);
                if (hitLayer == 11)
                {
                    isPlayerVisible = false;
                    Debug.Log("플레이어가 장애물 뒤에 있습니다.");

                }
                else
                {
                    isPlayerVisible = true;
                    Debug.Log("플레이어가 보입니다");
                    enemyData.SetState(EnemyState.E_Chase);
                }
            }
            else  // Ray에 아무것도 감지 안 됨
            {
                isPlayerVisible = true;
                Debug.Log("플레이어가 보입니다");
                enemyData.SetState(EnemyState.E_Chase);
            }
        }
        else if (isInTrigger == true && distance <= enemyData.stealthPerceptionRange)  // 스텔스 인지 범위까지 가까이 도달
        {
            enemyData.SetState(EnemyState.E_Chase);
        }
    }

    private void PreemptiveStrike()
    {
        if(health.CurrentHealth < maxHealth)
        {
            enemyData.SetState(EnemyState.E_Chase);
        }
    }

    public void BusterCall()
    {
        chaseBehaviour.isEncounter = true;
        animator.SetBool("IsChase", true);
        agent.SetDestination(target.position);
        if (distance <= 10f)    // 수치 변수로 바꿀 필요 있음
        {
            enemyData.SetState(EnemyState.E_Chase);
        }
    }

    private void AttackingWait()    // AttackTime 기다리는 용도
    {
        attackTimer += Time.deltaTime;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
        agent.enabled = false;
        if(attackTimer >= enemyData.attackInterval)
        {
            attackTimer = 0f;
            enemyData.SetState(EnemyState.E_Chase);
        }
    }

    private void Attack()   // 애니메이션 프레임에 적용하는 함수
    {
        targetDamageable.InflictDamage(enemyData.attackDamage);
    }

    private void Die()
    {
        if (isDeath)
            return;

        enemyData.SetState(EnemyState.E_Death);
        isDeath = true;
        _collider.enabled = false;
        if (hasItem)
        {
            DropItem();
        }
        Destroy(gameObject, 3f);
    }

    private void DropItem()
    {
        Instantiate(enemyData.item, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
        Debug.Log("Item dropped!");
    }

    private void OnDestroy()
    {
        PlayerStateManager.Instance.OnStealthStateChanged.RemoveListener(PlayerStealthCheck);
        GasOpen.GasGasGas.RemoveListener(EventOn);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, enemyData.stealthPerceptionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemyData.runPerceptionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyData.deadZone);

        if (isPlayerVisible)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        if (target != null && eyePoint != null)
        {
            Gizmos.DrawLine(eyePoint.position, targetHead.position);
        }

        if (spawnType == SpawnType.RandomPatrol)
        {
            Gizmos.color = Color.green;

            if (enemyData.enemyPatrolType == PatrolType.Circle)
            {
                Gizmos.DrawWireSphere(spawnPosition, enemyData.circlePatrolRange);
            }
            else if (enemyData.enemyPatrolType == PatrolType.Rectangle)
            {
                Vector3 size = new Vector3(enemyData.rectanglePatrolRange.x, 0f, enemyData.rectanglePatrolRange.y);
                Gizmos.DrawWireCube(spawnPosition, size);
            }
        }
    }
}
