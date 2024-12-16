using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class AggressiveChase : EnemyChase
{
    private float chaseTimer = 0;
    public override void Chase(NavMeshAgent agent)
    {
        distance = Vector3.Distance(target.position, agent.transform.position);
        directionToPlayer = (target.position - agent.transform.position).normalized;
        if (!isEncounter)
        {
            FirstEncounter(agent);
        }
        else
        {
            if (distance <= enemyData.deadZone)
            {
                Debug.Log("플레이어 잡힙(즉사)");
                targetDamageable.InflictDamage(100f);
            }
            chaseTimer += Time.deltaTime;
            animator.SetBool("IsAttack", false);
            animator.SetBool("IsChase", true);
            agent.enabled = true;
            agent.speed = enemyData.moveSpeed;
            agent.SetDestination(target.position);
            if (chaseTimer >= enemyData.chaseInterval)
            {
                chaseTimer = 0f;
                animator.SetBool("IsChase", false);
                enemyData.SetState(EnemyState.E_Attack);
            }
        }
    }
}
