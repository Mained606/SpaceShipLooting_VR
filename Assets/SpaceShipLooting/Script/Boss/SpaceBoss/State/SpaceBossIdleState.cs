using UnityEngine;

public class SpaceBossIdleState : State<BossController>
{
    private SpaceBossController boss;

    public override void OnInitialized()
    {
        boss = context as SpaceBossController;
    }

    public override void OnEnter()
    {
        Debug.Log("보스 아이들 스테이트");
    }

    public override void Update(float deltaTime)
    {
        // 타겟이 범위 안으로 들어오면 무적 상태로 전환
        if(boss.IsTargetInRange())
        {
            boss.SpaceBossDefenceState();
        }
    }
}
