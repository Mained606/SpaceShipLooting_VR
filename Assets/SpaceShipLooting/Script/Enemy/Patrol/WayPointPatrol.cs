using UnityEngine;
using UnityEngine.AI;

public class WayPointPatrol : IEnemyPatrol
{
    bool isLookAround;

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
            isLookAround = false;
        }
        return isLookAround;
    }
}
