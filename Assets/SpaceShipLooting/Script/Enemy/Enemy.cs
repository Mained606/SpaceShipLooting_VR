using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    #region Variables
    public Transform target;
    private NavMeshAgent agent;

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

    private Vector3 movingPosition;

    //
    public Transform spawnPoint;
    private bool isInPatrolRange;
    public float patrolRange = 10f;
    private Vector3 nextMovePoint;
    #endregion

    private void Start()
    {
        nextMovePoint = spawnPoint.position;
        currentState = EnemyState.E_Idle;
        agent = GetComponent<NavMeshAgent>();
        PlayerStateManager.Instance.OnStealthStateChanged.AddListener(PlayerStealthCheck);
        agent.speed = moveSpeed;
        renderer = GetComponent<Renderer>();
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
                ChasingTarget();
                if(distance <= attackRange)
                {
                    SetState(EnemyState.E_Attack);
                }
                Debug.Log("current State: E_Chase");
                break;

            case EnemyState.E_Attack:
                renderer.material = attackMaterial;
                Attack();
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
        float remainingDistance = Vector3.Distance(transform.position, destination);
        //Debug.Log(remainingDistance);
        if(remainingDistance <= 1.2f)
        {
            // 도착
            Debug.Log("도착");
            SetState(EnemyState.E_Idle);
        }
        else
        {
            Debug.Log("이동");
            Debug.Log(destination);
            agent.SetDestination(destination);
        }
    }

    private Vector3 SetMovePoint()
    {
        Vector2 randomXZ = new Vector2(Random.Range(-patrolRange, patrolRange + 1), Random.Range(-patrolRange, patrolRange + 1));
        nextMovePoint = new Vector3(randomXZ.x, 0f, randomXZ.y);
        float moveDistance = Vector3.Distance(this.transform.position, nextMovePoint);
        Debug.Log(Vector3.Distance(spawnPoint.position, nextMovePoint));
        isInPatrolRange = Vector3.Distance(spawnPoint.position, nextMovePoint) < patrolRange;
        if(moveDistance <= minMoveDistance || !isInPatrolRange)
        {
            SetMovePoint();
        }
        movingPosition = nextMovePoint;
        return nextMovePoint;
    }

    private void ChasingTarget()
    {
        agent.SetDestination(target.position);
    }

    private void Attack()
    {
        agent.ResetPath();
        if(distance > attackRange)
        {
            SetState(EnemyState.E_Chase);
        }
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

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(movingPosition, 0.2f);
    }
}
