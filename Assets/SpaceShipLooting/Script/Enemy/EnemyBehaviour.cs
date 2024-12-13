using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public EnemyData enemyData;

    private NavMeshAgent agent;
    private Health health;
    private Collider _collider;
    private Transform target;
    public Transform Target { get; }
    private Damageable targetDamageable;
    private Animator animator;
    private EnemyPatrol patrol;
    private SpawnType spawnType;

    public EnemyState currentState;
    private float attackTimer;
    private float chaseTimer;
    private float encounterTimer = 0f;

    private FanShapePerception fanPerception;
    //private bool isInTrigger = false;
    private bool isPlayerVisible = false;
    private Transform eyePoint;
    private Transform targetHead;
    private Vector3 directionToPlayer = Vector3.zero;
    public LayerMask obstacleLayer;

    private float distance;
    private bool isPlayerInStealthMode;
    private bool isPlayerRunning;

    private bool isEncounter = false;

    private bool isDeath = false;
    private bool hasItem = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        _collider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        patrol = GetComponent<EnemyPatrol>();
        target = GameObject.FindWithTag("Player").transform;
        if (target.GetComponent<Damageable>() != null)
        {
            targetDamageable = target.GetComponent<Damageable>();
        }
        targetHead = GameObject.FindWithTag("Player").transform.GetChild(1);
        fanPerception = GetComponentInChildren<FanShapePerception>();
        spawnType = GetComponent<EnemyPatrol>().spawnType;

        currentState = EnemyState.E_Idle;
        agent.speed = enemyData.moveSpeed;
        eyePoint = transform.GetChild(0);
        health.CurrentHealth = enemyData.health;

        if (enemyData.item != null)
        {
            hasItem = true;
        }
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
        switch (currentState)
        {
            case EnemyState.E_Idle:
                //CheckForTarget();
                SetState(EnemyState.E_Patrol);
                break;
            case EnemyState.E_Patrol:
                //patrol.Patrol();
                CheckForTarget();
                break;
            case EnemyState.E_Chase:
                ChaseTarget();
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

    public void SetState(EnemyState newState)
    {
        if (newState == currentState) return;

        currentState = newState;
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

    private void CheckForTarget()   // 타겟 감지, 서칭 함수
    {
        bool isInTrigger = fanPerception.IsInRange;

        if (distance <= enemyData.runPerceptionRange)
        {
            if (isPlayerRunning == true)   // Player Running Check
            {
                Debug.Log("Player Running Perception");
                SetState(EnemyState.E_Chase);
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
                    SetState(EnemyState.E_Chase);
                }
            }
            else  // Ray에 아무것도 감지 안 됨
            {
                isPlayerVisible = true;
                Debug.Log("플레이어가 보입니다");
                SetState(EnemyState.E_Chase);
            }
        }
        else if (isInTrigger == true && distance <= enemyData.stealthPerceptionRange)  // 스텔스 인지 범위까지 가까이 도달
        {
            SetState(EnemyState.E_Chase);
        }
    }

    private void FirstEncounter()
    {
        encounterTimer += Time.deltaTime;
        enemyData.targetEncounterUI.SetActive(true);
        animator.SetBool("IsPatrol", false);
        agent.enabled = false;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
        if(encounterTimer >= enemyData.chaseInterval)
        {
            isEncounter = true;
            enemyData.targetEncounterUI.SetActive(false);
            return;
        }
    }

    private void ChaseTarget()  // ChaseTime동안 타겟 추격
    {
        if (!isEncounter)
        {
            FirstEncounter();
        }
        else
        {
            if (distance <= enemyData.attackRange)
            {
                Debug.Log("플레이어 잡힙(즉사)");
                targetDamageable.InflictDamage(100f);
            }
            chaseTimer += Time.deltaTime;
            animator.SetBool("IsAttack", false);
            animator.SetBool("IsChase", true);
            agent.enabled = true;
            agent.speed = enemyData.moveSpeed;
            agent.SetDestination(target.position);
            if (chaseTimer >= enemyData.chaseInterval)
            {
                chaseTimer = 0f;
                animator.SetBool("IsChase", false);
                SetState(EnemyState.E_Attack);
            }
        }
       
    }

    public void BusterCall()
    {
        isEncounter = true;
        animator.SetBool("IsChase", true);
        agent.SetDestination(target.position);
        if (distance <= 10f)    // 수치 변수로 바꿀 필요 있음
        {
            SetState(EnemyState.E_Chase);
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
            SetState(EnemyState.E_Chase);
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

        SetState(EnemyState.E_Death);
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
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, enemyData.stealthPerceptionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemyData.runPerceptionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyData.attackRange);

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
    }
}
