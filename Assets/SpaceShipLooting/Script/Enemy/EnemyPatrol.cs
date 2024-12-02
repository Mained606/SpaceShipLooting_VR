using System.Runtime.CompilerServices;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    public enum PatrolType
    {
        Circle,
        Rectangle,
        None
    }

    #region Variables
    private Enemy enemy;
    [SerializeField] SpawnType spawnType;

    //
    public PatrolType patrolShape = PatrolType.Circle;
    [SerializeField] private float circlePatrolRange = 10f;
    [SerializeField] private Vector2 rectanglePatrolRange = new Vector2(5f, 5f);
    public Transform spawnPoint;
    private Vector3 spawnPosition;
    private Vector3 nextMovePoint;
    private Transform[] wayPoints;

    private Vector3 dir;

    // timer

    [SerializeField] private float rotationTime = 2f;
    private bool rotatingLeft = true;
    [SerializeField] private bool isLookAround = false;
    private Quaternion startRotation;
    private Quaternion targetRotation;

    [SerializeField] private float waitingTime = 4f;
    private BasicTimer rotationTimer;
    private float timer;
    [SerializeField] private float rotationAngle = 45f;
    // material
    private Renderer renderer;
    public Material moveMaterial;

    // Navmesh
    private NavMeshAgent agent;
    [SerializeField] private float patrolSpeed = 3.5f;
    private float navMeshSampleRange = 1f;
    private Vector3 destination;

    [SerializeField] private bool isInterActEvent = false;
    [SerializeField] private InterActEventData interActEventData;

    float angle;
    #endregion

    private void Awake()
    {
        spawnType = SpawnType.normal;
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();
        renderer = GetComponent<Renderer>();

        nextMovePoint = spawnPoint.position;
        spawnPosition = spawnPoint.position;
        timer = waitingTime;
        rotationTimer = new BasicTimer(rotationTime);
    }

    private void Update()
    {
        switch (enemy.currentState)
        {
            case EnemyState.E_Idle:
                break;

            case EnemyState.E_Move:
                renderer.material = moveMaterial;
                if (isInterActEvent)
                {
                    InvastigateTarget(interActEventData);
                }
                if (isLookAround)
                {
                    LookAround(rotationAngle);
                }
                else
                {
                    if (spawnType == SpawnType.RandomPatrol)
                    {
                        RandomPatrol();
                    }
                    else if(spawnType == SpawnType.WayPointPatrol)
                    {
                        WayPointPatrol();
                    }
                    else if(spawnType == SpawnType.normal)
                    {
                        NonePatrol();
                    }
                }
                break;

            case EnemyState.E_Chase:
                break;

            case EnemyState.E_Attack:
                break;

            case EnemyState.E_Death:
                break;
        }
            
    }

    Vector3 preClossTarget = Vector3.zero;

    private void InvastigateTarget(InterActEventData interActEventData)
    {
        // 1. 예외 처리: 유효하지 않은 데이터
        if (interActEventData.interActPosition == null || interActEventData.interActPosition.Count == 0)
        {
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
                isInterActEvent = false;
                agent.enabled = false;
                preClossTarget = Vector3.zero;
                return;
            }

            // 목표지점으로 계속 이동
            agent.SetDestination(preClossTarget);
            return;
        }

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

    private Vector3 GetCircleMovePoint()
    {
        nextMovePoint = spawnPosition + Random.insideUnitSphere * circlePatrolRange;
        nextMovePoint.y = spawnPosition.y;

        return nextMovePoint;
    }

    private Vector3 GetRectangleMovePoint()
    {
        float halfWidth = rectanglePatrolRange.x / 2;
        float halfHeight = rectanglePatrolRange.y / 2;

        float x = Random.Range(-halfWidth, halfWidth);
        float z = Random.Range(-halfHeight, halfHeight);

        nextMovePoint = new Vector3(spawnPosition.x + x, spawnPosition.y, spawnPosition.z + z);

        return nextMovePoint;
    }

    public void RandomPatrol()
    {
        bool isVaildPoint = false;
        while (!isVaildPoint)
        {
            destination = patrolShape == PatrolType.Circle
                ? GetCircleMovePoint()
                : GetRectangleMovePoint();
            agent.enabled = true;

            if(NavMesh.SamplePosition(destination, out NavMeshHit hit, navMeshSampleRange, NavMesh.AllAreas))
            {
                isVaildPoint = true;
                if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    isLookAround = true;
                    agent.enabled = false;

                    //timer -= Time.deltaTime;
                    //if (timer < 0f)
                    //{
                    //    agent.speed = patrolSpeed;
                    //    agent.SetDestination(destination);
                    //    timer = waitingTime;
                    //}
                }
            }
        }
    }
    int currentCount = 0;

    public void WayPointPatrol()
    {
        if (!isLookAround && agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            isLookAround = true;
            agent.enabled = false;
        }

        //timer -= Time.deltaTime;
        //LookAround(timer);
        //if (timer < 0f)
        //{

        //    Debug.LogWarning("time: " + timer.ToString());
        //    agent.speed = patrolSpeed;
        //    agent.SetDestination(wayPoints[currentCount].position);
        //    timer = waitingTime;
        //    currentCount++;
        //}
    }

    public void NonePatrol()
    {
        // waiting player
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
            Debug.Log("no wayPoints");
        }
    }

    bool turnCountOk = false;

    void LookAround(float Angle)
    {
        if (rotationTimer.IsRunning)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, 1f - rotationTimer.RemainingPercent);
        }
        else
        {
            TimerManager.Instance.StartTimer(rotationTimer);
            rotatingLeft = !rotatingLeft;

            if (rotatingLeft) 
            {
                Debug.Log($"좌측 회전 시작: 현재 웨이포인트 {currentCount}");
                startRotation = transform.rotation;
                targetRotation = Quaternion.Euler(0f, transform.eulerAngles.y - (Angle * 2), 0f);
                turnCountOk = true; 
            }
            else 
            {
                if (turnCountOk) 
                {
                    Debug.Log($"우측 회전 완료: 다음 행동 준비");
                    isLookAround = false;
                    turnCountOk = false;
                    rotatingLeft = true;
                    agent.enabled = true;

                    if (spawnType == SpawnType.RandomPatrol)
                    {
                        Debug.Log("랜덤 이동 시작");
                        agent.speed = patrolSpeed;
                        agent.SetDestination(destination);
                    }
                    else if(spawnType == SpawnType.WayPointPatrol)
                    {
                        Debug.Log("웨이포인트 이동 시작");
                        agent.speed = patrolSpeed;
                        agent.SetDestination(wayPoints[currentCount].position);
                        currentCount++;
                        if (currentCount >= wayPoints.Length)
                        {
                            currentCount = 0;
                        }
                    }
                    return;
                }
                Debug.Log($"우측 회전 시작: 현재 웨이포인트 {currentCount}");
                startRotation = transform.rotation;
                targetRotation = Quaternion.Euler(0f, transform.eulerAngles.y + Angle, 0f);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        if(patrolShape == PatrolType.Circle)
        {
            Gizmos.DrawWireSphere(spawnPoint.position, circlePatrolRange);
        }
        else if(patrolShape == PatrolType.Rectangle)
        {
            Vector3 size = new Vector3(rectanglePatrolRange.x, 0f, rectanglePatrolRange.y);
            Gizmos.DrawWireCube(spawnPoint.position, size);
        }
    }
}
