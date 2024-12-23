using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyInteract
{
    protected InteractEventData interactData;
    protected Vector3 destination = Vector3.zero;
    protected Animator animator;
    protected EnemyData enemyData;
    public virtual void Initialize(EnemyBehaviour _enemy)
    {
        if(_enemy == null)
        {
            Debug.LogWarning("EnemyInteract에서 EnemyBehaviour가 NULL");
            return;
        }
        else
        {
            interactData = _enemy.enemyData.enemyInteractData;
            animator = _enemy.animator;
            enemyData = _enemy.enemyData;
            if(_enemy.enemyData.enemyInteractData.interactPosition != null)
            {
                destination = _enemy.enemyData.enemyInteractData.interactPosition.position;
            }
        }
    }

    public abstract void Interacting(NavMeshAgent agent);
}
