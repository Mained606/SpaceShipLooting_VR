using UnityEngine;
using UnityEngine.AI;

public class WayPointPatrol : IEnemyPatrol
{
    bool isLookAround;
    Animator animator;

    public void Initialize(EnemyPatrol _enemyPatrol)
    {
        if (_enemyPatrol == null)
        {
            Debug.Log("EnemyPatrol이 null");
            return;
        }
        else
        {
            isLookAround = _enemyPatrol.IsLookAround;
            animator = _enemyPatrol.animator;
        }
    }

    public bool Patrol(NavMeshAgent agent)
    {
        if (!isLookAround && agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            isLookAround = true;
            agent.enabled = false;
        }
        else
        {
            animator.SetBool("IsPatrol", true);
            isLookAround = false;
        }
        return isLookAround;
    }
}
