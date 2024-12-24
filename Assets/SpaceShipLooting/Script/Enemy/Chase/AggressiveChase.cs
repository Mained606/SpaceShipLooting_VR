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
                if (!isPlayerDeath)
                {
                    targetDamageable.InflictDamage(100f);
                    isPlayerDeath = true;
                }
            }
            chaseTimer += Time.deltaTime;
            AudioManager.Instance.Play("EnemyWalk");
            enemyData.SoundPlay(enemyData.EnemyChase);
            animator.SetBool("IsAttack", false);
            animator.SetBool("IsChase", true);
            agent.enabled = true;
            agent.speed = enemyData.moveSpeed;
            agent.SetDestination(target.position);
            if (chaseTimer >= enemyData.chaseInterval)
            {
                chaseTimer = 0f;
                enemyData.SoundStop();
                animator.SetBool("IsChase", false);
                enemyData.SetState(EnemyState.E_Attack);
            }
        }
    }
}
