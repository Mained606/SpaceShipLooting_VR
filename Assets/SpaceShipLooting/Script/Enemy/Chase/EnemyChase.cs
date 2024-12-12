using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyChase
{
    protected float encounterTimer = 0f;


    public virtual void Initialize(EnemyBehaviour _enemy)
    {
        if(_enemy == null)
        {
            Debug.Log("EnemyChase에서 EnemyBehaviour가 null");
        }
        else
        {
            
        }

    }

    public virtual void FirstEncounter(NavMeshAgent agent)
    {

    }

    public abstract void Chase(NavMeshAgent agent);
}
