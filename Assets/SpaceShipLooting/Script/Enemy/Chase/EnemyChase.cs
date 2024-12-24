using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyChase
{
    protected float encounterTimer = 0f;
    protected EnemyData enemyData;
    protected Animator animator;
    protected Vector3 directionToPlayer = Vector3.zero;
    public bool isEncounter;
    protected Damageable targetDamageable;
    protected Transform target;
    protected float distance = 0f;
    protected bool isPlayerDeath = false;

    public virtual void Initialize(EnemyBehaviour _enemy)
    {
        if(_enemy == null)
        {
            Debug.Log("EnemyChase에서 EnemyBehaviour가 null");
        }
        else
        {
            enemyData = _enemy.enemyData;
            animator = _enemy.animator;
            targetDamageable = _enemy.targetDamageable;
            target = _enemy.target;
        }

    }

    public virtual void FirstEncounter(NavMeshAgent agent)
    {

        enemyData.SoundPlay(enemyData.EnemyFirstEncounter);
        encounterTimer += Time.deltaTime;
        enemyData.targetEncounterUI.SetActive(true);
        animator.SetBool("IsPatrol", false);
        agent.enabled = false;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, targetRotation, Time.deltaTime * 2f);
        if (encounterTimer >= enemyData.chaseInterval)
        {
            isEncounter = true;
            enemyData.targetEncounterUI.SetActive(false);
            return;
        }
    }

    public abstract void Chase(NavMeshAgent agent);
}
