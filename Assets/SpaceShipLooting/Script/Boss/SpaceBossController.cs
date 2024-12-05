using UnityEngine;

public class SpaceBossController : BossController
{
    private Transform playerTransform;
    
    protected override void Start()
    {
        base.Start();

        stateMachine.AddState(new SpaceBossIdleState());

        ChangeState<SpaceBossIdleState>();

        // Player 태그가 붙은 오브젝트를 서치
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    public void SpaceBossIdle()
    {
        ChangeState<SpaceBossIdleState>();
    }
}
