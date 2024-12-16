using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RandomPatrol : EnemyPatrol
{
    PatrolType patrolType;
    Animator animator;
    
    Vector3 destination;
    Vector3 nextMovePoint;
    Vector3 spawnPosition;
    float circlePatrolRange;
    Vector2 rectanglePatrolRange;

    bool isLookAround;
    bool isEnter;

    public override void Initialize(EnemyBehaviour _enemy)
    {

        if(_enemy == null)
        {
            Debug.Log("EnemyPatrol이 null");
            return;
        }
        else
        {

            patrolType = _enemy.enemyData.enemyPatrolType;
            destination = _enemy.Destination;
            isLookAround = _enemy.IsLookAround;
            nextMovePoint = _enemy.transform.position;
            spawnPosition = _enemy.transform.position;
            circlePatrolRange = _enemy.enemyData.circlePatrolRange;
            rectanglePatrolRange = _enemy.enemyData.rectanglePatrolRange;
            animator = _enemy.animator;
            isEnter = false;
        }
    }

    public override bool Patrol(NavMeshAgent agent)
    {
        if(agent == null)
        {
            Debug.LogWarning("Patrol 함수에서 agent가 null입니다.");
            return false;
        }

        bool isVaildPoint = false;
        while (!isVaildPoint)
        {
            destination = patrolType == PatrolType.Circle
                ? CalculateCircleMovePoint()
                : CalculateRectangleMovePoint();
            agent.enabled = true;

            if (NavMesh.SamplePosition(destination, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                isVaildPoint = true;
                animator.SetBool("IsPatrol", true);
                agent.SetDestination(destination);
                if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    isLookAround = true;
                    agent.enabled = false;
                    isEnter = false;
                }
                else
                {
                    isLookAround = false;
                }
            }
            
        }
        return isLookAround;

    }

    private Vector3 CalculateCircleMovePoint()
    {
        if(isEnter == true)
        {
            return nextMovePoint;
        }
        else
        {
            isEnter = true;
            nextMovePoint = spawnPosition + UnityEngine.Random.insideUnitSphere * circlePatrolRange;
            nextMovePoint.y = spawnPosition.y;
            return nextMovePoint;
        }
    }

    private Vector3 CalculateRectangleMovePoint()
    {
        if (isEnter == true)
        {
            return nextMovePoint;
        }
        else
        {
            float halfWidth = rectanglePatrolRange.x / 2;
            float halfHeight = rectanglePatrolRange.y / 2;

            float x = UnityEngine.Random.Range(-halfWidth, halfWidth);
            float z = UnityEngine.Random.Range(-halfHeight, halfHeight);

            nextMovePoint = new Vector3(spawnPosition.x + x, spawnPosition.y, spawnPosition.z + z);
            isEnter = true;
            return nextMovePoint;
        }
    }
}
