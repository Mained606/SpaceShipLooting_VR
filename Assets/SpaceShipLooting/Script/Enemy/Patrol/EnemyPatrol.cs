using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyPatrol
{
    public abstract void Initialize(EnemyBehaviour _enemyPatrol);

    public abstract bool Patrol(NavMeshAgent agent);
}
