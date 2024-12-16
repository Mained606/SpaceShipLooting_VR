using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;

public class OriginalPatrol : MonoBehaviour
{
    #region Variables
    [HideInInspector] public EnemyBehaviour enemy;
    [HideInInspector] public Animator animator;
    public SpawnType spawnType;
    private EnemyPatrol patrolBehavior;
    private bool isInfinite;

    //
    public PatrolType patrolShape = PatrolType.Circle;
    [SerializeField] private float circlePatrolRange = 10f;
    public float CirclePatrolRange { get; private set; }

    [SerializeField] private Vector2 rectanglePatrolRange = new Vector2(5f, 5f);
    public Vector2 RectanglePatrolRange { get; private set; }

    [HideInInspector] public Vector3 spawnPosition;
    private Transform[] wayPoints;
    public Transform enemyHeadPosition;

    private Vector3 dir;

    // timer

    private float lookAroundTimer = 0f;
    [SerializeField] private bool isLookAround = false;
    public bool IsLookAround { get; set; }

    // Navmesh
    private NavMeshAgent agent;
    [SerializeField] private float patrolSpeed = 3.5f;
    private Vector3 destination;
    public Vector3 Destination { get; set; }

    [SerializeField] private bool isInterActEvent = false;
    [SerializeField] private InterActEventData interActEventData;

    int currentCount = 0;
    #endregion

    private void Awake()
    {
        RectanglePatrolRange = rectanglePatrolRange;
        CirclePatrolRange = circlePatrolRange;
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<EnemyBehaviour>();
        animator = GetComponent<Animator>();

        spawnPosition = transform.position;
        isInfinite = enemy.enemyData.infinitePatrolMode;

        //SetPatrolBehavior();
    }

    private void Update()
    {
        switch (enemy.enemyData.currentState)
        {
            case EnemyState.E_Patrol:
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
        }
            
    }

    //private void SetPatrolBehavior()
    //{
    //    switch (spawnType)
    //    {
    //        case SpawnType.RandomPatrol:
    //            patrolBehavior = new RandomPatrol();
    //            patrolBehavior.Initialize(this);
    //            break;
    //        case SpawnType.WayPointPatrol:
    //            patrolBehavior = new WayPointPatrol();
    //            patrolBehavior.Initialize(this);
    //            break;
    //        case SpawnType.Normal:
    //            patrolBehavior = new NonePatrol();
    //            patrolBehavior.Initialize(this);
    //            break;
    //    }
    //}

    Vector3 preClossTarget = Vector3.zero;

    private void InvastigateTarget(InterActEventData interActEventData)
    {
        // 1. 예외 처리: 유효하지 않은 데이터
        if (interActEventData.interActPosition == null || interActEventData.interActPosition.Count == 0)
        {
            if (interActEventData.interActType == InterActType.BusterCall)
            {
                agent.enabled = true;
                enemy.enemyData.SetState(EnemyState.E_BusterCall);
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
    private void OnDrawGizmosSelected()
    {
        if (spawnType == SpawnType.RandomPatrol)
        {
            Gizmos.color = Color.green;

            if (patrolShape == PatrolType.Circle)
            {
                Gizmos.DrawWireSphere(spawnPosition, circlePatrolRange);
            }
            else if (patrolShape == PatrolType.Rectangle)
            {
                Vector3 size = new Vector3(rectanglePatrolRange.x, 0f, rectanglePatrolRange.y);
                Gizmos.DrawWireCube(spawnPosition, size);
            }
        }
    }
}
