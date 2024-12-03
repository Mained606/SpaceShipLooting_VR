using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    #region Variables
    private Transform target;
    private NavMeshAgent agent;
    private Collider _collider;
    public GameObject item;
    private Pattern patrolPattern;
    private Health health;
    

    // 메테리얼
    public Material idleMaterial;
    public Material chaseMaterial;
    public Material attackMaterial;

    private Renderer renderer;

    public EnemyState currentState;

    private bool isPlayerInStealthMode;
    [SerializeField] private bool isPlayerRunnning = true;

    private float distance;
    //[SerializeField] private float minMoveDistance = 5f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float moveSpeed = 5f;
    

    // Perception
    [SerializeField] private float runPerceptionRange = 15f;
    [SerializeField] private float movePerceptionRange = 10f;
    [SerializeField] private float stealthPerceptionRange = 3f;
    private FanShapePerception fanPerception;
    [SerializeField] private bool isInTrigger = false;
    [SerializeField] private bool isPlayerVisible = false;
    public Transform eyePoint;
    private Transform targetHead;
    private Vector3 directionToPlayer = Vector3.zero;
    public LayerMask obstacleLayer;

    // patrol (move)
    EnemyPatrol enemyPatrol;

    // chase
    private bool isTargeting = false;
    [SerializeField] private float chasingTime = 1f;
    private float chaseTimer;

    // attack
    [SerializeField] private float attackTime = 2f;
    [SerializeField] private float attackDelayTime = 0.5f;
    private BasicTimer AttackTimer;
    private BasicTimer AttackDelayTimer;

    // death
    [SerializeField] private bool isDeath = false;
    [SerializeField] private bool hasItem = false;

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
        agent = GetComponent<NavMeshAgent>();
        _collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
        enemyPatrol = GetComponent<EnemyPatrol>();
        health = GetComponent<Health>();
        fanPerception = GetComponentInChildren<FanShapePerception>();
        target = GameObject.FindWithTag("Player").transform;
        targetHead = GameObject.FindWithTag("Player").transform.GetChild(1);

        // 초기화
        currentState = EnemyState.E_Idle;
        agent.speed = moveSpeed;
        chaseTimer = chasingTime;
        isInTrigger = fanPerception.IsInRange;

        AttackTimer = new BasicTimer(attackTime);
        AttackDelayTimer = new BasicTimer(attackDelayTime);

        if (item != null)
        {
            hasItem = true;
        }
    }

    private void Update()
    {
        if (isDeath)
            return;

        isInTrigger = fanPerception.IsInRange;
        distance = Vector3.Distance(eyePoint.position, targetHead.position);
        directionToPlayer = (targetHead.position - eyePoint.position).normalized;
        
        switch (currentState)
        {
            case EnemyState.E_Idle:
                renderer.material = idleMaterial;
                //Debug.Log("current State: E_Idle");
                AISearching();
                break;

            case EnemyState.E_Move:
                //Debug.Log("current State: E_Move");
                AISearching();
                break;

            case EnemyState.E_Chase:
                renderer.material = chaseMaterial;
                agent.enabled = true;
                ChasingTarget();
                if(distance <= attackRange)
                {
                    SetState(EnemyState.E_Attack);
                }
                //Debug.Log("current State: E_Chase");
                break;

            case EnemyState.E_Attack:
                renderer.material = attackMaterial;
                agent.enabled = false;
                if (AttackTimer.IsRunning)  // AttackTimer 가동 중이면 Attack()
                {
                    Attack();
                }
                else                         // AttackTimer 가동 중 아니면 새 AttackTimer 생성, 스타트
                {
                    TimerManager.Instance.StartTimer(AttackTimer);
                    SetState(EnemyState.E_Chase);
                }

                break;

            case EnemyState.E_Death:
                agent.enabled = false;
                // Animation
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
        if (currentState == EnemyState.E_Idle)
        {
            SetState(EnemyState.E_Move);
        }
        else if(currentState == EnemyState.E_Move)
        {
            //if (distance <= runPerceptionRange && distance > movePerceptionRange)
            //{
            //    if (isPlayerRunnning == true)   // Player Running Check
            //    {
            //        Debug.Log("Player Running Perception");
            //        SetState(EnemyState.E_Chase);
            //    }
            //}
            if (isInTrigger == true)
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
            if (distance <= movePerceptionRange)
            {
                targetStealthCheck();       // 스텔스 검사
            }
            else
            {
                return;
            }
        }
        
    }

    private void targetStealthCheck()
    {
        // AI 행동 변경 로직 추가
        if (isPlayerInStealthMode)
        {
            if(distance <= stealthPerceptionRange)
            {
                SetState(EnemyState.E_Chase);
            }
            else
            {
                SetState(EnemyState.E_Move);
            }
            
        }
        else
        {
            SetState(EnemyState.E_Chase);
        }
    }

   

    private void ChasingTarget()
    {
        agent.speed = moveSpeed;
        if (isTargeting == false)
        {
            isTargeting = true;
            Debug.Log($"{this.gameObject.name} says 타겟 발견!");
            SetState(EnemyState.E_Attack);
        }
        else
        {
            chaseTimer -= Time.deltaTime;
            agent.SetDestination(target.position);
            if (chaseTimer < 0f)
            {
                chaseTimer = chasingTime;
                SetState(EnemyState.E_Attack);
            }
        }

    }

    private void Attack()
    {
        if (AttackDelayTimer.IsRunning)
        {
            Debug.Log("대기");
        }
        else
        {
            //TakeDamage();
            Debug.Log("TakeDamage");
            TimerManager.Instance.StartTimer(AttackDelayTimer);
        }

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
        Destroy(gameObject, 1f);
    }

    private void DropItem()
    {
        Instantiate(item, transform.position, Quaternion.identity);
        Debug.Log("Item dropped!");
    }

    private void OnDestroy()
    {
        PlayerStateManager.Instance.OnStealthStateChanged.RemoveListener(PlayerStealthCheck);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stealthPerceptionRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, movePerceptionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, runPerceptionRange);

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
