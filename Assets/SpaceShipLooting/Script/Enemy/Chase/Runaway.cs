using UnityEngine;
using UnityEngine.AI;

public class Runaway : EnemyChase
{
    public override void Chase(NavMeshAgent agent)
    {

        if (!isEncounter)
        {
            FirstEncounter(agent);
        }
        else
        {
            Debug.Log("Runaway Chase");
        }
    }
}
