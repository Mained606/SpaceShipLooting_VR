using UnityEngine;
using UnityEngine.AI;

public class WayPointPatrol : EnemyPatrol
{
    bool isLookAround;
    Animator animator;

    public override void Initialize(EnemyBehaviour _enemy)
    {
        if (_enemy == null)
        {
            Debug.Log("EnemyPatrol이 null");
            return;
        }
        else
        {
            isLookAround = _enemy.IsLookAround;
            animator = _enemy.animator;
        }
    }

    public override bool Patrol(NavMeshAgent agent)
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
