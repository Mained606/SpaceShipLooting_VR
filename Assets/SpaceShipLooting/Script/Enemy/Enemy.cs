using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    #region Variables
    public Transform target;
    private NavMeshAgent agent;

    [SerializeField] private float idleTime = 2f;
    private float timer;

    // 메테리얼
    public Material idleMaterial;
    public Material moveMaterial;
    public Material chaseMaterial;
    public Material attackMaterial;

    private Renderer renderer;

    EnemyState currentState;

    private bool isPlayerInStealthMode;

    private float distance;
    [SerializeField] private float minMoveDistance = 5f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float moveSpeed = 5f;
    

    // Perception Range
    [SerializeField] private float runPerceptionRange = 15f;
    [SerializeField] private float movePerceptionRange = 10f;
    [SerializeField] private float stealthPerceptionRange = 3f;

    // patrol
    public Transform spawnPoint;
    private Vector3 spawnPosition;
    public float patrolRange = 10f;
    private Vector3 nextMovePoint;

    // chase
    private bool isTargeting = false;
    private float chasingTime = 1f;
    private float chaseTimer;

    // attack
    private float attackDelay = 1f;
    private float attackTimer;
    #endregion

    private void Start()
    {
        PlayerStateManager.Instance.OnStealthStateChanged.AddListener(PlayerStealthCheck);

        // 참조
        agent = GetComponent<NavMeshAgent>();
        renderer = GetComponent<Renderer>();

        // 초기화
        currentState = EnemyState.E_Idle;
        nextMovePoint = spawnPoint.position;
        agent.speed = moveSpeed;
        spawnPosition = spawnPoint.position;
        timer = idleTime;
        attackTimer = attackDelay;
        chaseTimer = chasingTime;
    }

    private void Update()
    {
        distance = Vector3.Distance(transform.position, target.position);
        Vector3 dir = new Vector3(target.position.x - transform.position.x, 0f, target.position.z - transform.position.z);
        
        switch (currentState)
        {
            case EnemyState.E_Idle:
                renderer.material = idleMaterial;
                Debug.Log("current State: E_Idle");
                AISearching();
                break;

            case EnemyState.E_Move:
                renderer.material = moveMaterial;
                Debug.Log("current State: E_Move");
                AIMove(nextMovePoint);
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
                Debug.Log("current State: E_Chase");
                break;

            case EnemyState.E_Attack:
                renderer.material = attackMaterial;
                attackTimer -= Time.deltaTime;
                Attack();
                if (attackTimer < 0f)
                {
                    attackTimer = attackDelay;
                    SetState(EnemyState.E_Chase);
                }
                
                break;

            case EnemyState.E_Death:
                break;
        }
    }

    private void PlayerStealthCheck(bool isStealth)
    {
        // 플레이어 스텔스 모드 변경 여부 저장
        isPlayerInStealthMode = isStealth;
        Debug.Log("PlayerStealthCheck : " + isStealth);
    }

    private void SetState(EnemyState newState)
    {
        if (newState == currentState) return;

        currentState = newState;
    }

     private void AISearching()
    {
        if (currentState == EnemyState.E_Idle)
        {
            if (distance <= runPerceptionRange)
            {
                // 타겟 RUN 여부 파악할 필요 있음
                SetMovePoint();
                SetState(EnemyState.E_Move);
            }
            else if (distance <= movePerceptionRange)
            {
                SetState(EnemyState.E_Chase);
            }
            else
            {
                return;
            }
        }
        else if(currentState == EnemyState.E_Move)
        {
            if(distance <= movePerceptionRange)
            {
                targetStealthCheck();       // 스텔스 검사
            }
            else if(distance > runPerceptionRange)
            {
                SetState(EnemyState.E_Idle);
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

    private void AIMove(Vector3 destination)
    {   
        if(agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            timer -= Time.deltaTime;
            if (timer < 0f)
            {
                destination = SetMovePoint();
                agent.SetDestination(destination);
                timer = idleTime;
            }
        }
    }

    private Vector3 SetMovePoint()
    {
        nextMovePoint = spawnPosition + Random.insideUnitSphere * patrolRange;
        nextMovePoint.y = spawnPosition.y;

        //
        NavMeshHit hit;
        if(NavMesh.SamplePosition(nextMovePoint, out hit, patrolRange, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return spawnPosition;
    }

    private void ChasingTarget()
    {
        if(isTargeting == false)
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
        agent.enabled = false;
        Debug.Log("Attack");


    }

    private void OnDestroy()
    {
        PlayerStateManager.Instance.OnStealthStateChanged.RemoveListener(PlayerStealthCheck);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stealthPerceptionRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, movePerceptionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, runPerceptionRange);
    }
}
