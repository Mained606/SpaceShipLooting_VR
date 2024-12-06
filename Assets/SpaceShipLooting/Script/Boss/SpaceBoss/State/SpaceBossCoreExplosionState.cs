using UnityEngine;

public class SpaceBossCoreExplosionState : State<BossController>
{
    private SpaceBossController boss;

    private float timer;
    private float coreExplosionDuration;

    public override void OnInitialized()
    {
        boss = context as SpaceBossController;
        coreExplosionDuration = boss.coreExplosionDuration;
    }

    public override void OnEnter()
    {
        Debug.Log("보스 코어 폭발 스테이트 시작");
        timer = 0f;
    }

    public override void Update(float deltaTime)
    {
        timer += deltaTime;
        if(timer >= coreExplosionDuration)
        {
            // 코어 폭발 스킬 효과 발동


            // 공격 후 다시 디펜스 스테이트로 변경
            boss.SpaceBossDefenceState();
        }
    }
}
