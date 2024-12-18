using UnityEngine;
using UnityEngine.AI;

public class PipeExplosion : EnemyInteract
{
    private Vector3 modifyRotation = new Vector3(-90f, 0f, 0f);
    public override void Interacting(NavMeshAgent agent)
    {
        if(interactData.interactPosition == null)
        {
            Debug.Log("Pipe Explosion Event Position : NULL");
            return;
        }
        if (!agent.enabled) agent.enabled = true;

        agent.SetDestination(destination);
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            Debug.Log("이벤트목표 도착");
            Quaternion targetRotation = Quaternion.LookRotation(modifyRotation);
            agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, targetRotation, Time.deltaTime * 2f);
            animator.SetBool("IsLookAround", true);
            agent.enabled = false;
        }
        else
        {
            agent.enabled = true;
            enemyData.isLookAround = false;
            animator.SetBool("IsLookAround", false);
            animator.SetBool("IsPatrol", true);
            agent.SetDestination(destination);
            return;
        }

    }
}
