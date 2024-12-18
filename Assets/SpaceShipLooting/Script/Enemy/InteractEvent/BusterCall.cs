using UnityEngine;
using UnityEngine.AI;

public class BusterCall : EnemyInteract
{
    public override void Interacting(NavMeshAgent agent)
    {
        agent.enabled = true;
        enemyData.SetState(EnemyState.E_BusterCall);
    }
}
