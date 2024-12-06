using UnityEngine;

public class SpaceBossAttackState : State<BossController>
{
    private SpaceBossController boss;

    public override void OnInitialized()
    {
        boss = context as SpaceBossController;
    }
    public override void OnEnter()
    {
        // 보스의 랜덤 패턴 선택 및 실행
        boss.SpaceBossAttack();
    }

    public override void Update(float deltaTime)
    {
        
    }
}
