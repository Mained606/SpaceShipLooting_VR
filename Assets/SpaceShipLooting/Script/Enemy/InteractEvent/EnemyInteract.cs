using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyInteract
{
    protected InterActEventData interactData;
    protected Vector3 destination = Vector3.zero;
    protected Animator animator;
    public virtual void Initialize(EnemyBehaviour _enemy)
    {
        if(_enemy == null)
        {
            Debug.LogWarning("EnemyInteract에서 EnemyBehaviour가 NULL");
            return;
        }
        else
        {
            interactData = _enemy.interActEventData;
            animator = _enemy.animator;
        }
    }

    public abstract void Interacting(NavMeshAgent agent);
}
