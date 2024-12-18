using UnityEngine;
using UnityEngine.AI;

public class None : EnemyInteract
{
    public override void Interacting(NavMeshAgent agent)
    {
        Debug.Log("Event NONE");
    }
}
