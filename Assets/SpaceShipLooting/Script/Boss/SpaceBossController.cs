using UnityEngine;

public class SpaceBossController : BossController
{
    [SerializeField] private float searchRange = 15f;
    [SerializeField] private Transform target;
    
    protected override void Start()
    {
        base.Start();

        stateMachine.AddState(new SpaceBossIdleState());
        stateMachine.AddState(new SpaceBossAttackState());

        ChangeState<SpaceBossIdleState>();

        // Player 태그가 붙은 오브젝트를 서치
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    public bool IsTargetInRange()
    {
        if (target == null) return false;
        return Vector3.Distance(transform.position, target.position) <= searchRange;
    }

    public void SpaceBossIdle()
    {
        ChangeState<SpaceBossIdleState>();
    }

    public void SpaceBossAttackState()
    {
        ChangeState<SpaceBossAttackState>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRange);
    }  
}
