using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public EnemyData enemyData;
    private EnemyPatrol patrolBehavior;
    private EnemyChase chaseBehaviour;
    private EnemyInteract interactBehavior;
    [SerializeField] private float assassiableDist = 3f;

    private NavMeshAgent agent;
    private Health health;
    private float maxHealth;
    private Collider _collider;
    public Transform target;

    [HideInInspector] public Vector3 spawnPosition;
    private Transform[] wayPoints;

    private float lookAroundTimer = 0f;

    int currentCount = 0;
    int falseCount = 0;

    [HideInInspector] public Transform Target { get; }
    [HideInInspector] public Damageable targetDamageable;
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpawnType spawnType;

    private bool isInfinite;

    [HideInInspector] public EnemyState currentState;
    private float attackTimer;

    private FanShapePerception fanPerception;
    private bool isPlayerVisible = false;
    private Transform eyePoint;
    private Transform targetHead;
    private Vector3 directionToPlayer = Vector3.zero;
    public Vector3 DirectionToPlayer { get; }
    public LayerMask obstacleLayer;

    private float distance;
    private bool isPlayerInStealthMode;
    private bool isPlayerRunning;

    private bool isInTrigger = false;
    public bool isAssassiable = false;

    private bool isDeath = false;
    private bool hasItem = false;

    private void Start()
    {
        PlayerStateManager.Instance.OnStealthStateChanged.AddListener(PlayerStealthCheck);
        PlayerStateManager.Instance.OnRunningStateChanged.AddListener(PlayerRunningCheck);
        Floor1Console.consoleFalse.AddListener(ConsoleFalse);
        GasOpen.GasGasGas.AddListener(GasEventOn);


        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        _collider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        target = PlayerStateManager.PlayerTransform;
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
        SetInteractBehavior();
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
        SetAssassiable();

        switch (enemyData.currentState)
        {
            case EnemyState.E_Idle:
                enemyData.SetState(EnemyState.E_Patrol);
                break;
            case EnemyState.E_Patrol:
                CheckForTarget();
                PreemptiveStrike();
                if (enemyData.isInteracting)
                {
                    enemyData.isLookAround = false;
                    interactBehavior.Interacting(agent);
                }
                else
                {
                    if (enemyData.isLookAround)
                    {
                        LookAround();
                    }
                    else
                    {
                        agent.enabled = true;
                        animator.SetBool("IsLookAround", false);
                        enemyData.isLookAround = patrolBehavior.Patrol(agent);
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
                CheckForTargetOnChase();
                if (isPlayerVisible)
                {
                    animator.SetBool("IsAttack", true);
                    AttackingWait();
                }
                else
                {
                    enemyData.SetState(EnemyState.E_Chase);
                }
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

    private void SetInteractBehavior()
    {
        switch (enemyData.enemyInteractData.interactType)
        {
            case InteractType.PipeExplosion:
                interactBehavior = new PipeExplosion();
                interactBehavior.Initialize(this);
                break;
            case InteractType.Dispatch:
                interactBehavior = new Dispatch();
                interactBehavior.Initialize(this);
                break;
            case InteractType.BusterCall:
                interactBehavior = new BusterCall();
                interactBehavior.Initialize(this);
                break;
            case InteractType.None:
                interactBehavior = new None();
                interactBehavior.Initialize(this);
                break;
        }
    }

    private void SetAssassiable()
    {
        if(distance <= assassiableDist && enemyData.currentState == EnemyState.E_Patrol)
        {
            isAssassiable = true;
        }
        else
        {
            isAssassiable = false;
        }
    }

    private void GasEventOn(bool isEventOn)
    {
        if(enemyData.enemyInteractData.interactType == InteractType.PipeExplosion)
        {
            enemyData.isInteracting = isEventOn;
        }
    }

    private void ConsoleFalse(bool isEventOn)
    {
        if (falseCount == 0)
        {
            falseCount++;
            if (enemyData.enemyInteractData.interactType == InteractType.Dispatch)
            {
                enemyData.isInteracting = !isEventOn;
            }
        }
        else if (falseCount == 1)
        {
            falseCount = 0;
            if(enemyData.enemyInteractData.interactType == InteractType.BusterCall)
            {
                enemyData.isInteracting = !isEventOn;
            }
        }

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
    }

    void LookAround()
    {
        if (isInfinite)
        {
            agent.enabled = true;
            animator.SetBool("IsLookAround", false);
            agent.speed = enemyData.patrolSpeed;
            enemyData.isLookAround = false;
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
                agent.speed = enemyData.patrolSpeed;
                enemyData.isLookAround = false;
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
        isInTrigger = fanPerception.IsInRange;

        if (distance <= enemyData.runPerceptionRange)
        {
            if (isPlayerRunning == true)   // Player Running Check
            {
                enemyData.SetState(EnemyState.E_Chase);
            }
        }

        if (isInTrigger == true && distance > enemyData.stealthPerceptionRange)   // 움직임 범위 안에 도달
        {
            Ray ray = new Ray(eyePoint.position, directionToPlayer);
            if (Physics.Raycast(ray, out RaycastHit hit, distance, obstacleLayer))
            {
                int hitLayer = hit.collider.gameObject.layer;   // Obstacle

                if (hitLayer == 11)                     // 움직임 감지 범위 안엔 있지만 장애물 뒤에 숨은 경우
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

    private void CheckForTargetOnChase()
    {
        Ray ray = new Ray(eyePoint.position, directionToPlayer);
        if (Physics.Raycast(ray, out RaycastHit hit, distance, obstacleLayer))
        {
            int hitLayer = hit.collider.gameObject.layer;   // Obstacle

            if (hitLayer == 11)                     // 움직임 감지 범위 안엔 있지만 장애물 뒤에 숨은 경우
            {
                isPlayerVisible = false;
                Debug.Log("총을 쏠 수 없음");
                enemyData.SetState(EnemyState.E_Chase);

            }
            else
            {
                isPlayerVisible = true;
                Debug.Log("총을 쏠 수 있음");
                enemyData.SetState(EnemyState.E_Attack);
            }
        }
        else  // Ray에 아무것도 감지 안 됨
        {
            isPlayerVisible = true;
            Debug.Log("플레이어가 보입니다");
            enemyData.SetState(EnemyState.E_Attack);
        }
    }

    private void PreemptiveStrike()
    {
        if(health.CurrentHealth < maxHealth && isAssassiable == false)
        {
            enemyData.SetState(EnemyState.E_Chase);
        }
    }

    public void BusterCall()
    {
        chaseBehaviour.isEncounter = true;
        animator.SetBool("IsChase", true);
        agent.SetDestination(target.position);
        enemyData.SoundPlay(enemyData.EnemyWalk);   // chase 사운드 필요
        CheckForTargetOnChase();
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
        enemyData.SoundPlay(enemyData.EnemyShot);
        targetDamageable.InflictDamage(enemyData.attackDamage);
    }

    private void Die()
    {
        if (isDeath)
            return;

        enemyData.SetState(EnemyState.E_Death);
        isDeath = true;
        _collider.enabled = false;
        enemyData.SoundPlay(enemyData.EnemyKill);
        if (hasItem)
        {
            DropItem();
        }
        Destroy(gameObject, 3f);
    }

    private void DropItem()
    {
        Instantiate(enemyData.item, agent.transform.position, Quaternion.identity);
        if (enemyData.item.ToString() == "Card_Key")
        {
            enemyData.SoundPlay(enemyData.KeyCard);
        }
        Debug.Log("Item dropped!");
    }

    private void OnDestroy()
    {
        PlayerStateManager.Instance.OnStealthStateChanged.RemoveListener(PlayerStealthCheck);
        PlayerStateManager.Instance.OnStealthStateChanged.RemoveListener(PlayerRunningCheck);
        Floor1Console.consoleFalse.RemoveListener(ConsoleFalse);
        GasOpen.GasGasGas.RemoveListener(GasEventOn);
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
