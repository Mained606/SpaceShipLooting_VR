using UnityEngine;

public class SpaceBossIdleState : State<BossController>
{
    public override void OnInitialized()
    {

    }

    public override void OnEnter()
    {
        Debug.Log("보스 아이들 스테이트");
    }

    public override void Update(float deltaTime)
    {
        SpaceBossController boss = context as SpaceBossController;
        if(boss.IsTargetInRange() == true)
        {
            boss.ChangeState<SpaceBossAttackState>();
        }
    }

    public override void OnExit()
    {

    }
}
