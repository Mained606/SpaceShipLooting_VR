using UnityEngine;
using UnityEngine.AI;

public interface IEnemyPatrol
{
    void Initialize(EnemyPatrol _enemyPatrol);
    bool Patrol(NavMeshAgent agent);
}
