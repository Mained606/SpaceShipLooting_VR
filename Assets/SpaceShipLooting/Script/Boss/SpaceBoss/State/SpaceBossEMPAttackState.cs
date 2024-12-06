using UnityEngine;

public class SpaceBossEMPAttackState : State<BossController>
{
    private SpaceBossController boss;

    private float timer;
    private float empAttackDuration;

    public override void OnInitialized()
    {
        boss = context as SpaceBossController;
        empAttackDuration = boss.empAttackDuration;
    }

    public override void OnEnter()
    {
        Debug.Log("보스 EMP어택 스테이트 시작");
        timer = 0f;
    }

    public override void Update(float deltaTime)
    {
        timer += deltaTime;
        if(timer >= empAttackDuration)
        {
            // 범위 스킬 효과 발동


            // 공격 후 다시 디펜스 스테이트로 변경
            boss.SpaceBossDefenceState();
        }
    }
}
