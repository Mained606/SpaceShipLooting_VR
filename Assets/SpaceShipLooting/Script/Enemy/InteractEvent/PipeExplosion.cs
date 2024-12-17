using UnityEngine;
using UnityEngine.AI;

public class PipeExplosion : EnemyInteract
{

    public override void Interacting(NavMeshAgent agent)
    {
        if(interactData.interActPosition == null || interactData.interActPosition.Count == 0)
        {
            Debug.Log("Pipe Explosion Event Position : NULL");
            return;
        }
        if (!agent.enabled) agent.enabled = true;

        if(agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            Debug.Log("이벤트목표 도착");
            animator.SetBool("IsLookAround", true);
            agent.enabled = false;
            destination = Vector3.zero;
        }
        else
        {
            agent.enabled = true;
            //isLookAround = false;
            animator.SetBool("IsLookAround", false);
            animator.SetBool("IsPatrol", true);
            agent.SetDestination(destination);
            return;
        }
    }
}
