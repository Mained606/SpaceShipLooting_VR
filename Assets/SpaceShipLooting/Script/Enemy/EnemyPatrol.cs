using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    public enum PatrolType
    {
        Circle,
        Rectangle
    }

    #region Variables
    private Enemy enemy;
    SpawnType spawnType;

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
    [SerializeField] private float waitingTime = 4f;
    private float timer;

    [SerializeField] private float rotationTime = 2f;
    private bool rotatingLeft = true;
    private Quaternion startRotation;
    private Quaternion targetRotation;

    private string rotationTimer = "rotationTimer";

    // material
    private Renderer renderer;
    public Material moveMaterial;

    // Navmesh
    private NavMeshAgent agent;
    [SerializeField] private float patrolSpeed = 3.5f;
    private float navMeshSampleRange = 1f;
    private Vector3 destination;

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
    }

    private void Update()
    {
        Vector3 forward = transform.forward;
        angle = Mathf.Atan2(forward.z, forward.x) * Mathf.Rad2Deg;
        Debug.Log($"현재 오브젝트가 바라보는 방향의 앵글: {angle}");
        switch (enemy.currentState)
        {
            case EnemyState.E_Idle:
                break;

            case EnemyState.E_Move:
                renderer.material = moveMaterial;
                if(spawnType == SpawnType.normal)
                {
                    RandomPatrol();
                }
                else
                {
                    WayPointPatrol();
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

            if(NavMesh.SamplePosition(destination, out NavMeshHit hit, navMeshSampleRange, NavMesh.AllAreas))
            {
                isVaildPoint = true;
                if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    TimerManager.AddTimer(rotationTimer, rotationTime);

                    startRotation = Quaternion.Euler(new Vector3(0f, angle, 0f));
                    LookAround(-90f, 90f);

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
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            //startRotation = transform.rotation;
            TimerManager.AddTimer(rotationTimer, rotationTime);

            LookAround(-45f, 45f);

            if (currentCount == wayPoints.Length)
            {
                currentCount = 0;
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
    }

    //void LookAround(float timer)
    //{
    //    float rotate = 1f;


    //    transform.localRotation = Quaternion.Euler(0f, transform.localRotation.y - rotate, 0f);
    //    if (transform.localEulerAngles == new Vector3(0f, -45f, 0f))
    //    {
    //        transform.localRotation = Quaternion.Euler(0f, transform.localRotation.y + rotate, 0f);
    //    }
    //    else if(transform.localEulerAngles == new Vector3(0f, 45f, 0f))
    //    {
    //        transform.localRotation = Quaternion.Euler(0f, transform.localRotation.y - rotate, 0f);
    //    }

    //}

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

    public void SetSpawnType(SpawnType spawnerType, Transform[] gob)
    {
        spawnType = spawnerType;
        if(gob != null && gob.Length > 0)
        {
            wayPoints = gob;
        }
        else
        {
            Debug.Log("no wayPoints");
        }


    }



    void LookAround(float leftAngle, float rightAngle)
    {
        
        if (!TimerManager.IsContainsKey(rotationTimer))
        {
            TimerManager.AddTimer(rotationTimer, rotationTime);
        }
        else
        {

        }

        float currentTime = TimerManager.currentTime(rotationTimer);

        if (currentTime > 0)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, (rotationTime - currentTime) / rotationTime);
        }
        else
        {
            SetTargetRotation(leftAngle, rightAngle);
        }
    }

    void SetTargetRotation(float leftAngle, float rightAngle)
    {
        rotatingLeft = !rotatingLeft;

        if (rotatingLeft) // 좌측으로 회전
        {
            startRotation = transform.rotation;
            targetRotation = Quaternion.LookRotation(
                Quaternion.AngleAxis(leftAngle, Vector3.up) * transform.forward
            );
            return;
        }
        else // 우측으로 회전
        {
            //startRotation = transform.rotation;
            //targetRotation = Quaternion.LookRotation(
            //    Quaternion.AngleAxis(rightAngle, Vector3.up) * transform.forward
            //);
            //return;
            agent.SetDestination(destination);
        }

        

    }
}
