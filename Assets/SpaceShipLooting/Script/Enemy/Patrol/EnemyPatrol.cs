using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyPatrol
{
    protected EnemyData enemyData;
    public abstract void Initialize(EnemyBehaviour _enemyPatrol);

    public abstract bool Patrol(NavMeshAgent agent);
}
