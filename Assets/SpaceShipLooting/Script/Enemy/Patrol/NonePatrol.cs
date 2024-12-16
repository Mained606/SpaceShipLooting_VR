using UnityEngine;
using UnityEngine.AI;

public class NonePatrol : EnemyPatrol
{
    private Vector3 spawnPosition;
    private Vector3 currentPosition;
    private float distance;
    private Animator animator;
    public override void Initialize(EnemyBehaviour _enemy)
    {
        if (_enemy == null)
        {
            Debug.Log("EnemyPatrol이 null");
            return;
        }

        spawnPosition = _enemy.spawnPosition;
        currentPosition = _enemy.transform.position;
        animator = _enemy.animator;
    }

    public override bool Patrol(NavMeshAgent agent)
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
