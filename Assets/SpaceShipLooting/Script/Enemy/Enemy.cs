using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.OpenXR.Input;

public class Enemy : MonoBehaviour
{
    #region Variables
    private Transform target;
    private NavMeshAgent agent;
    private Collider _collider;
    public GameObject item;
    private Pattern patrolPattern;
    private Health health;
    private Damageable targetDamageable;
    private Destructable destructable;
    private Animator animator;
    private SpawnType spawnType;

    public EnemyState currentState;

    private bool isPlayerInStealthMode;
    private bool isPlayerRunnning;

    [SerializeField] private float moveSpeed = 5f;

    // Perception
    private float distance;
    [SerializeField] private float runPerceptionRange = 15f;
    [SerializeField] private float stealthPerceptionRange = 3f;

    private FanShapePerception fanPerception;
    private bool isInTrigger = false;
    private bool isPlayerVisible = false;
    private Transform eyePoint;
    private Transform targetHead;
    private Vector3 directionToPlayer = Vector3.zero;
    public LayerMask obstacleLayer;

    // patrol (move)

    // chase
    private bool isTargeting = false;
    private float chasingTime = 1f;
    private float chaseTimer;

    // attack
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackTime = 3f;
    private BasicTimer AttackTimer;
    [SerializeField] private float attackDamage = 10f;

    // death
    private bool isDeath = false;
    private bool hasItem = false;

    #endregion


    private void Start()
    {
        PlayerStateManager.Instance.OnStealthStateChanged.AddListener(PlayerStealthCheck);
        if(GetComponentInParent<WayPointParent>() != null)
        {
            //Debug.Log("GetComponentInParent WayPointParent");
            patrolPattern = new WayPointParent();
        }
        else if (GetComponentInParent<RandomParent>() != null)
        {
            //Debug.Log("GetComponentInParent RandomParent");
            patrolPattern = new RandomParent();
        }
        else if (GetComponentInParent<NoneParent>() != null)
        {
            //Debug.Log("GetComponentInParent NoneParent");
            patrolPattern = new NoneParent();
        }
        
        // 참조
        target = GameObject.FindWithTag("Player").transform;
        targetHead = GameObject.FindWithTag("Player").transform.GetChild(1);
        agent = GetComponent<NavMeshAgent>();
        _collider = GetComponent<Collider>();

        health = GetComponent<Health>();
        if(target.GetComponent<Damageable>() != null)
        {
            targetDamageable = target.GetComponent<Damageable>();
        }
        destructable = GetComponent<Destructable>();
        fanPerception = GetComponentInChildren<FanShapePerception>();


        animator = GetComponent<Animator>();
        eyePoint = transform.GetChild(0);

        // 초기화
        currentState = EnemyState.E_Idle;
        agent.speed = moveSpeed;
        chaseTimer = chasingTime;
        isInTrigger = fanPerception.IsInRange;
        

        AttackTimer = new BasicTimer(attackTime);

        if (item != null)
        {
            hasItem = true;
        }
    }

    private void Update()
    {
        if (isDeath)
            return;

        if(health.CurrentHealth <= 0)
        {
            Die();
        }

        isInTrigger = fanPerception.IsInRange;
        distance = Vector2.Distance(new Vector2(eyePoint.position.x, eyePoint.position.z), new Vector2(targetHead.position.x, targetHead.position.z));
        directionToPlayer = (targetHead.position - eyePoint.position).normalized;
        
        switch (currentState)
        {
            case EnemyState.E_Idle:
                AISearching();
                break;

            case EnemyState.E_Patrol:
                animator.SetBool("IsPatrol", true);
                AISearching();
                break;

            case EnemyState.E_Chase:
;
                animator.SetBool("IsPatrol", false);
                if (isTargeting == false)
                {
                    animator.SetBool("IsChase", false);
                    agent.enabled = false;
                    chaseTimer -= Time.deltaTime;

                    if (chaseTimer <= 0f)
                    {
                        Debug.Log($"{this.gameObject.name} says 타겟 발견!");
                        isTargeting = true;
                    }
                }
                else
                {
                    if(animator.GetBool("IsChase") == false)
                    {
                        animator.SetBool("IsChase", true);
                    }

                    ChasingTarget();
                }
                break;

            case EnemyState.E_Attack:
                agent.enabled = false;
                if (AttackTimer.IsRunning)   // AttackTimer 가동 중이면 Attack()
                {
                    animator.SetBool("IsAttack", true);
                    return;
                }
                else                         // AttackTimer 가동 중 아니면 새 AttackTimer 생성, 스타트
                {
                    TimerManager.Instance.StartTimer(AttackTimer);
                    animator.SetBool("IsAttack", false);
                    SetState(EnemyState.E_Chase);
                }

                break;

            case EnemyState.E_Death:
                agent.enabled = false;
                animator.SetBool("IsDeath", true);
                break;
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
        isPlayerRunnning = isRunning;
        Debug.Log("Player Running");
    }

    private void SetState(EnemyState newState)
    {
        if (newState == currentState) return;

        currentState = newState;
    }

     public void AISearching()
    {

        if (currentState == EnemyState.E_Idle && spawnType != SpawnType.Normal)
        {
            SetState(EnemyState.E_Patrol);
        }
        else if(currentState == EnemyState.E_Patrol)
        {
            if (distance <= runPerceptionRange)
            {
                if (isPlayerRunnning == true)   // Player Running Check
                {
                    Debug.Log("Player Running Perception");
                    SetState(EnemyState.E_Chase);
                }
            }

            if (isInTrigger == true && distance > stealthPerceptionRange)   // 움직임 감지 범위 안엔 있지만 장애물 뒤에 숨은 경우
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
            else if(isInTrigger == true && distance <= stealthPerceptionRange)  // 스텔스 인지 범위까지 가까이 도달
            {
                SetState(EnemyState.E_Chase);
            }

        }
        
    }

    private void ChasingTarget()
    {
        if (distance <= attackRange)
        {
            Debug.Log("플레이어 잡힙(즉사처리 예정)");
        }
        chaseTimer -= Time.deltaTime;
        agent.enabled = true;
        agent.speed = moveSpeed;
        agent.SetDestination(target.position);
        if (chaseTimer <= 0f)
        {
            chaseTimer = chasingTime;
            animator.SetBool("IsChase", false);
            SetState(EnemyState.E_Attack);
        }
    }

    private void Attack()
    {
        targetDamageable.InflictDamage(attackDamage);
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
        Destroy(gameObject, 2f);
    }

    private void DropItem()
    {
        Instantiate(item, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
        JsonTextManager.instance.OnDialogue("stage2-8");
        Debug.Log("Item dropped!");
    }

    private void OnDestroy()
    {
        PlayerStateManager.Instance.OnStealthStateChanged.RemoveListener(PlayerStealthCheck);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stealthPerceptionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, runPerceptionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (isPlayerVisible)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        if(target != null && eyePoint != null)
        {
            Gizmos.DrawLine(eyePoint.position, targetHead.position);
        }
    }
}
