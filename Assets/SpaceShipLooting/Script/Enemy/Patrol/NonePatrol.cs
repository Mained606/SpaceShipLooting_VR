using UnityEngine;
using UnityEngine.AI;

public class NonePatrol : IEnemyPatrol
{
    private Vector3 spawnPosition;
    private Vector3 currentPosition;
    private float distance;
    private Animator animator;
    public void Initialize(EnemyPatrol _enemyPatrol)
    {
        if (_enemyPatrol == null)
        {
            Debug.Log("EnemyPatrol이 null");
            return;
        }

        spawnPosition = _enemyPatrol.spawnPosition;
        currentPosition = _enemyPatrol.transform.position;
        animator = _enemyPatrol.animator;
    }

    public bool Patrol(NavMeshAgent agent)
    {
        if (agent == null)
        {
            Debug.LogWarning("Patrol 함수에서 agent가 null입니다.");
            return false;
        }
        Return(agent);
        return false;
    }

    public void Return(NavMeshAgent agent)
    {
        currentPosition = agent.transform.position;
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            distance = Vector3.Distance(currentPosition, spawnPosition);
            if (distance >= agent.remainingDistance)
            {
                agent.SetDestination(spawnPosition);
            }
        }
    }
}
