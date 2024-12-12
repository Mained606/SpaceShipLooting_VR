using UnityEngine;
using UnityEngine.AI;

public class NonePatrol : IEnemyPatrol
{
    public void Initialize(EnemyPatrol _enemyPatrol)
    {
        if (_enemyPatrol == null)
        {
            Debug.Log("EnemyPatrol이 null");
            return;
        }
    }

    public bool Patrol(NavMeshAgent agent)
    {
        if (agent == null)
        {
            Debug.LogWarning("Patrol 함수에서 agent가 null입니다.");
            return false;
        }

        return false;
    }
}
